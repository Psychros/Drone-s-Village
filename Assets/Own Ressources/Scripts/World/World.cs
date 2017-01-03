using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class World : MonoBehaviour
{
    [HideInInspector]
    public static World instance;

    public GameObject[] biomModels      = new GameObject[System.Enum.GetNames(typeof(BiomModels)).Length];
    public GameObject[] hexagonBorderModels    = new GameObject[System.Enum.GetNames(typeof(HexagonBorders)).Length];
    public GameObject[] structureModels = new GameObject[System.Enum.GetNames(typeof(Structures)).Length-1];
    public BiomData[] biomsData = new BiomData[System.Enum.GetNames(typeof(Bioms)).Length];
    public GameObject droneModel;

    [HideInInspector] public NPC drone;
    [HideInInspector] public Chunk[,] chunks;
    [HideInInspector] public float offsetX, offsetZ;
    public int width = 8;
    public int height = 8;

    [HideInInspector] public GameObject[] currentHexagonBorder;


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

        //Initialize the Hexagonborder
        currentHexagonBorder = new GameObject[7];
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
                chunks[x, z].generateBioms();
            }
        }


        //Place the spaceship and focus the camera on it
        //Place a drone over the spaceship
        while (true)
        {
            int x = (int)(Random.value * width * Chunk.chunkSize), 
                z = (int)(Random.value * height * Chunk.chunkSize);

            Chunk c = getChunkAt(x, z);
            print(c.posX + ", " + c.posZ + "    " + getBiom(x, z));
            if (getBiom(x, z) != Bioms.Ocean && getBiom(x, z) != Bioms.OceanMountain)
            {
                Vector3 shipPos = Hexagon.getWorldPosition(x, z);

                //Place the ship
                changeStructure(x, z, Structures.Spaceship);
                changeBiom(x, z, Bioms.SpaceShip);

                //Place the first drone over the ship
                GameObject g = Instantiate(droneModel);
                drone = g.AddComponent<NPC>();
                g.transform.position = shipPos + droneModel.transform.position;
                setNPCAtPosition(drone, g.transform.position);

                //Focus the camera
                Camera.main.transform.position = shipPos + new Vector3(0, Camera.main.transform.position.y, -8f);
                break;
            }
        }

        
    }


    //Make the calculated world visible
    public void showWorld()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                chunks[x, z].generateModel();
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

    public bool isNPCAtPosition(Vector3 position)
    {
        Vector2Int p = Hexagon.getHexPositionInt(position);
        return getChunkAt(p.x, p.z).isNPCAtGlobalCoords(p.x, p.z);
    }

    public NPC getNPCAtPosition(Vector3 position)
    {
        Vector2Int p = Hexagon.getHexPositionInt(position);
        return getChunkAt(p.x, p.z).getNPCAtGlobalCoords(p.x, p.z);
    }

    public NPC getNPCAtPosition(int x, int z)
    {
        return getChunkAt(x, z).getNPCAtGlobalCoords(x, z);
    }

    public void setNPCAtPosition(NPC npc, Vector3 position)
    {
        Vector2Int p = Hexagon.getHexPositionInt(position);
        Chunk c = getChunkAt(p.x, p.z);
        c.setNPCAtGlobalCoords(npc, p.x, p.z);
    }

    public void printFirstChunk()
    {
        for (int x = 0; x < Chunk.chunkSize; x++)
        {
            string s = "";
            for (int z = 0; z < Chunk.chunkSize; z++)
            {
                s += chunks[0, 0].getBiomChunkCoords(x, z) + ", ";
            }
            print(s);
        }
    }

    public void generateHexagonBorder()
    {
        //Generate a new HexagonBorder
        Vector2Int intPos = Hexagon.getHexPositionInt(HexagonFrame.instance.selectedPosition);
        generateHexagonBorder(intPos.x, intPos.z);
    }

    public void generateHexagonBorder(int x, int z)
    {
        destroyHexagonBorder();

        //Generate a new HexagonBorder
        Vector2Int intPos = new Vector2Int(x, z);
        Vector3 pos = Hexagon.getWorldPosition(intPos.x, intPos.z) + new Vector3(0, .01f, 0);
        currentHexagonBorder[0] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderBlue]);
        currentHexagonBorder[0].transform.position = pos;

        if (intPos.z % 2 == 0)
        {
            Vector3 pos1 = Hexagon.getWorldPosition(intPos.x, intPos.z + 1) + new Vector3(0, .01f, 0);
            currentHexagonBorder[1] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[1].transform.position = pos1;

            Vector3 pos2 = Hexagon.getWorldPosition(intPos.x, intPos.z - 1) + new Vector3(0, .01f, 0);
            currentHexagonBorder[2] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[2].transform.position = pos2;

            Vector3 pos3 = Hexagon.getWorldPosition(intPos.x + 1, intPos.z) + new Vector3(0, .01f, 0);
            currentHexagonBorder[3] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[3].transform.position = pos3;

            Vector3 pos4 = Hexagon.getWorldPosition(intPos.x - 1, intPos.z + 1) + new Vector3(0, .01f, 0);
            currentHexagonBorder[4] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[4].transform.position = pos4;

            Vector3 pos5 = Hexagon.getWorldPosition(intPos.x - 1, intPos.z - 1) + new Vector3(0, .01f, 0);
            currentHexagonBorder[5] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[5].transform.position = pos5;

            Vector3 pos6 = Hexagon.getWorldPosition(intPos.x - 1, intPos.z) + new Vector3(0, .01f, 0);
            currentHexagonBorder[6] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[6].transform.position = pos6;
        }
        else
        {
            {
                Vector3 pos1 = Hexagon.getWorldPosition(intPos.x, intPos.z + 1) + new Vector3(0, .01f, 0);
                currentHexagonBorder[1] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[1].transform.position = pos1;

                Vector3 pos2 = Hexagon.getWorldPosition(intPos.x, intPos.z - 1) + new Vector3(0, .01f, 0);
                currentHexagonBorder[2] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[2].transform.position = pos2;

                Vector3 pos3 = Hexagon.getWorldPosition(intPos.x - 1, intPos.z) + new Vector3(0, .01f, 0);
                currentHexagonBorder[3] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[3].transform.position = pos3;

                Vector3 pos4 = Hexagon.getWorldPosition(intPos.x + 1, intPos.z + 1) + new Vector3(0, .01f, 0);
                currentHexagonBorder[4] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[4].transform.position = pos4;

                Vector3 pos5 = Hexagon.getWorldPosition(intPos.x + 1, intPos.z - 1) + new Vector3(0, .01f, 0);
                currentHexagonBorder[5] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[5].transform.position = pos5;

                Vector3 pos6 = Hexagon.getWorldPosition(intPos.x + 1, intPos.z) + new Vector3(0, .01f, 0);
                currentHexagonBorder[6] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[6].transform.position = pos6;
            }
        }
    }

    public void destroyHexagonBorder()
    {
        for (int i = 0; i < 7; i++)
        {
            Destroy(currentHexagonBorder[i]);
        }
    }
}
