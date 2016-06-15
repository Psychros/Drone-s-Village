using UnityEngine;
using System.Collections;

public class Hexagon : MonoBehaviour {
    public static float factorX = 1.73206f, 
                        factorZ = 1.5f, 
                        deltaX = 0.86603f;

    //Convert the HexCoords to WorldCoords
    public static Vector3 getWorldPosition(int x, int z)
    {
        if (z % 2 == 0)
            return new Vector3(x * factorX, 0, z * factorZ);
        else
            return new Vector3(x * factorX + deltaX, 0, z * factorZ);
    }

    public static Vector3 getHexPosition(Vector3 worldPos)
    {
        Vector3 v;

        if ((int)((worldPos.z / factorZ) % 2) == 0)
        {
            v = new Vector3(worldPos.x / factorX, 0, worldPos.z / factorZ); 
            v.z++;

            return v;
        }
        else
        {
            v = new Vector3((worldPos.x / factorX) - deltaX + .4f, 0, worldPos.z / factorZ);
            v.x++;
            v.z++;

            return v;
        }
    }
}
