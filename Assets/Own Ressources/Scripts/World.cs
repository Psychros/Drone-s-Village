using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class World : MonoBehaviour {
    public GameObject grass, sand, water, stone, mesh;
    public int width = 5, 
               height = 5;

    // Use this for initialization
    void Start () {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        //Generate the world
        generate();
        
        watch.Stop();
        print(watch.ElapsedMilliseconds);
	}
	
	// Update is called once per frame
	void Update () {
	
	}




    public void generate()
    {
        GameObject g;

        //Initialize the Random
        Random.seed = Random.Range(int.MinValue, int.MaxValue);
        float offsetX = Random.value * 10000,
              offsetZ = Random.value * 10000;

        //Create the Cards
        for (float i = 0; i < width; i++)
        {
            for (float j = 0; j < height; j++)
            {
                Vector3 v;
                if (j % 2 == 0)
                    v = new Vector3(i * 1.73206f, 0, j * 1.5f);
                else
                    v = new Vector3(i * 1.73206f + 0.86603f, 0, j * 1.5f);

                //Select the correct terrain
                float biom = Mathf.PerlinNoise(offsetX + i / width * 20, offsetZ + j / height * 20);   

                if (biom > .75f)
                    g = Instantiate(stone);
                else if (biom > .52f)
                    g = Instantiate(grass);
                else if (biom > .47f)
                    g = Instantiate(sand);
                else
                    g = Instantiate(water);
                g.transform.position = v;
                g.transform.parent = transform;
            }
        }
    }
}
