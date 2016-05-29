using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    //Camera
    public float zoomSpeed = 1f;
    public float minZoom = 10f,
                 maxZoom = 60f;
    public float moveSpeed = 1f,
                 currentMoveSpeed;


    void Start()
    {
        calculateCurrentMoveSpeed();
    }

    void Update () {
        //Zoom the camera
        if (Input.GetAxis("Mouse ScrollWheel") > 0)      //Zoom in
            zoom(true);     
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)  //Zoom out
            zoom(false);

        //Move the camera
        float moveX = Input.GetAxis("Mouse X");
        float moveZ = Input.GetAxis("Mouse Y");
        Camera.main.transform.position += new Vector3(moveX * currentMoveSpeed, 0, moveZ * currentMoveSpeed);
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

        calculateCurrentMoveSpeed();
    }


    private void calculateCurrentMoveSpeed()
    {
        currentMoveSpeed = moveSpeed / maxZoom * Camera.main.fieldOfView;
    }
}
