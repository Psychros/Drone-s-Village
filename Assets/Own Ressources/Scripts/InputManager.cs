using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public float zoomSpeed = 1f;
    public float minZoom = 10f,
                 maxZoom = 60f;

    // Update is called once per frame
    void Update () {
        //Zoom the camera
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            zoom(true);     //Zoom in
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
            zoom(false);    //Zoom out
    }

    public void zoom(bool zoomIn)
    {
        if (zoomIn)
        {
            //Zoom in
            Camera.main.fieldOfView -= zoomSpeed;
            if (Camera.main.fieldOfView < minZoom)
                Camera.main.fieldOfView = minZoom;
        }
        else
        {
            //Zoom out
            Camera.main.fieldOfView += zoomSpeed;
            if (Camera.main.fieldOfView > maxZoom)
                Camera.main.fieldOfView = maxZoom;
        }
    }
}
