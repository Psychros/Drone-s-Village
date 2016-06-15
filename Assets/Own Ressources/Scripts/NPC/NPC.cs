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


    //The drone changes the biom of a hexagon
    public void changeBiom(Bioms oldBiom, Bioms newBiom)
    {
        //Get the position of the hexagon
        Vector2Int posHex = Hexagon.getHexPositionInt(destination);

        //Remove the old GameObject and create a new one
        if (World.instance.worldBiomes[posHex.x, posHex.z] == (int)oldBiom)
        {
            Destroy(World.instance.world[posHex.x, posHex.z]);
            GameObject g = Instantiate(World.instance.models[(int)newBiom]);
            g.transform.position = destination;
            g.transform.parent = World.instance.transform;
            World.instance.world[posHex.x, posHex.z] = g;
        }
    }


    public void cutTree()
    {
        changeBiom(Bioms.Forest, Bioms.Plain);
    }
}
