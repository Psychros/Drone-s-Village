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
    [HideInInspector] public GameObject[,] hexagonCollider;        
    [HideInInspector] public GameObject[,] structures;              //Includes all structures on the hexagons
    [HideInInspector] private List<GameObject>[] hexagonTypeLists;  //Contains all hexagons of every material
    [HideInInspector] public GameObject[] hexagons;                 //Includes all different hexagons
    public int width = 50, 
               height = 50;



    // Use this for initialization
    void Start()
    {
        instance = this;
        worldBiomes = new int[width, height];
        hexagonCollider = new GameObject[width, height];
        structures = new GameObject[width, height];
        hexagons = new GameObject[biomModels.Length];
        hexagonTypeLists = new List<GameObject>[biomModels.Length];

        for (int i = 0; i < hexagonTypeLists.Length; i++)
            hexagonTypeLists[i] = new List<GameObject>();

        //Generate the world
        generate();
        showWorld();
        createHexagonMeshs();
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
                hexagonTypeLists[biomData.biomModel].Add(g);
                hexagonCollider[i, j] = g;

                //Generate the structure
                if (biomData.structure != (int)Structures.None)
                {
                    structures[i, j] = Instantiate<GameObject>(structureModels[biomData.structure]);
                    structures[i, j].transform.position = Hexagon.getWorldPosition(i, j);
                    structures[i, j].transform.parent = this.transform;
                }
                else
                    structures[i, j] = null;
            }
        }
    }


    //Creates the visible hexagonmeshs
    public void createHexagonMeshs()
    {
        //Get the materials of the hexagons
        Material[] materials = new Material[biomModels.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = biomModels[i].GetComponent<MeshRenderer>().sharedMaterial;
        }

        //Create the new GameObjects
        MeshRenderer[] renderer = new MeshRenderer[biomModels.Length];
        MeshFilter[] filter = new MeshFilter[biomModels.Length];
        for (int i = 0; i < hexagons.Length; i++)
        {
            hexagons[i] = new GameObject();
            renderer[i] = hexagons[i].AddComponent<MeshRenderer>();
            renderer[i].material = materials[i];
            filter[i] = hexagons[i].AddComponent<MeshFilter>();
            hexagons[i].transform.position = Vector3.zero;
            //hexagons[i].transform.parent = this.transform;
        }

        //Get the MeshFilters
        MeshFilter[][] meshFilter = new MeshFilter[hexagonTypeLists.Length][];
        for (int i = 0; i < meshFilter.Length; i++)
        {
            meshFilter[i] = new MeshFilter[hexagonTypeLists[i].Count];
            for (int j = 0; j < meshFilter[i].Length; j++)
            {
                meshFilter[i][j] = hexagonTypeLists[i].ElementAt(j).GetComponent<MeshFilter>();
            }
        }

        //Create the CombineInstances
        CombineInstance[][] combines = new CombineInstance[hexagonTypeLists.Length][];
        for (int i = 0; i < combines.Length; i++)
        {
            combines[i] = new CombineInstance[meshFilter[i].Length];
            for (int j = 0; j < combines[i].Length; j++)
            {
                combines[i][j].mesh = meshFilter[i][j].sharedMesh;
                combines[i][j].transform = meshFilter[i][j].transform.localToWorldMatrix;
                Destroy(meshFilter[i][j].GetComponent<MeshRenderer>()); //Destroy the old MeshRenderer
            }
        }

        //Combine the Meshes
        for (int i = 0; i < hexagons.Length; i++)
        {
            filter[i].mesh.CombineMeshes(combines[i]);
        }
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
