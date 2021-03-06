﻿using UnityEngine;
using System.Collections;

public class HexagonFrame : MonoBehaviour {
    public static HexagonFrame instance;

    public Vector3 selectedPosition;
    public GameObject hexagonFrameModel;
    [HideInInspector] public GameObject hexagonFrame;
    [HideInInspector] public Vector3 mousePosition;


    void Start()
    {
        instance = this;

        //Place the hexagonFrame
        if (hexagonFrameModel == null)
            hexagonFrame = gameObject;
        else
            hexagonFrame = Instantiate(hexagonFrameModel);

        hexagonFrame.transform.position = RayCastManager.noResult;
    }


    void Update()
    {
        //Set the position of the hexagonFrame to the selected Hexagon
        if ((CameraController.instance.isMoving) || (mousePosition.x != Input.mousePosition.x) || (mousePosition.y != Input.mousePosition.y))
        {
            //If the player selects no hexagon the position is RayCastManager.noResult
            hexagonFrame.transform.position = RayCastManager.getWorldCoordsRaycast("Hexagon");
            selectedPosition = hexagonFrame.transform.position;
        }

        mousePosition = Input.mousePosition;
    }
}
