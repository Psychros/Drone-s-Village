using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Moves a TextMeshComponent to the sky
 */
public class RessourceText : MonoBehaviour {
    public static float maxHeight = 2f;
    public static float speed = .5f;
    public static float heightDisappear = 1.5f;  //Height where the text starts to disappear
    TextMesh tM;
    bool isStarted = false;

	// Use this for initialization
	void Start () {
        tM = GetComponent<TextMesh>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (isStarted) {
            if (transform.position.y < maxHeight)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 0), Time.deltaTime * speed);
                if (transform.position.y > heightDisappear)
                {
                    print(1 - ((transform.position.y - heightDisappear)) / (maxHeight - heightDisappear));
                    tM.color = new Color(tM.color.r, tM.color.g, tM.color.b, 1 - ((transform.position.y - heightDisappear)) / (maxHeight - heightDisappear));
                }
            }
            else
                Destroy(gameObject);
        }
	}

    public void startMovement()
    {
        isStarted = true;
    }
}
