using UnityEngine;


/*
 *  This class can be used for Raycasts
 */
public class RayCastManager : MonoBehaviour {

    //Start a Raycast from the mouseposition
    public static RaycastHit startRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        return hit;
    }

    public static Vector3 getHexCoordsRaycast()
    {
        RaycastHit hit = startRayCast();
        return Hexagon.getHexPosition(hit.collider.gameObject.transform.position);
    }

    public static Vector3 getWorldCoordsRaycast()
    {
        RaycastHit hit = startRayCast();
        return hit.collider.gameObject.transform.position;
    }
}
