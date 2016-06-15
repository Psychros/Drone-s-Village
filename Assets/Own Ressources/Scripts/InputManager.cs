using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    public KeyCode leftClick = KeyCode.Mouse1;
    public KeyCode switchFunction = KeyCode.E;

    //False = cut, True = build
    public bool cutTreeOrBuild = false;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(leftClick))
            World.instance.drone.Destination = RayCastManager.getWorldCoordsRaycast();

        if (Input.GetKeyDown(switchFunction))
            cutTreeOrBuild = !cutTreeOrBuild;
    }
}
