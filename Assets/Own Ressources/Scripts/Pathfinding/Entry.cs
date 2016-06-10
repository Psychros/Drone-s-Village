using UnityEngine;
using System.Collections;

public class Entry{
    public Node node;
    int x, z;

    public Entry(Node n)
    {
        node = n;
        Vector3 hexPos = Hexagon.getHexPosition(n.position);
        x = (int)hexPos.x;
        z = (int)hexPos.z;
    }

    public Entry(Node n, int x, int z)
    {
        node = n;
        this.x = x;
        this.z = z;
    }
}
