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


    //Returns Vector3.down if there is no Hexagon
    public static Vector3 getHexCoordsRaycast()
    {
        RaycastHit hit = startRayCast();
        if (hit.collider != null)
            return Hexagon.getHexPosition(hit.collider.gameObject.transform.position);
        else
            return Vector3.down;
    }


    //Returns Vector3.down if there is no Hexagon
    public static Vector3 getWorldCoordsRaycast()
    {
        RaycastHit hit = startRayCast();
        if (hit.collider != null)
            return hit.collider.gameObject.transform.position;
        else
            return Vector3.down;
    }
}
