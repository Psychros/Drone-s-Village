using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

    [HideInInspector]public bool isMoving;
    public float speed = 2f, turnSpeed = 200f;
    private Vector3 nextDestination;
    private Vector2Int finalDestination;
    public Vector2Int Destination
    {
        get {return finalDestination;}
        set {
            World.instance.setNPCAtPosition(null, nextDestination); //Resets the hexagonNPC at the old position
            finalDestination = value;
            isMoving = true;

            //Removes the hexagonBorder when the NPC gets a new destination
            World.instance.destroyHexagonBorder();

            selectNextDestination();
        }
    }
    public bool isSelected = false;
    public bool IsSelected
    {
        get { return isSelected; }
        set {
            isSelected = value;
            if (isSelected)
            {
                Vector2Int posOfHexagonBorder = Hexagon.getHexPositionInt(nextDestination);
                World.instance.generateHexagonBorder(posOfHexagonBorder);
            }
            else
                World.instance.destroyHexagonBorder();
        }
    }


    void Start()
    {
        transform.forward = Vector3.Cross(transform.forward, transform.up);
        Destination = Hexagon.getHexPositionInt(transform.position);
        nextDestination = Hexagon.getWorldPosition(finalDestination.x, finalDestination.z);
    }


    void Update () {
        if (isMoving)
        {
            //Rotation
            Vector3 destinationRelative = transform.InverseTransformPoint(nextDestination);
            if (destinationRelative.x > 0)
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            else
                transform.Rotate(0, turnSpeed * Time.deltaTime * -1, 0);


            //Position
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);
            //The NPC is at the goal
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), nextDestination) < 0.2f)
            {
                if (Vector3.Distance(nextDestination, Hexagon.getWorldPosition(finalDestination)) < .2f)
                {
                    //Select the command
                    if (InputManager.instance.cutTreeOrBuild)
                        buildBuilding();
                    else
                        cutTree();

                    isMoving = false;
                }
                //save that there is a NPC at this positioon
                World.instance.setNPCAtPosition(this, nextDestination);

                //Set a new HexagonBorder around the NPC if it is selected
                if (isSelected) {
                    Vector2Int v = Hexagon.getHexPositionInt(nextDestination);
                    World.instance.generateHexagonBorder(v);
                }

                selectNextDestination();
            }
        }
	}

    //Select the nextHexagon on the way to the final destination
    public void selectNextDestination()
    {
        Vector2Int pos = Hexagon.getHexPositionInt(nextDestination);

        int x = 0, z = 0;
        if (finalDestination.x - pos.x > 0)
            x = 1;
        else if (finalDestination.x - pos.x < 0)
            x = -1;

        if (x == 0)
        {
            if (finalDestination.z - pos.z > 0)
                z = 1;
            else if (finalDestination.z - pos.z < 0)
                z = -1;
        }

        nextDestination = Hexagon.getWorldPosition(pos.x + x, pos.z + z);
    }


    public void cutTree()
    {
        Vector2Int pos = finalDestination;

        if (World.instance.getBiom(pos.x, pos.z) == Bioms.Forest)
        {
            World.instance.getStructureGameObject(pos.x, pos.z).AddComponent<CutTree>();
        }
    }

    public void buildBuilding()
    {
        Vector2Int des = finalDestination;
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
