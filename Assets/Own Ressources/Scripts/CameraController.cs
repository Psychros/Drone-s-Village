using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    //Camera
    public float zoomSpeed = 1f;
    public float minZoom = 10f,
                 maxZoom = 60f;
    public float moveSpeed = 1f,
                 currentMoveSpeed;
    public float startZoom = 10;
    public bool isMoving = false;
    [HideInInspector] public static float factor = 20;      //How far away from the Screenedge must the camera be?(Not in pixels)


    void Start()
    {
        instance = this;

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
        //Reset isMoving
        isMoving = false;

        float moveX = 0;
        float moveZ = 0;

        //Move in x-direction
        if (Input.mousePosition.x <= Screen.width / factor)
        {   //The parentheses must be there
            if (Camera.main.transform.position.x > 0)
            {
                moveX = -1;
                isMoving = true;
            }
        }
        else if (Input.mousePosition.x >= Screen.width - Screen.width / factor)
            if (Camera.main.transform.position.x < World.instance.width * Hexagon.factorX + Hexagon.deltaX)
            {
                moveX = 1;
                isMoving = true;
            }


        //Move in z-direction
        if (Input.mousePosition.y <= Screen.height / factor)
        {   //The parentheses must be there
            if (Camera.main.transform.position.z > -8)
            {
                moveZ = -1;
                isMoving = true;
            }
        }
        else if (Input.mousePosition.y >= Screen.height - Screen.height / factor)
            if (Camera.main.transform.position.z < World.instance.height * Hexagon.factorZ - 8)
            {
                moveZ = 1;
                isMoving = true;
            }


        //Move the camera
        Camera.main.transform.position += new Vector3(moveX * currentMoveSpeed, 0, moveZ * currentMoveSpeed);
    }
}
