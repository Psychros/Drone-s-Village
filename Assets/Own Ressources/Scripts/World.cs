using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class World : MonoBehaviour {
    public GameObject grass, sand;
    public int width = 5, 
               height = 5;

    // Use this for initialization
    void Start () {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        //Generate the world
        for (float i = 0; i < width; i++)
        {
            for (float j = 0; j < height; j++)
            {
                Vector3 v;
                if (j % 2 == 0)
                    v = new Vector3(i * 1.73206f, 0, j * 1.5f);
                else
                    v = new Vector3(i * 1.73206f + 0.86603f, 0, j * 1.5f);

                GameObject g = Instantiate(grass);
                g.transform.position = v;
            }
        }
        watch.Stop();
        print(watch.ElapsedMilliseconds);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
