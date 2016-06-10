using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {

    public Node[,] nodes;


    //Creates the Map for the Nodes
    public void generateNodes(int width, int height)
    {
        nodes = new Node[width, height];

        //Generate all Nodes
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                nodes[x, z] = new Node(Hexagon.getWorldPosition(x, z));
            }
        }


        //Set the 6 neighbours
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (z > 0)
                    nodes[x, z].neighbours.Add(nodes[x, z - 1]);
                if (x < World.instance.width - 1 && z > 0)
                    nodes[x, z].neighbours.Add(nodes[x + 1, z - 1]);
                if (x < World.instance.width - 1)
                    nodes[x, z].neighbours.Add(nodes[x + 1, z]);
                if (x < World.instance.width - 1 && z < World.instance.height - 1)
                    nodes[x, z].neighbours.Add(nodes[x + 1, z + 1]);
                if (z < World.instance.height - 1)
                    nodes[x, z].neighbours.Add(nodes[x, z + 1]);
                if (x > 0)
                    nodes[x, z].neighbours.Add(nodes[x - 1, z]);
            }
        }
    }



    //Calculate the path
    public void findPath(Node start, Node end)
    {
        List<Entry> waitList = new List<Entry>();
        List<Entry> finishedList = new List<Entry>();
        Entry currentNode = new Entry(start);

        //Find a path
        while (waitList.Count > 0)
        {
            for (int i = 0; i < waitList[0].node.neighbours.Count; i++)
            {
                //if(!waitList.Contains())
            }
        }
    }


    private void contains(List<Entry> list, int x, int z) {

    }
}
