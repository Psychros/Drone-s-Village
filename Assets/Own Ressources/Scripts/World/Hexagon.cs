using UnityEngine;
using System.Collections;

public class Hexagon : MonoBehaviour {
    public static float factorX = 1.73206f, 
                        factorZ = 1.5f, 
                        deltaX = 0.86603f;  //Needed for every second row
    public static float maxDelta = 0.001f;  //The maximal delta for correct rounding to int

    /*
     * Converts the HexCoords to WorldCoords
     */
    public static Vector3 getWorldPosition(int x, int z)
    {
        if (z % 2 == 0)
            return new Vector3(x * factorX, 0, z * factorZ);
        else
            return new Vector3(x * factorX + deltaX, 0, z * factorZ);
    }

    public static Vector3 getWorldPosition(Vector2Int v)
    {
        if (v.z % 2 == 0)
            return new Vector3(v.x * factorX, 0, v.z * factorZ);
        else
            return new Vector3(v.x * factorX + deltaX, 0, v.z * factorZ);
    }

    /*
     * Converts the WorldCoords to HexCoords
     */
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


    /*
     * Converts the WorldCoords to HexCoords and get the value as an integer
     */
    public static Vector2Int getHexPositionInt(Vector3 worldPos)
    {
        Vector3 v;

        if ((int)((worldPos.z / factorZ) % 2) == 0)
        {
            v = new Vector3(worldPos.x / factorX, 0, worldPos.z / factorZ);
        }
        else
        {
            v = new Vector3((worldPos.x / factorX) - deltaX + .4f, 0, worldPos.z / factorZ);
        }

        //Convert the Vector to integers
        Vector2Int ints = new Vector2Int();
        ints.x = round(v.x);
        ints.z = round(v.z);

        return ints;
    }

    public static int round(float f)
    {
        if (f % (int)f >= .5f - maxDelta)
            return (int)f + 1;
        else
            return (int)f;
    }

    public static Vector2Int getHexagonTopLeft(Vector2Int pos)
    {
        Vector2Int v;
        if (pos.z % 2 == 0)
            v = new Vector2Int(pos.x - 1, pos.z + 1);
        else
            v = new Vector2Int(pos.x, pos.z + 1);
        return v;
    }

    public static Vector2Int getHexagonTopRight(Vector2Int pos)
    {
        Vector2Int v;
        if (pos.z % 2 == 0)
            v = new Vector2Int(pos.x, pos.z + 1);
        else
            v = new Vector2Int(pos.x + 1, pos.z + 1);
        return v;
    }

    public static Vector2Int getHexagonLeft(Vector2Int pos)
    {
        Vector2Int v = new Vector2Int(pos.x - 1, pos.z);
        return v;
    }

    public static Vector2Int getHexagonRight(Vector2Int pos)
    {
        Vector2Int v = new Vector2Int(pos.x + 1, pos.z);
        return v;
    }
    public static Vector2Int getHexagonDownLeft(Vector2Int pos)
    {
        Vector2Int v;
        if (pos.z % 2 == 0)
            v = new Vector2Int(pos.x - 1, pos.z - 1);
        else
            v = new Vector2Int(pos.x, pos.z - 1);
        return v;
    }

    public static Vector2Int getHexagonDownRight(Vector2Int pos)
    {
        Vector2Int v;
        if (pos.z % 2 == 0)
            v = new Vector2Int(pos.x, pos.z - 1);
        else
            v = new Vector2Int(pos.x + 1, pos.z - 1);
        return v;
    }

}
