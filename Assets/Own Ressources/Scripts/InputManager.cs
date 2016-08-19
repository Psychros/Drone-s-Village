using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public Texture2D mouseIcon; 

    public KeyCode rightClick     = KeyCode.Mouse1;
    public KeyCode leftClick      = KeyCode.Mouse0;
    public KeyCode switchFunction = KeyCode.E;
    public KeyCode testKey = KeyCode.F;
    public KeyCode pausemenuKey = KeyCode.Escape;

    public GameObject pausemenu;

    //False = cut, True = build
    public bool cutTreeOrBuild = false;

    void Start()
    {
        instance = this;
        Cursor.SetCursor(mouseIcon, Vector2.zero, CursorMode.ForceSoftware);

    }

    void Update()
    {
        if (Input.GetKeyDown(rightClick))
            World.instance.drone.Destination = HexagonFrame.instance.selectedPosition;

        if (Input.GetKeyDown(switchFunction))
            cutTreeOrBuild = !cutTreeOrBuild;

        if (Input.GetKeyDown(testKey))
            World.instance.GetComponent<Inventory>().addRessources(Ressources.Concrete, 1000);

        if (Input.GetKeyDown(pausemenuKey))
            if (!pausemenu.active)
                pausemenu.SetActive(true);
            else
                pausemenu.SetActive(false);
    }

    public void activateMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void deactivateMenu(GameObject menu)
    {
        menu.SetActive(false);
    }
}
