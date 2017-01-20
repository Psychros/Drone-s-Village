using UnityEngine;
using UnityEngine.UI;
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

    //DayLightCycle
    [HideInInspector] public int time = 8;        //Time in hours
    [HideInInspector] public static int DAY_LENGTH = 24;
    public bool enableDayNightCycle = false;
    public Text timeText;
    [HideInInspector] public int nightTime = 6;

    //Other stuff
    [HideInInspector] public GameObject[] currentHexagonBorder;
    [HideInInspector] public GameObject[] wayHexagonBorder;
    [HideInInspector] public List<NPC> npcs = new List<NPC>();
    [HideInInspector] public List<Building> buildings = new List<Building>();
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
        wayHexagonBorder     = new GameObject[100];

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

                //Focus the camera
                Camera.main.GetComponent<CameraController>().focusOn(shipPos);
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

    public Bioms getBiom(Vector2Int v)
    {
        return getChunkAt(v.x, v.z).getBiomWorldCoords(v.x, v.z);
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
    public Structures getStructure(Vector2Int v)
    {
        Chunk c = getChunkAt(v.x, v.z);
        return c.getStructureGlobalCoords(v.x, v.z);
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

        //Generate the remaining border
        Vector2Int[] allNeighbours = neighbours.ToArray();
        for (int i = 0; i < allNeighbours.Length; i++)
        {
            //The position must be in the world
            if (allNeighbours[i].x >= 0 && allNeighbours[i].x < width * Chunk.chunkSize && allNeighbours[i].z >= 0 && allNeighbours[i].z < height * Chunk.chunkSize)
            {
                Vector3 posBorder = Hexagon.getWorldPosition(allNeighbours[i]) + new Vector3(0, .01f, 0);

                //Red border on High Mountains
                if (getBiom(allNeighbours[i]) == Bioms.HighMountain)
                    currentHexagonBorder[i + 1] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderRed]);
                //Otherwise generate a yellow border
                else
                    currentHexagonBorder[i + 1] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderYellow]);

                currentHexagonBorder[i + 1].transform.position = posBorder;
            }
        }
    }

    public void destroyHexagonBorder()
    {
        for (int i = 0; i < currentHexagonBorder.Length; i++)
        {
            Destroy(currentHexagonBorder[i]);
        }
    }

    //Shows the way that a npc has to fly
    public void showWay(Vector2Int endPos)
    {
        destroyWayBorder();

        Vector2Int curPos = Hexagon.getHexPositionInt(InputManager.instance.selectedNPC.CurPosition);
        int movePower = InputManager.instance.selectedNPC.MovePower;


        //If there is a HighMountain on the field no border should be generated
        if (getBiom(endPos) == Bioms.HighMountain)
        {
            wayHexagonBorder[0] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderRed]);
            wayHexagonBorder[0].transform.position = Hexagon.getWorldPosition(endPos) + new Vector3(0, .02f, 0);
            return;
        }


        //Calculate and show the way
        for (int i=0; curPos != endPos &&curPos != endPos && i < wayHexagonBorder.Length; i++)
        {
            Vector2Int nextPos = Hexagon.nextHexagon(curPos, endPos);

            if (nextPos != null)
            {

                //Show in the way the range of the npc in lightGray and the rest in darkGray
                if (movePower > 0)
                    movePower--;

                wayHexagonBorder[i] = Instantiate(hexagonBorderModels[(int)HexagonBorders.BorderDarkGray]);
                wayHexagonBorder[i].transform.position = Hexagon.getWorldPosition(nextPos) + new Vector3(0, .02f, 0);

                //The way starts now from the next field
                curPos = nextPos;
            }
        }
    }

    public void destroyWayBorder()
    {
        for (int i = 0; i < wayHexagonBorder.Length; i++)
        {
            Destroy(wayHexagonBorder[i]);
        }
    }

    public void nextRound()
    {
        //Reset the movepower of the NPCs
        foreach(NPC npc in npcs)
        {
            npc.resetMovePower();
        }

        //next round for the buildings
        foreach (Building b in buildings)
        {
            b.nextRound();
        }

        //Rebuild the hexagonBorder
        if (InputManager.instance.selectedNPC != null && !InputManager.instance.selectedNPC.isMoving)
        {
            generateHexagonBorder(Hexagon.getHexPositionInt(InputManager.instance.selectedNPC.NextDestination), InputManager.instance.selectedNPC.MovePower);
        }
        else
            destroyHexagonBorder();

        //Rebuild the npcBox
        if(InputManager.instance.selectedNPC != null)
            InputManager.instance.recalculateNPCBox();


        //Select a NPC if nothing is selected
        selectFirstNPC();

        //DayLightCycle
        if(enableDayNightCycle)
            nextHour();
    }

    //Select a NPC if nothing is selected
    private void selectFirstNPC()
    {
        if (InputManager.instance.selectedNPC == null)
        {
            if (npcs.First() != null)
            {
                InputManager.instance.selectNPC(npcs.First());
                Camera.main.GetComponent<CameraController>().focusOn(npcs.First().CurPosition);
            }
        }
    }

    //DayLightCycle
    private void nextHour()
    {
        //The next day comes
        if (time == DAY_LENGTH - 1)
            time = 0;
        else
            time++;

        if (time < nightTime)
            transform.Find("Directional Light").GetComponent<Light>().intensity = .2f;
        else if (time == nightTime || time == DAY_LENGTH - 1)
        {
            transform.Find("Directional Light").GetComponent<Light>().intensity = .5f;
            foreach (NPC npc in npcs)
            {
                Light l = npc.GetComponentInChildren<Light>();
                if (l != null)
                    l.enabled = true;
            }
        }
        else if (time == nightTime + 1 || time == DAY_LENGTH - 2)
            transform.Find("Directional Light").GetComponent<Light>().intensity = .8f;
        else
        {
            transform.Find("Directional Light").GetComponent<Light>().intensity = 1f;
            foreach (NPC npc in npcs)
            {
                Light l = npc.GetComponentInChildren<Light>();
                if (l != null)
                    l.enabled = false;
            }
        }

        //Show the time
        timeText.text = time + "h";
    }
}
