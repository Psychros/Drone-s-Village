using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class World : MonoBehaviour
{
    [HideInInspector]
    public static World instance;

    public GameObject[] models = new GameObject[System.Enum.GetNames(typeof(Bioms)).Length];
    public GameObject drone;
    public int width = 50,
               height = 50;
    private int[,] world;

    // Use this for initialization
    void Start()
    {
        instance = this;
        world = new int[width, height];

        Stopwatch watch = new Stopwatch();
        watch.Start();

        //Generate the world
        generate();
        showWorld();

        watch.Stop();
        //print(watch.ElapsedMilliseconds);
    }

    // Update is called once per frame
    void Update()
    {

    }




    public void generate()
    {
        //Initialize the Random
        Random.seed = Random.Range(int.MinValue, int.MaxValue);
        float offsetX = Random.value * 10000,
              offsetZ = Random.value * 10000;

        //Create the Cards
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //Select the correct terrain
                float biom = Mathf.PerlinNoise(offsetX + (float)i / width * 5f, offsetZ + (float)j / height * 5f);

                if (biom > .85f)
                    world[i, j] = (int)Bioms.HighMountain;
                else if(biom > .8f)
                    world[i, j] = (int)Bioms.Mountain;
                else if (biom > .75f)
                {
                    if (Random.value > .3)
                        world[i, j] = (int)Bioms.Mountain;
                    else
                        world[i, j] = (int)Bioms.StonePlain;
                }
                else if (biom > .52f)
                    world[i, j] = Random.value > .5 ? (int)Bioms.Plain : (int)Bioms.Forest;
                else if (biom > .49f)
                    world[i, j] = (int)Bioms.Desert;
                else if (biom > .4f)
                    world[i, j] = Random.value > .05 ? (int)Bioms.Ocean : (int)Bioms.OceanMountain;
                else
                    world[i, j] = (int)Bioms.Ocean; ;
            }
        }


        //Place the spaceship and focus the camera on it
        while (true)
        {
            int x = (int)(Random.value * width), z = (int)(Random.value * height);
            if (world[x, z] == (int)Bioms.Plain)
            {
                Vector3 shipPos;
                if (z % 2 == 0)
                    shipPos = new Vector3(x * 1.73206f, 0, z * 1.5f);
                else
                    shipPos = new Vector3(x * 1.73206f + 0.86603f, 0, z * 1.5f);

                //Place the ship
                world[x, z] = (int)Bioms.SpaceShip;

                //Place the first drone over the ship
                GameObject g = Instantiate(drone);
                g.transform.position = shipPos + drone.transform.position;

                //Focus the camera
                Camera.main.transform.position = shipPos + new Vector3(0, Camera.main.transform.position.y, -8f);
                break;
            }
        }
    }


    //Make the calculated world visible
    public void showWorld()
    {
        GameObject g;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                g = Instantiate<GameObject>(models[world[i, j]]);

                Vector3 v;
                if (j % 2 == 0)
                    v = new Vector3(i * 1.73206f, 0, j * 1.5f);
                else
                    v = new Vector3(i * 1.73206f + 0.86603f, 0, j * 1.5f);

                g.transform.position = v;
                g.transform.parent = transform;
            }
        }
    }
}
