using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class World : MonoBehaviour
{
    [HideInInspector]
    public static World instance;

    //Models
    public GameObject[] biomModels      = new GameObject[System.Enum.GetNames(typeof(BiomModels)).Length];
    public GameObject[] hexagonBorderModels    = new GameObject[System.Enum.GetNames(typeof(HexagonBorders)).Length];
    public GameObject[] structureModels = new GameObject[System.Enum.GetNames(typeof(Structures)).Length-1];
    public BiomData[] biomsData = new BiomData[System.Enum.GetNames(typeof(Bioms)).Length];
    public GameObject droneModel;

    //Chunks
    [HideInInspector] public Chunk[,] chunks;
    [HideInInspector] public float offsetX, offsetZ;
    public int width = 8;
    public int height = 8;

    //Other stuff
    [HideInInspector] public GameObject[] currentHexagonBorder;
    [HideInInspector] public List<NPC> npcs = new List<NPC>();
    [HideInInspector] public Inventory inventory;

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
        currentHexagonBorder = new GameObject[150];

        //Get the inventory
        inventory = GetComponent<Inventory>();
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
            if (getBiom(x, z) == Bioms.Plain || getBiom(x, z) == Bioms.PlainDandelion || getBiom(x, z) == Bioms.Forest)
            {
                Vector3 shipPos = Hexagon.getWorldPosition(x, z);

                //Place the ship
                changeStructure(x, z, Structures.Spaceship);
                changeBiom(x, z, Bioms.SpaceShip);

                //Place the first drone over the ship
                GameObject g = Instantiate(droneModel);
                NPC drone = g.AddComponent<NPC>();
                g.transform.position = shipPos + droneModel.transform.position;
                setNPCAtPosition(drone, g.transform.position);

                //Place the second drone over the ship
                GameObject g2 = Instantiate(droneModel);
                NPC drone2 = g2.AddComponent<NPC>();
                Vector2Int v = Hexagon.getHexPositionInt(shipPos);
                g2.transform.position = Hexagon.getWorldPosition(v.x+1, v.z) + droneModel.transform.position;
                setNPCAtPosition(drone2, g2.transform.position);

                //Place the third drone over the ship
                GameObject g3 = Instantiate(droneModel);
                NPC drone3 = g3.AddComponent<NPC>();
                g3.transform.position = Hexagon.getWorldPosition(v.x - 1, v.z) + droneModel.transform.position;
                setNPCAtPosition(drone3, g3.transform.position);

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

    public NPC getNPCAtPosition(Vector2Int v)
    {
        return getChunkAt(v.x, v.z).getNPCAtGlobalCoords(v.x, v.z);
    }

    public void setNPCAtPosition(NPC npc, Vector3 position)
    {
        Vector2Int p = Hexagon.getHexPositionInt(position);
        Chunk c = getChunkAt(p.x, p.z);
        c.setNPCAtGlobalCoords(npc, p.x, p.z);
    }

    public void setNPCAtPosition(NPC npc, Vector2Int p)
    {
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

    public void generateHexagonBorder(Vector2Int intPos, int size)
    {
        destroyHexagonBorder();

        //Generate the border at the startPosition
        Vector3 pos = Hexagon.getWorldPosition(intPos.x, intPos.z) + new Vector3(0, .01f, 0);
        if(size > 0)
            currentHexagonBorder[0] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderBlue]);
        else
            currentHexagonBorder[0] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderRed]);
        currentHexagonBorder[0].transform.position = pos;

        List<Vector2Int> neighbours   = new List<Vector2Int>();
        List<Vector2Int> currentLayer = new List<Vector2Int>();
        List<Vector2Int> nextLayer    = new List<Vector2Int>();

        //Set the startPosition
        currentLayer.Add(intPos);

        //Get the neighbours of the hexagon
        for (int i = 0; i < size; i++)
        {
            //Get the neighbours for every element in currentLayer
            foreach (Vector2Int v in currentLayer)
            {
                //Put the neighbours to the lists
                List<Vector2Int> list = Hexagon.getNeighbours(v);
                foreach (Vector2Int v2 in list)
                {
                    if(!v2.Equals(intPos) && !neighbours.Contains(v2))
                    {
                        neighbours.Add(v2);
                        nextLayer.Add(v2);
                    }
                }
            }

            currentLayer = nextLayer;
            nextLayer = new List<Vector2Int>();
        }

        //Generate the yellow border
        Vector2Int[] allNeighbours = neighbours.ToArray();
        for (int i = 0; i < allNeighbours.Length; i++)
        {
            Vector3 posBorder = Hexagon.getWorldPosition(allNeighbours[i]) + new Vector3(0, .01f, 0);
            currentHexagonBorder[i + 1] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[i + 1].transform.position = posBorder;
        }
    }

    public void destroyHexagonBorder()
    {
        for (int i = 0; i < currentHexagonBorder.Length; i++)
        {
            Destroy(currentHexagonBorder[i]);
        }
    }


    public void nextRound()
    {
        //Reset the movepower of the NPCs
        foreach(NPC npc in npcs)
        {
            npc.resetMovePower();
        }

        //Rebuild the hexagonBorder 
        if (!InputManager.instance.selectedNPC.isMoving)
            generateHexagonBorder(Hexagon.getHexPositionInt(InputManager.instance.selectedNPC.NextDestination), InputManager.instance.selectedNPC.movePower);
        else
            destroyHexagonBorder();
    }
}
