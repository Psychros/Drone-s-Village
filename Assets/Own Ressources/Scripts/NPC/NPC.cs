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
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), destination) < 0.2f)
            {
                isMoving = false;

                if (InputManager.instance.cutTreeOrBuild)
                    buildBuilding();
                else
                    cutTree();
            }
        }
	}


    public void cutTree()
    {
        Vector2Int pos = Hexagon.getHexPositionInt(destination);
        if (World.instance.worldBiomes[pos.x, pos.z] == (int)Bioms.Forest)
        {
            GameObject g = World.instance.world[pos.x, pos.z].transform.GetChild(0).gameObject;
            g.AddComponent<CutTree>();

            //Activate the ParticleSystem
            World.instance.world[pos.x, pos.z].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void buildBuilding()
    {
        GameObject g = World.instance.changeBiom(Bioms.Plain, Bioms.StoreHouse, destination).transform.GetChild(0).gameObject;
        g.AddComponent<BuildBuilding>();
    }
}
