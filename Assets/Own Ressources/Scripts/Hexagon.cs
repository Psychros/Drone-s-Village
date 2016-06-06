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
        if (worldPos.z % 2 == 0)
            return new Vector3(worldPos.x / factorX, 0, worldPos.z / factorZ);
        else
            return new Vector3(worldPos.x / factorX - deltaX, 0, worldPos.z / factorZ);
    }
}
