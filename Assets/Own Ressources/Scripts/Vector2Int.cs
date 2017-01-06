using UnityEngine;
using System.Collections;

public class Vector2Int{
    public int x, z;

    public Vector2Int()
    {

    }

    public Vector2Int(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override System.String ToString()
    {
        return "(" + x + ", " + z + ")";
    }

    public override bool Equals(System.Object obj)
    {
        // Check for null values and compare run-time types.
        if (obj == null || GetType() != obj.GetType())
            return false;
        Vector2Int v = (Vector2Int)obj;

        return x == v.x && z == v.z;
    }
}
