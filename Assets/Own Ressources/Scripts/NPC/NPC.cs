using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

    public bool isMoving;
    public float speed = 2f, turnSpeed = 150f;
    private Vector3 destination;
    public Vector3 Destination
    {
        get {return destination;}
        set {
            destination = value;
            isMoving = true;
        }
    }

    void Start()
    {
        transform.forward = Vector3.Cross(transform.forward, transform.up);
        print(transform.forward);
    }


    void Update () {
        if (isMoving)
        {
            //Rotation
            Vector3 destinationRelative = transform.InverseTransformPoint(destination);
            if (destinationRelative.x > 0)
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            }
            else
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime * -1, 0);
            }


            //Position
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, destination) < 0.4f)
            {
                isMoving = false;
                cutTree();
            }
        }
	}


    //Cuts a tree
    public void cutTree()
    {
        Vector3 posHex = Hexagon.getHexPosition(destination);
        int x = (int)Mathf.Round(posHex.x);
        int z = (int)Mathf.Round(posHex.z);
        if ((posHex.x % 1 >= .5f) && (x > posHex.x))
            x--;
        if ((posHex.z % 1 >= .5f) && (z > posHex.z))
            z--;
        if (z >= posHex.z)
            z--;

        print("HexX:" + posHex.x + " HexZ:" + posHex.z
            + " XRound:" + x + " ZRound:" + z);

        //Remove the old GameObject and create a new one
        Destroy(World.instance.world[x, z]);
        GameObject g = Instantiate(World.instance.models[(int)Bioms.Plain]);
        g.transform.position = destination;
        World.instance.world[x, z] = g;
    }
}
