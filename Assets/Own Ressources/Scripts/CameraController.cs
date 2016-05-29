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
        if (Input.GetMouseButton(0))
            moveCamera();
    }



    private void zoom(bool zoomIn)
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

    private void moveCamera()
    {
        float moveX = 0;
        float moveZ = 0;
        if (Input.mousePosition.x < 300)
            moveX = -1;
        else if (Input.mousePosition.x > 1200)
            moveX = 1;

        if (Input.mousePosition.y < 100)
            moveZ = -1;
        else if (Input.mousePosition.y > 600)
            moveZ = 1;
        Camera.main.transform.position += new Vector3(moveX * currentMoveSpeed, 0, moveZ * currentMoveSpeed);
    }
}
