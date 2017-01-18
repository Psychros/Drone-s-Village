using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    //Camera
    public float zoomSpeed = 1f;
    public float minZoom = 10f,
                 maxZoom = 60f;
    public float moveSpeed = 1f,
                 currentMoveSpeed,
                 currentMouseMoveSpeed; //The moveSpeed for the movement with the mouse
    public float startZoom = 10;
    public bool isMoving = false;
    [HideInInspector] public static float factor = 20;      //How far away from the Screenedge must the camera be?(Not in pixels)
    private Vector3 oldMousePosition;


    void Start()
    {
        instance = this;
        oldMousePosition = Input.mousePosition;

        Camera.main.fieldOfView = startZoom;
        calculateCurrentMoveSpeed();
    }

    void Update () {
        //Zoom the camera
        if (Input.GetAxis("Mouse ScrollWheel") > 0)      //Zoom in
            zoom(true);     
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)  //Zoom out
            zoom(false);


        //Move the camera
        moveCameraAtTheScreenEdge();
        moveCameraWithLeftMouse();

        //Actualize the oldMousePosition
        if (oldMousePosition != Input.mousePosition)
            oldMousePosition = Input.mousePosition;
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
        currentMouseMoveSpeed = currentMoveSpeed / 3f;
    }

    //Uses the mousePosition
    private void moveCameraAtTheScreenEdge()
    {
        //Reset isMoving
        isMoving = false;

        float moveX = 0;
        float moveZ = 0;

        //Move in x-direction
        if (Input.mousePosition.x <= Screen.width / factor)
        {   
            moveX = -1;
            isMoving = true;
        }
        else if (Input.mousePosition.x >= Screen.width - Screen.width / factor)       
        {
            moveX = 1;
            isMoving = true;
        }


        //Move in z-direction
        if (Input.mousePosition.y <= Screen.height / factor)
        {   
            moveZ = -1;
            isMoving = true;
        }
        else if (Input.mousePosition.y >= Screen.height - Screen.height / factor)
        { 
            moveZ = 1;
            isMoving = true;
         }


        moveCamera(moveX * currentMoveSpeed, moveZ * currentMoveSpeed);
    }


    public void moveCameraWithLeftMouse()
    {
        if ((!isMoving) && (Input.GetKey(KeyCode.Mouse0)) && (oldMousePosition != Input.mousePosition)) //!isMoving removes a doubled movement
        {
            float moveX = oldMousePosition.x - Input.mousePosition.x;
            float moveZ = oldMousePosition.y - Input.mousePosition.y;

            moveCamera(moveX * currentMouseMoveSpeed, moveZ * currentMouseMoveSpeed);
        }
    }



    public void moveCamera(float moveX, float moveZ)
    {
        float moveX2 = 0, moveZ2 = 0;

        if (moveX < 0 && Camera.main.transform.position.x > 0)
            moveX2 = moveX;
        else if ((moveX > 0 && Camera.main.transform.position.x < World.instance.width * Chunk.chunkSize * Hexagon.factorX + Hexagon.deltaX))
            moveX2 = moveX;

        if (moveZ < 0 && Camera.main.transform.position.z > -8)
            moveZ2 = moveZ;
        else if (moveZ > 0 && Camera.main.transform.position.z < World.instance.height * Chunk.chunkSize * Hexagon.factorZ - 8)
            moveZ2 = moveZ;

        //Move the camera
        Camera.main.transform.position += new Vector3(moveX2, 0, moveZ2);
    }

    //Moves the camera in the Direction d
    public void move(Direction d)
    {
        float moveX = 0;
        float moveZ = 0;

        if (d == Direction.Forward)
            moveZ = 1;
        else if (d == Direction.Back)
            moveZ = -1;
        else if (d == Direction.Right)
            moveX = 1;
        else if(d == Direction.Left)
            moveX = -1;

        moveCamera(moveX * currentMoveSpeed, moveZ * currentMoveSpeed);
    }

    public void focusOn(Vector3 v)
    {
        transform.position = v + new Vector3(0, Camera.main.transform.position.y, -8f);
    }
}
