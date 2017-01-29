using UnityEngine;
using System.Collections;

public class BuildBuilding : MonoBehaviour {

    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 0), Time.deltaTime * speed);
        }
        else
        {
            Building building = GetComponent<Building>();
            building.buildBuilding();
            Destroy(this);
        }
    }
}
