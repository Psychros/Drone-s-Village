using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour {

    [HideInInspector]public bool isMoving;
    public float speed = 2f, turnSpeed = 150f;
    public int MOVE_POWER = 5;           //The maximal movePower of the NPC
    public int movePower;                //The current movePower of the NPC
    public static List<Vector2Int> allFinalDestinations = new List<Vector2Int>();//The finalDestinations of all NPCs
    public static List<Vector2Int> allNextDestinations = new List<Vector2Int>();//The nextDestinations of all NPCs

    private Vector3 nextDestination;
    public Vector3 NextDestination
    {
        get { return nextDestination; }
    }

    private Vector3 curPos;
    public Vector3 CurPosition
    {
        get { return curPos; }
    }

    private Vector2Int finalDestination;
    public Vector2Int Destination
    {
        get {return finalDestination;}
        set {
            if (World.instance.getBiom(value) != Bioms.HighMountain && !allFinalDestinations.Contains(value) && value != Hexagon.getHexPositionInt(curPos))
            {
                allFinalDestinations.Add(value);

                //Resets the hexagonNPC at the old position
                World.instance.setNPCAtPosition(null, nextDestination); 
                finalDestination = value;
                isMoving = true;

                //Removes the hexagonBorder when the NPC gets a new destination
                World.instance.destroyHexagonBorder();

                selectNextDestination();
                transform.LookAt(nextDestination + new Vector3(0, transform.position.y, 0));
            }
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
                Vector2Int posOfHexagonBorder = Hexagon.getHexPositionInt(curPos);
                World.instance.generateHexagonBorder(posOfHexagonBorder, movePower);
            }
            else
                World.instance.destroyHexagonBorder();
        }
    }


    void Start()
    {
        transform.forward = Vector3.Cross(transform.forward, transform.up);
        finalDestination = Hexagon.getHexPositionInt(transform.position);
        nextDestination = Hexagon.getWorldPosition(finalDestination.x, finalDestination.z);
        curPos = nextDestination;
        isMoving = false;

        movePower = MOVE_POWER;

        //Add the npc to the worldlist
        World.instance.npcs.Add(this);
    }


    void Update () {
        if (isMoving && movePower > 0)
        {
            //Rotation
            /*Vector3 destinationRelative = transform.InverseTransformPoint(nextDestination);
            if (destinationRelative.x > 0)
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            else
                transform.Rotate(0, turnSpeed * Time.deltaTime * -1, 0);
            */

            //Position
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);
            //The NPC is at the goal
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), nextDestination) < 0.2f)
            {
                reachHexagon();

                //Test if the NPC is at the finalDestination
                if (Vector3.Distance(nextDestination, Hexagon.getWorldPosition(finalDestination)) < .2f)
                {
                    reachDestination();
                }

                allNextDestinations.Remove(Hexagon.getHexPositionInt(nextDestination));
                selectNextDestination();
            }
        }
	}

    //Select the nextHexagon on the way to the final destination
    public void selectNextDestination()
    {
        Vector2Int pos = Hexagon.getHexPositionInt(nextDestination);

        //Calculate the directionVector
        Vector2Int v = Hexagon.nextHexagon(pos, finalDestination);


        if (v != null)
        {
            //If there is a NPC at the destination the npc has to stand
            if (World.instance.getNPCAtPosition(v) != null || allNextDestinations.Contains(v)) {
                reachDestination();
                transform.LookAt(Hexagon.getWorldPosition(v) + new Vector3(0, transform.position.y, 0));
            }
            else
            {
                nextDestination = Hexagon.getWorldPosition(v);
                transform.LookAt(nextDestination + new Vector3(0, transform.position.y, 0));

                allNextDestinations.Add(Hexagon.getHexPositionInt(nextDestination));
            }
        }
    }

    public void reachDestination()
    {
        //Select the command
        if (InputManager.instance.cutTreeOrBuild)
            buildBuilding();
        else
            cutTree();

        //Set a new HexagonBorder around the NPC if it is selected
        if (isSelected)
        {
            Vector2Int v = Hexagon.getHexPositionInt(curPos);
            World.instance.generateHexagonBorder(v, movePower);
        }

        //save that there is a NPC at this position
        World.instance.setNPCAtPosition(this, curPos);

        isMoving = false;

        //Remove the destination from the destinationList
        allFinalDestinations.Remove(finalDestination);
    }


    public void reachHexagon()
    {
        curPos = nextDestination;

        //Reduce the movePower
        if (movePower > 0)
            movePower--;
        else
        {
            //Save the NPCPosition
            World.instance.setNPCAtPosition(this, curPos);
            World.instance.generateHexagonBorder(Hexagon.getHexPositionInt(curPos), 0);
        }

        if(isSelected)
            InputManager.instance.recalculateNPCBox();
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

            //The building can only be build if there are enough ressources
            if (World.instance.inventory.hasRessources(g.GetComponent<Building>().costs))
            {
                World.instance.inventory.removeRessources(g.GetComponent<Building>().costs);
                g.transform.position = new Vector3(g.transform.position.x, -1.014f, g.transform.position.z);
                g.AddComponent<BuildBuilding>();
            }
            else
            {
                //Reset the structure
                World.instance.changeStructure(des.x, des.z, Structures.None);
            }
        }
    }

    public void resetMovePower()
    {
        if(movePower < 0)
            movePower += MOVE_POWER;
        else
            movePower = MOVE_POWER;
    }
}
