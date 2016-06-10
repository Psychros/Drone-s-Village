using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node{
    public Vector3 position;
    public float distanceToStart = 0;
    public float distance = 0;

    public List<Node> neighbours = new List<Node>();
    public Node previous;


    public Node(int x, int z)
    {
        position = new Vector3(x, 0, z);
    }

    public Node(Vector3  pos)
    {
        position = pos;
    }

    public Node clone()
    {
        return (Node)MemberwiseClone();
    }
}
