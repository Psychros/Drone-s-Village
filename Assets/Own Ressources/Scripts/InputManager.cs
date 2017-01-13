using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public Texture2D mouseIcon; 

    public KeyCode rightClick     = KeyCode.Mouse1;
    public KeyCode leftClick      = KeyCode.Mouse0;
    public KeyCode switchFunction = KeyCode.E;
    public KeyCode buildmenuKey   = KeyCode.F;
    public KeyCode nextRoundKey   = KeyCode.N;
    public KeyCode pausemenuKey   = KeyCode.Escape;

    public GameObject pausemenu;
    public GameObject buildmenu;
    public GameObject npcBpx;

    public Text movePower;

    //False = cut, True = build
    [HideInInspector] public bool cutTreeOrBuild = false;

    [HideInInspector] public GameObject[] currentHexagonBorder;
    [HideInInspector] public NPC selectedNPC = null;

    void Start()
    {
        instance = this;
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        if (Input.GetKeyDown(rightClick))
            if(selectedNPC != null && !selectedNPC.isMoving)
                selectedNPC.Destination = Hexagon.getHexPositionInt(HexagonFrame.instance.selectedPosition);
        if (Input.GetMouseButtonDown(0))
            selectNPC();

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

            activateNPCBox(selectedNPC);
        }
        else
        {
            if (selectedNPC != null)
            {
                selectedNPC.IsSelected = false;
                selectedNPC = null;
            }
        }
    }

    public void activateNPCBox(NPC npc)
    {
        npcBpx.SetActive(true);

        movePower.text = npc.movePower + "/" + npc.MOVE_POWER;
    }
}
