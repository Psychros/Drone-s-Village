using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class World : MonoBehaviour
{
    [HideInInspector]
    public static World instance;

    public GameObject[] biomModels = new GameObject[4];
    public GameObject[] structureModels = new GameObject[5];
    public BiomData[] biomsData = new BiomData[System.Enum.GetNames(typeof(Bioms)).Length];
    public GameObject droneModel;
    [HideInInspector] public NPC drone;
    [HideInInspector] public int[,] worldBiomes;
    [HideInInspector] public GameObject[,] world;       
    [HideInInspector] public GameObject[,] structures;  //Includes all structures on the hexagons
    [HideInInspector] public GameObject[] hexagons;     //Includes all different hexagons
    public int width = 50, 
               height = 50;



    // Use this for initialization
    void Start()
    {
        instance = this;
        worldBiomes = new int[width, height];
        world = new GameObject[width, height];
        structures = new GameObject[width, height];
        hexagons = new GameObject[biomModels.Length];

        //Generate the world
        generate();
        showWorld();
    }



    public void generate()
    {
        //Initialize the Random
        Random.seed = Random.Range(int.MinValue, int.MaxValue);
        float offsetX = Random.value * 10000,
              offsetZ = Random.value * 10000;

        //Create the Cards
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //Select the correct terrain
                float biom = Mathf.PerlinNoise(offsetX + (float)i / width * 5f, offsetZ + (float)j / height * 5f);

                if (biom > .85f)
                    worldBiomes[i, j] = (int)Bioms.HighMountain;
                else if (biom > .8f)
                    worldBiomes[i, j] = (int)Bioms.Mountain;
                else if (biom > .75f)
                {
                    if (Random.value > .3)
                        worldBiomes[i, j] = (int)Bioms.Mountain;
                    else
                        worldBiomes[i, j] = (int)Bioms.StonePlain;
                }
                else if (biom > .52f)
                    worldBiomes[i, j] = Random.value > .5 ? (int)Bioms.Plain : (int)Bioms.Forest;
                else if (biom > .49f)
                    worldBiomes[i, j] = (int)Bioms.Desert;
                else if (biom > .4f)
                    worldBiomes[i, j] = Random.value > .05 ? (int)Bioms.Ocean : (int)Bioms.OceanMountain;
                else
                    worldBiomes[i, j] = (int)Bioms.Ocean;
            }
        }



        //Place the spaceship and focus the camera on it
        while (true)
        {
            int x = (int)(Random.value * width), 
                z = (int)(Random.value * height);
            if (worldBiomes[x, z] == (int)Bioms.Plain)
            {
                Vector3 shipPos = Hexagon.getWorldPosition(x, z);

                //Place the ship
                worldBiomes[x, z] = (int)Bioms.SpaceShip;

                //Place the first drone over the ship
                GameObject g = Instantiate(droneModel);
                drone = g.AddComponent<NPC>();
                g.transform.position = shipPos + droneModel.transform.position;

                //Focus the camera
                Camera.main.transform.position = shipPos + new Vector3(0, Camera.main.transform.position.y, -8f);
                break;
            }
        }
    }


    //Make the calculated world visible
    public void showWorld()
    {
        GameObject g;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                BiomData biomData = biomsData[worldBiomes[i, j]];

                //Generate the ground
                g = Instantiate<GameObject>(biomModels[biomData.biomModel]);
                g.transform.position = Hexagon.getWorldPosition(i, j);
                g.transform.parent = transform;
                world[i, j] = g;

                //Generate the structure
                if (biomData.structure >= 0)
                {
                    structures[i, j] = Instantiate<GameObject>(structureModels[biomData.structure]);
                    structures[i, j].transform.position = Hexagon.getWorldPosition(i, j);
                }
                else
                    structures[i, j] = null;
            }
        }
    }



    //The changes the biom of a hexagon
    public GameObject changeBiom(Bioms oldBiom, Bioms newBiom, Vector3 v)
    {
        //Get the position of the hexagon
        Vector2Int posHex = Hexagon.getHexPositionInt(v);

        //Remove the old GameObject and create a new one
        if (worldBiomes[posHex.x, posHex.z] == (int)oldBiom)
        {
            Destroy(world[posHex.x, posHex.z]);

            GameObject g = Instantiate(biomModels[(int)newBiom]);
            g.transform.position = v;
            g.transform.parent = transform;
            world[posHex.x, posHex.z] = g;
            worldBiomes[posHex.x, posHex.z] = (int)newBiom;

            return g;
        }

        return null;
    }
}
