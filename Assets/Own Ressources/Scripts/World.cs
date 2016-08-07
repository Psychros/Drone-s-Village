using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class World : MonoBehaviour
{
    [HideInInspector]
    public static World instance;

    public GameObject[] biomModels = new GameObject[6];
    public GameObject[] structureModels = new GameObject[System.Enum.GetNames(typeof(BiomModels)).Length-1];
    public BiomData[] biomsData = new BiomData[System.Enum.GetNames(typeof(Bioms)).Length];
    public GameObject droneModel;
    [HideInInspector] public NPC drone;
    [HideInInspector] public int[,] worldBiomes;
    [HideInInspector] public GameObject[,] hexagons;
    [HideInInspector] public GameObject[,] structures;              //Includes all structures on the hexagons
    public int width = 50, 
               height = 50;


    // Use this for initialization
    void Start()
    {
        instance = this;
        worldBiomes = new int[width, height];
        hexagons = new GameObject[width, height];
        structures = new GameObject[width, height];

        //Generate the world
        generate();
        showWorld();
        //smoothHexagonTextureTransition();
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
                    worldBiomes[i, j] = Random.value > .5 ? (Random.value > .2 ? (int)Bioms.Plain : (int)Bioms.PlainDandelion) : (int)Bioms.Forest;
                else if (biom > .49f)
                    worldBiomes[i, j] = Random.value > .2 ? (int)Bioms.Desert : (int)Bioms.DesertShell;
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
                g = Instantiate(biomModels[(int)biomData.biomModel]);
                g.transform.position = Hexagon.getWorldPosition(i, j);
                g.transform.parent = transform;
                hexagons[i, j] = g;

                //Generate the structure
                if ((int)biomData.structure != (int)Structures.None)
                {
                    structures[i, j] = Instantiate<GameObject>(structureModels[(int)biomData.structure]);
                    structures[i, j].transform.position = Hexagon.getWorldPosition(i, j);
                    structures[i, j].transform.parent = this.transform;
                }
                else
                    structures[i, j] = null;
            }
        }
    }


    //Smoothes the texturetransitions
    //There is anywhere a bug
    public void smoothHexagonTextureTransition()
    {
        string s = "";

        for (int i = 1; i < width; i++)
        {
            for (int j = 1; j < height; j++)
            {
                MeshRenderer renderer = hexagons[i, j].GetComponent<MeshRenderer>();
                Texture2D texture = renderer.material.mainTexture as Texture2D;
                
                MeshRenderer rendererLeft = hexagons[i-1, j].GetComponent<MeshRenderer>();
                Texture2D textureLeft = rendererLeft.material.mainTexture as Texture2D;

                Color[] cs = texture.GetPixels(0, 0, 20, 512);
                Color[] cs2 = textureLeft.GetPixels(0, 0, 20, 512);
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 512; y++)
                    {
                        Color c  = cs[y + x * 512];
                        Color c2 = cs2[y + x * 512];
                        s = ((c.r + c2.r) / 2).ToString();
                        texture.SetPixel(x, y, new Color((c.r+c2.r)/2, (c.g + c2.g) / 2, (c.b + c2.b) / 2, c.a));
                    }
                }
                texture.Apply();
            }
        }
        print(s);
    }


    //changes the biom of a hexagon
    public void changeBiom(Bioms newBiom, Vector3 v)
    {
        //Get the position of the hexagon
        Vector2Int posHex = Hexagon.getHexPositionInt(v);

        worldBiomes[posHex.x, posHex.z] = (int)newBiom;
    }

    //Changes a structure with the hexCoords
    public GameObject changeStructure(int x, int z, Structures newStructure, float newHeight = 0)
    {
        //Remove the old structure
        if (structures[x, z] != null)
        {
            Destroy(structures[x, z]);
        }

        //Place the new structure
        if(newStructure != Structures.None)
        {
            Vector3 pos = Hexagon.getWorldPosition(x, z);
            structures[x, z] = Instantiate(structureModels[(int)newStructure]);
            structures[x, z].transform.position = pos + new Vector3(0, newHeight, 0);
        }
        else
            structures[x, z] = null;

        return structures[x, z];
    }

    public GameObject changeStructure(Vector3 pos, Structures newStructure, float newHeight = 0)
    {
        Vector2Int v = Hexagon.getHexPositionInt(pos);
        return changeStructure(v.x, v.z, newStructure, newHeight);
    }
}
