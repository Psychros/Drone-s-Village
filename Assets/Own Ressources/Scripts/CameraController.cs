using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    //Camera
    public float zoomSpeed = 1f;
    public float minZoom = 10f,
                 maxZoom = 60f;
    public float moveSpeed = 1f,
                 currentMoveSpeed;
    public float startZoom = 10;
    public bool isMoving = false;


    void Start()
    {
        calculateCurrentMoveSpeed();
        Camera.main.fieldOfView = startZoom;
    }

    void Update () {
        //Zoom the camera
        if (Input.GetAxis("Mouse ScrollWheel") > 0)      //Zoom in
            zoom(true);     
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)  //Zoom out
            zoom(false);


        //Move the camera
        if (Input.GetMouseButton(0))
        {
            moveCamera();
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

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

        //Move in x-direction
        if (Input.mousePosition.x < Screen.width/5)
        {   //The parentheses must be there
            if (Camera.main.transform.position.x > 0)
                moveX = -1;
        }
        else if (Input.mousePosition.x > Screen.width - Screen.width/5)
            if (Camera.main.transform.position.x < World.instance.width * Hexagon.factorX + Hexagon.deltaX)
                moveX = 1;


        //Move in z-direction
        if (Input.mousePosition.y < Screen.height/5)
        {   //The parentheses must be there
            if (Camera.main.transform.position.z > -8)
                moveZ = -1;
        }
        else if (Input.mousePosition.y > Screen.height - Screen.height/5)
            if (Camera.main.transform.position.z < World.instance.height * Hexagon.factorZ - 8)
                moveZ = 1;


        //Move the camera
        Camera.main.transform.position += new Vector3(moveX * currentMoveSpeed, 0, moveZ * currentMoveSpeed);
    }
}
