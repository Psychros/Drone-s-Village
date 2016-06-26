using UnityEngine;
using System.Collections;

public class CutTree : MonoBehaviour {
    public float speed = 1;

	// Update is called once per frame
	void Update () {
	    if(transform.position.y > -2)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, -1, 0), Time.deltaTime * speed);
        }
        else
        {
            World.instance.changeBiom(Bioms.Plain, transform.position);
            World.instance.changeStructure(transform.position, Structures.None);
            Destroy(gameObject);
        }
	}
}
