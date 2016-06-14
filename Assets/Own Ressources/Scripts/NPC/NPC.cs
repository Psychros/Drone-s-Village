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
                isMoving = false;
        }
	}
}
