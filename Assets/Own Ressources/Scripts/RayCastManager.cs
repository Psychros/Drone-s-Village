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

    public static Vector3 getCoordsRaycast()
    {
        RaycastHit hit = startRayCast();
        print(hit.collider.gameObject.transform.position);
        return Hexagon.getHexPosition(hit.collider.gameObject.transform.position);
    }
}
