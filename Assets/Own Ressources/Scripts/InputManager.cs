using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public Texture2D mouseIcon; 

    public KeyCode selectDestination = KeyCode.Mouse1;
    public KeyCode switchFunction    = KeyCode.E;
    public KeyCode buildmenuKey      = KeyCode.B;
    public KeyCode focusCameraKey    = KeyCode.F;
    public KeyCode nextRoundKey      = KeyCode.N;
    public KeyCode pausemenuKey      = KeyCode.Escape;

    public GameObject pausemenu;
    public GameObject buildmenu;
    public GameObject npcBpx;

    public Text movePower;

    //False = cut, True = build
    [HideInInspector] public bool cutTreeOrBuild = false;
    [HideInInspector] public NPC selectedNPC = null;

    void Start()
    {
        instance = this;
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        //LeftMouseButton
        if (Input.GetMouseButtonDown(0))
            selectNPC();

        //Set the destination for a npc
        if (Input.GetKeyUp(selectDestination))
            if (selectedNPC != null && !selectedNPC.isMoving && HexagonFrame.instance.selectedPosition != RayCastManager.noResult)
            {
                selectedNPC.Destination = Hexagon.getHexPositionInt(HexagonFrame.instance.selectedPosition);
                World.instance.destroyWayBorder();
            }

        //Show the way that a npc has to fly
        if (Input.GetKey(selectDestination))
        {
            if (selectedNPC != null && !selectedNPC.isMoving && HexagonFrame.instance.selectedPosition != RayCastManager.noResult)
                World.instance.showWay(Hexagon.getHexPositionInt(HexagonFrame.instance.selectedPosition));
            else
                World.instance.destroyWayBorder();
        }

        if (Input.GetKeyDown(switchFunction))
            cutTreeOrBuild = !cutTreeOrBuild;

        if (Input.GetKeyDown(buildmenuKey))
            if (!buildmenu.active)
                activateMenu(buildmenu);
            else
                deactivateMenu(buildmenu);

        if (Input.GetKeyDown(pausemenuKey))
            if (!pausemenu.active)
                activateMenu(pausemenu);
            else
                deactivateMenu(pausemenu);

        if (Input.GetKeyDown(nextRoundKey))
            World.instance.nextRound();

        if (Input.GetKeyDown(focusCameraKey))
            Camera.main.GetComponent<CameraController>().focusOn(HexagonFrame.instance.selectedPosition);

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
