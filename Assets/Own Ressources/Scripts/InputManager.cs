using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public KeyCode rightClick     = KeyCode.Mouse1;
    public KeyCode leftClick      = KeyCode.Mouse0;
    public KeyCode switchFunction = KeyCode.E;

    //False = cut, True = build
    public bool cutTreeOrBuild = false;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(rightClick))
            World.instance.drone.Destination = HexagonFrame.instance.selectedPosition;

        if (Input.GetKeyDown(switchFunction))
            cutTreeOrBuild = !cutTreeOrBuild;
    }
}
