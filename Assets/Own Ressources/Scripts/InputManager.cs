using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public KeyCode leftClick = KeyCode.Mouse1;

    void Update()
    {
        if (Input.GetKeyDown(leftClick))
            World.instance.drone.Destination = RayCastManager.getWorldCoordsRaycast();
    }
}
