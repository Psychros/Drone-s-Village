using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public Texture2D mouseIcon; 

    public KeyCode selectDestination = KeyCode.Mouse1;
    public KeyCode switchFunction    = KeyCode.E;
    public KeyCode buildmenuKey      = KeyCode.B;
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
    public GameObject npcBpx;

    public Text movePower;

    //False = cut, True = build
    [HideInInspector] public bool cutTreeOrBuild = false;
    [HideInInspector] public NPC selectedNPC = null;
    private bool isSelecting = false;

    void Start()
    {
        instance = this;
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        //Select or deselect a npc
        if (Input.GetMouseButtonDown(0) && !isSelecting)
            selectNPC();

        //Switch between cut or build
        if (Input.GetKeyDown(switchFunction))
            cutTreeOrBuild = !cutTreeOrBuild;

        //Open the buildmenu
        if (Input.GetKeyDown(buildmenuKey))
            if (!buildmenu.active)
                activateMenu(buildmenu);
            else
                deactivateMenu(buildmenu);

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
        Camera.main.GetComponent<CameraController>().enableMove = false;
    }

    public void deactivateMenu(GameObject menu)
    {
        menu.SetActive(false);
        Camera.main.GetComponent<CameraController>().enableMove = false;
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
            if (selectedNPC != null)
            {
                selectedNPC.IsSelected = false;
                selectedNPC = null;
                deactivateNPCBox();
            }
        }
    }

    public void activateNPCBox()
    {
        npcBpx.SetActive(true);
        recalculateNPCMovePower();
    }

    public void deactivateNPCBox()
    {
        npcBpx.SetActive(false);
    }

    public void recalculateNPCMovePower()
    {
        movePower.text = selectedNPC.movePower + "/" + selectedNPC.MOVE_POWER;
    }

    
}
