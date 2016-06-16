using UnityEngine;
using System.Collections;

public class HexagonFrame : MonoBehaviour {
    private float timerHexagonFrame = 0;
    private float timeHexagonFrame = .1f;
    public GameObject hexagonFrameModel;
    [HideInInspector] public GameObject hexagonFrame;


    void Start()
    {
        //Place the hexagonFrame
        hexagonFrame = Instantiate(hexagonFrameModel);
        hexagonFrame.transform.position = new Vector3(0, -1, 0);
    }


    void Update()
    {
        //Set the position of the hexagonFrame to the selected Hexagon
        timerHexagonFrame += Time.deltaTime;
        if (timerHexagonFrame >= timeHexagonFrame)
        {
            timerHexagonFrame = 0;
            hexagonFrame.transform.position = RayCastManager.getWorldCoordsRaycast();
        }
    }
}
