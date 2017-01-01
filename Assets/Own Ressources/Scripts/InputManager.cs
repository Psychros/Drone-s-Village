using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public Texture2D mouseIcon; 

    public KeyCode rightClick     = KeyCode.Mouse1;
    public KeyCode leftClick      = KeyCode.Mouse0;
    public KeyCode switchFunction = KeyCode.E;
    public KeyCode buildmenuKey   = KeyCode.F;
    public KeyCode pausemenuKey   = KeyCode.Escape;

    public GameObject pausemenu;
    public GameObject buildmenu;

    //False = cut, True = build
    public bool cutTreeOrBuild = false;

    public GameObject[] currentHexagonBorder;

    void Start()
    {
        instance = this;
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.ForceSoftware);

        currentHexagonBorder = new GameObject[7];
    }

    void Update()
    {
        if (Input.GetKeyDown(rightClick))
            World.instance.drone.Destination = HexagonFrame.instance.selectedPosition;
        if (Input.GetMouseButtonDown(0))
        {
            generateHexagonBorder();
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
    }

    public void activateMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void deactivateMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void generateHexagonBorder()
    {
        //Generate 7 HexagonBorders
        for (int i = 0; i < 7; i++)
        {
            Destroy(currentHexagonBorder[i]);
        }

        Vector2Int intPos = Hexagon.getHexPositionInt(HexagonFrame.instance.selectedPosition);
        Vector3 pos = HexagonFrame.instance.selectedPosition + new Vector3(0, .01f, 0);
        currentHexagonBorder[0] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderBlue]);
        currentHexagonBorder[0].transform.position = pos;

        if (intPos.z % 2 == 0)
        {
            Vector3 pos1 = Hexagon.getWorldPosition(intPos.x, intPos.z + 1) + new Vector3(0, .01f, 0);
            currentHexagonBorder[1] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[1].transform.position = pos1;

            Vector3 pos2 = Hexagon.getWorldPosition(intPos.x, intPos.z - 1) + new Vector3(0, .01f, 0);
            currentHexagonBorder[2] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[2].transform.position = pos2;

            Vector3 pos3 = Hexagon.getWorldPosition(intPos.x + 1, intPos.z) + new Vector3(0, .01f, 0);
            currentHexagonBorder[3] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[3].transform.position = pos3;

            Vector3 pos4 = Hexagon.getWorldPosition(intPos.x - 1, intPos.z + 1) + new Vector3(0, .01f, 0);
            currentHexagonBorder[4] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[4].transform.position = pos4;

            Vector3 pos5 = Hexagon.getWorldPosition(intPos.x - 1, intPos.z - 1) + new Vector3(0, .01f, 0);
            currentHexagonBorder[5] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[5].transform.position = pos5;

            Vector3 pos6 = Hexagon.getWorldPosition(intPos.x - 1, intPos.z) + new Vector3(0, .01f, 0);
            currentHexagonBorder[6] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
            currentHexagonBorder[6].transform.position = pos6;
        }
        else
        {
            {
                Vector3 pos1 = Hexagon.getWorldPosition(intPos.x, intPos.z + 1) + new Vector3(0, .01f, 0);
                currentHexagonBorder[1] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[1].transform.position = pos1;

                Vector3 pos2 = Hexagon.getWorldPosition(intPos.x, intPos.z - 1) + new Vector3(0, .01f, 0);
                currentHexagonBorder[2] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[2].transform.position = pos2;

                Vector3 pos3 = Hexagon.getWorldPosition(intPos.x - 1, intPos.z) + new Vector3(0, .01f, 0);
                currentHexagonBorder[3] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[3].transform.position = pos3;

                Vector3 pos4 = Hexagon.getWorldPosition(intPos.x + 1, intPos.z + 1) + new Vector3(0, .01f, 0);
                currentHexagonBorder[4] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[4].transform.position = pos4;

                Vector3 pos5 = Hexagon.getWorldPosition(intPos.x + 1, intPos.z - 1) + new Vector3(0, .01f, 0);
                currentHexagonBorder[5] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[5].transform.position = pos5;

                Vector3 pos6 = Hexagon.getWorldPosition(intPos.x + 1, intPos.z) + new Vector3(0, .01f, 0);
                currentHexagonBorder[6] = Instantiate(World.instance.hexagonBorderModels[(int)HexagonBorders.BorderYellow]);
                currentHexagonBorder[6].transform.position = pos6;
            }
        }
    }
}
