using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public Texture2D mouseIcon; 

    public KeyCode rightClick     = KeyCode.Mouse1;
    public KeyCode leftClick      = KeyCode.Mouse0;
    public KeyCode switchFunction = KeyCode.E;
    public KeyCode testKey = KeyCode.F;

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
    }
}
