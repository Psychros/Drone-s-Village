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
    [HideInInspector] public Chunk[,] chunks;
    public int width = 8, 
               height = 8;
    [HideInInspector] public float offsetX, offsetZ;


    // Use this for initialization
    void Start()
    {
        instance = this;
        chunks = new Chunk[width, height];

        //Initialize the Random offset
        Random.seed = Random.Range(int.MinValue, int.MaxValue);
        offsetX = Random.value * 10000;
        offsetZ = Random.value * 10000;

        //Generate the world
        generate();
        showWorld();
    }



    public void generate()
    {
        //Create the chunks
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject g = new GameObject();
                g.name = "Chunk(" + x + ", " + z + ")";
                chunks[x, z] = g.AddComponent<Chunk>() as Chunk;
                chunks[x, z].setPosition(x, z);
                chunks[x, z].transform.SetParent(gameObject.transform);
            }
        }


        //Place the spaceship and focus the camera on it
        //Place a drone over the spaceship
        /*while (true)
        {
            int x = (int)(Random.value * width * Chunk.chunkSize), 
                z = (int)(Random.value * height * Chunk.chunkSize);
            print(x + "   " + z);

            Chunk c = getChunkAt(x, z);
            if (c.getBiomWorldCoords(x, z) == Bioms.Plain)
            {
                Vector3 shipPos = Hexagon.getWorldPosition(x, z);

                //Place the ship
                changeStructure(x, z, Structures.Spaceship);

                //Place the first drone over the ship
                GameObject g = Instantiate(droneModel);
                drone = g.AddComponent<NPC>();
                g.transform.position = shipPos + droneModel.transform.position;

                //Focus the camera
                Camera.main.transform.position = shipPos + new Vector3(0, Camera.main.transform.position.y, -8f);
                break;
            }
        }*/
    }


    //Make the calculated world visible
    public void showWorld()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                chunks[x, z].generate();
            }
        }
    }


    //Gets the chunk at the hexagonposition x, z
    public Chunk getChunkAt(int x, int z)
    {
        int posX = x / Chunk.chunkSize;
        int posZ = z / Chunk.chunkSize;

        return chunks[posX, posZ];
    }

    //Params are worldcoords
    public Vector2Int getPositionInChunk(int x, int z)
    {
        int posX = x % Chunk.chunkSize;
        int posZ = z % Chunk.chunkSize;

        return new Vector2Int(posX, posZ);
    }


    //Gets the biom at the hexagonposition x, z
    public Bioms getBiom(int x, int z)
    {
        Chunk c = getChunkAt(x, z);
        return c.getBiomWorldCoords(x, z);
    }


    //changes the biom of a hexagon
    public void changeBiom(int x, int z, Bioms newBiom)
    {
        Chunk c = getChunkAt(x, z);
        c.changeBiomGlobalCoords(x, z, newBiom);
    }


    //Gets the structure at the hexagonposition x, z
    public Structures getStructure(int x, int z)
    {
        Chunk c = getChunkAt(x, z);
        return c.getStructureGlobalCoords(x, z);
    }

    //Gets the structure at the hexagonposition x, z
    public GameObject getStructureGameObject(int x, int z)
    {
        Chunk c = getChunkAt(x, z);
        return c.getStructureGameObjectGlobalCoords(x, z);
    }

    //Changes a structure with the hexCoords
    public void changeStructure(int x, int z, Structures newStructure)
    {
        Chunk c = getChunkAt(x, z);
        c.changeStructureGlobalCoords(x, z, newStructure);
    }
}
