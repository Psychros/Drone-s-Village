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


    public static Vector2Int getHexPositionInt(Vector3 worldPos)
    {
        Vector3 v;

        if ((int)((worldPos.z / factorZ) % 2) == 0)
        {
            v = new Vector3(worldPos.x / factorX, 0, worldPos.z / factorZ);
            v.z++;
        }
        else
        {
            v = new Vector3((worldPos.x / factorX) - deltaX + .4f, 0, worldPos.z / factorZ);
            v.x++;
            v.z++;
        }

        //Convert the Vector to integers
        Vector2Int ints = new Vector2Int();
        int x = (int)Mathf.Round(v.x);
        int z = (int)Mathf.Round(v.z);
        //Correct the rounding errors
        if ((v.x % 1 >= .5f) && (x > v.x))
            x--;
        if (v.x - x >= 0.99999f)
            x++;
        if ((v.z % 1 >= .5f) && (z > v.z))
            z--;
        if (z >= v.z)
            z--;

        ints.x = x;
        ints.z = z;

        print("HexX:" + v.x + " HexZ:" + v.z + " x:" + x + " z:" + z + " xDelta:" + (v.x - x));
        return ints;
    }
}
