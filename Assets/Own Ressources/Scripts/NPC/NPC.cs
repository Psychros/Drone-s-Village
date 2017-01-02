using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

    [HideInInspector]public bool isMoving;
    public float speed = 2f, turnSpeed = 150f;
    private Vector3 destination;
    public Vector3 Destination
    {
        get {return destination;}
        set {
            World.instance.setNPCAtPosition(null, destination); //Resets the hexagonNPC at the old position
            destination = value;
            isMoving = true;
        }
    }

    void Start()
    {
        transform.forward = Vector3.Cross(transform.forward, transform.up);
    }


    void Update () {
        if (isMoving)
        {
            //Rotation
            Vector3 destinationRelative = transform.InverseTransformPoint(destination);
            if (destinationRelative.x > 0)
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            else
                transform.Rotate(0, turnSpeed * Time.deltaTime * -1, 0);


            //Position
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);
            //The NPC is at the goal
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), destination) < 0.2f)
            {
                isMoving = false;

                if (InputManager.instance.cutTreeOrBuild)
                    buildBuilding();
                else
                    cutTree();

                //save that there is a NPC at this positioon
                World.instance.setNPCAtPosition(this, destination);
            }
        }
	}


    public void cutTree()
    {
        Vector2Int pos = Hexagon.getHexPositionInt(destination);

        if (World.instance.getBiom(pos.x, pos.z) == Bioms.Forest)
        {
            World.instance.getStructureGameObject(pos.x, pos.z).AddComponent<CutTree>();
        }
    }

    public void buildBuilding()
    {
        Vector2Int des = Hexagon.getHexPositionInt(destination);
        //print("OwnPosition:" + pos2 + ", " + transform.position + " Destination: " + destination + "  realDestination: " + des);
        if (World.instance.getBiom(des.x, des.z) == Bioms.Plain)
        {
            World.instance.changeStructure(des.x, des.z, Structures.StoreHouse);
            GameObject g = World.instance.getStructureGameObject(des.x, des.z);
            g.transform.position = new Vector3(g.transform.position.x, - 1.014f, g.transform.position.z);
            g.AddComponent<BuildBuilding>();
        }
    }
}
