using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public Texture2D mouseIcon; 

    public KeyCode selectDestination = KeyCode.Mouse1;
    public KeyCode inventoryKey      = KeyCode.E;
    public KeyCode nextRoundKey      = KeyCode.N;
    public KeyCode pausemenuKey      = KeyCode.Escape;

    //CameraKeys
    public KeyCode focusCameraKey = KeyCode.F;
    public KeyCode moveForwardKey = KeyCode.W;
    public KeyCode moveLeftKey    = KeyCode.A;
    public KeyCode moveBackKey    = KeyCode.S;
    public KeyCode moveRightKey   = KeyCode.D;

    public GameObject pausemenu;
    public GameObject buildmenu;
    public GameObject inventory;
    public GameObject npcBpx;
    public GameObject npcCommandBpx;
    public GameObject buildingBox;

    public Text movePower;      //NPCBox
    public Text buildingName;   //BuildingBox

    //False = cut, True = build
    [HideInInspector] public bool cutTreeOrBuild = false;
    [HideInInspector] public NPC selectedNPC = null;
    [HideInInspector] public Building selectedBuilding = null;
    private bool isSelecting = false;
    [HideInInspector]public bool isChoosingProduct = false;

    void Start()
    {
        instance = this;
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        //Select or deselect a npc
        if (Input.GetMouseButtonDown(0) && !isSelecting)
        {
            selectNPC();
            if(!isChoosingProduct)
                selectBuilding();
        }

        //Open the buildmenu
        if (Input.GetKeyDown(inventoryKey))
            if (!inventory.active)
                activateMenu(inventory);
            else
                deactivateMenu(inventory);

        //Open the pausemenu
        if (Input.GetKeyDown(pausemenuKey))
            if (!pausemenu.active)
                activateMenu(pausemenu);
            else
                deactivateMenu(pausemenu);

        //start the next round
        if (Input.GetKeyDown(nextRoundKey))
            World.instance.nextRound();



        /*
         * Keys for controlling the camera
         */
        //Focus the camera on the current npc
        if (Input.GetKeyDown(focusCameraKey) && selectedNPC != null)
            Camera.main.GetComponent<CameraController>().focusOn(selectedNPC.CurPosition);

        if (Input.GetKey(moveForwardKey))
            Camera.main.GetComponent<CameraController>().move(Direction.Forward);
        if (Input.GetKey(moveBackKey))
            Camera.main.GetComponent<CameraController>().move(Direction.Back);
        if (Input.GetKey(moveRightKey))
            Camera.main.GetComponent<CameraController>().move(Direction.Right);
        if (Input.GetKey(moveLeftKey))
            Camera.main.GetComponent<CameraController>().move(Direction.Left);


        /*
         *  Keys for selecting the destination of a npc
         */
        //Start a selecting process
        if (Input.GetKeyDown(selectDestination))
            isSelecting = true;

        //Cancel the selecting process
        if (Input.GetMouseButtonDown(0) && isSelecting)
        {
            isSelecting = false;
            World.instance.destroyWayBorder();
        }

        //Show the way that a npc has to fly
        if (Input.GetKey(selectDestination) && isSelecting)
        {
            if (selectedNPC != null && !selectedNPC.isMoving && HexagonFrame.instance.selectedPosition != RayCastManager.noResult)
                World.instance.showWay(Hexagon.getHexPositionInt(HexagonFrame.instance.selectedPosition));
            else
                World.instance.destroyWayBorder();
        }

        //Set the destination for a npc
        if (Input.GetKeyUp(selectDestination) && isSelecting)
        {
            isSelecting = false;
            if (selectedNPC != null && !selectedNPC.isMoving && HexagonFrame.instance.selectedPosition != RayCastManager.noResult)
            {
                selectedNPC.Destination = Hexagon.getHexPositionInt(HexagonFrame.instance.selectedPosition);
                World.instance.destroyWayBorder();
            }
        }
    }

    public void activateMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void deactivateMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void selectNPC()
    {
        if (World.instance.isNPCAtPosition(HexagonFrame.instance.selectedPosition))
        {
            //Reset the old NPC
            if (selectedNPC != null)
                selectedNPC.IsSelected = false;

            selectedNPC = World.instance.getNPCAtPosition(HexagonFrame.instance.selectedPosition);
            selectedNPC.IsSelected = true;

            activateNPCBox();
        }
        else
        {
            //Deselect the current NPC
            if (selectedNPC != null && selectedNPC.MovePower == 0)
            {
                selectedNPC.isSelected = false;
                selectedNPC = null;
                deactivateNPCBox();
                World.instance.destroyHexagonBorder();
            }
        }
    }

    public void selectNPC(NPC npc)
    {
        if (selectedNPC != null)
            selectedNPC.isSelected = false;

        selectedNPC = npc;
        if(npc != null)
        {
            selectedNPC.isSelected = true;
            World.instance.generateHexagonBorder(Hexagon.getHexPositionInt(selectedNPC.CurPosition), selectedNPC.MovePower);
        }
        else
        {
            World.instance.destroyHexagonBorder();
        }
    }

    public void selectBuilding()
    {
        if (World.instance.isBuildingAtPosition(HexagonFrame.instance.selectedPosition))
        {
            selectedBuilding = World.instance.getBuildingAtPosition(HexagonFrame.instance.selectedPosition);
            recalculateBuildingBox();
        }
        else
        {
            selectedBuilding = null;
            recalculateBuildingBox();
        }
    } 

    public void activateNPCBox()
    {
        recalculateNPCCommandBox();
        recalculateNPCBox();
    }

    public void deactivateNPCBox()
    {
        npcBpx.SetActive(false);
        deactivateNPCCommandBox();
    }

    public void deactivateNPCCommandBox()
    {
        npcCommandBpx.SetActive(false);
        buildmenu.SetActive(false);
    }

    public void recalculateNPCBox()
    {
        if (selectedNPC != null)
        {
            npcBpx.SetActive(true);
            movePower.text = selectedNPC.MovePower + "/" + selectedNPC.MOVE_POWER;
            recalculateNPCCommandBox();
        }
        else
        {
            deactivateNPCBox();
        }
    }

    public void recalculateBuildingBox()
    {
        if (selectedBuilding != null && selectedBuilding.gameObject.tag.Equals("Factory"))
        {
            buildingBox.SetActive(true);
            string s = selectedBuilding.gameObject.ToString();
            s = s.Replace("(Clone)", "");
            buildingName.text = s;
            isChoosingProduct = true;
        }
        else
        {
            buildingBox.SetActive(false);
            isChoosingProduct = false;
        }
    }

    public void recalculateNPCBoxOnly()
    {
        movePower.text = selectedNPC.MovePower + "/" + selectedNPC.MOVE_POWER;
    }

    public void recalculateNPCCommandBox()
    {
        bool isActive = false;

        if (selectedNPC.MovePower > 0) {
            //CutTree
            Transform t = npcCommandBpx.transform.FindChild("CutTree Button");
            if (World.instance.getBiom(Hexagon.getHexPositionInt(selectedNPC.CurPosition)) == Bioms.Forest)
            {
                t.gameObject.SetActive(true);
                isActive = true;
            }
            else
                t.gameObject.SetActive(false);
        }

        //Activate the buildMenu
        recalculateBuildMenu();


        //Activate the npcCommandBox
        if (isActive && selectedNPC.MovePower > 0 && !selectedNPC.isMoving)
            npcCommandBpx.SetActive(true);
        else
            npcCommandBpx.SetActive(false);
    }

    public void recalculateBuildMenu()
    {
        if (selectedNPC == null || selectedNPC.MovePower == 0)
            return;

        bool b = false;

        if (selectedNPC.MovePower > 0 && !selectedNPC.isMoving && World.instance.getStructure(Hexagon.getHexPositionInt(selectedNPC.CurPosition)) != Structures.Building)
        {
            Vector2Int curPos = Hexagon.getHexPositionInt(selectedNPC.CurPosition);
            /*
             * Activate or deactivate the buildingButtons and deactivate the buildmenu if all buttons are deactivated
             */
            if (canBuildingBeBuild(Buildings.StoreHouse     , "StoreHouse Button"     , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.Woodcutter     , "Woodcutter Button"     , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.House          , "House Button"          , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.Sandmine       , "Sandmine Button"       , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.Gravelmine     , "Gravelmine Button"     , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.LimeStoneMine  , "LimeStoneMine Button"  , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.Fountain       , "Fountain Button"       , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.Claymine       , "Claymine Button"       , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.CementFactory  , "CementFactory Button"  , curPos)) b = true;
            if (canBuildingBeBuild(Buildings.ConcreteFactory, "ConcreteFactory Button", curPos)) b = true;
        }

        buildmenu.SetActive(b);
    }

    //Only for this class important. Makes another method shorter
    private bool canBuildingBeBuild(Buildings building, string button, Vector2Int curPos)
    {
        if (World.instance.isOneOfBioms(curPos, World.instance.buildingModels[(int)building].GetComponent<Building>().bioms))
        {
            Transform t = buildmenu.transform.Find(button);
            t.GetComponent<Button>().enabled = true;
            t.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            return true;
        }
        else
        {
            Transform t = buildmenu.transform.Find(button);
            t.GetComponent<Button>().enabled = false;
            t.GetComponent<Image>().color = new Color(.6f, .6f, .6f, .7f);
            return false;
        }
    }
}
