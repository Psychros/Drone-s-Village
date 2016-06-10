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
    public GameObject droneModel;
    [HideInInspector]
    public NPC drone;

    [HideInInspector]
    public Pathfinder pathfinder;
    public int width = 50,
              height = 50;
    private int[,] world;



    // Use this for initialization
    void Start()
    {
        instance = this;
        world = new int[width, height];

        //Generate the world
        generate();
        showWorld();

        //Generate the Pathfinder
        pathfinder = gameObject.AddComponent<Pathfinder>();
        pathfinder.generateNodes(width, height);
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
            int x = (int)(Random.value * width), 
                z = (int)(Random.value * height);
            if (world[x, z] == (int)Bioms.Plain)
            {
                Vector3 shipPos = Hexagon.getWorldPosition(x, z);

                //Place the ship
                world[x, z] = (int)Bioms.SpaceShip;

                //Place the first drone over the ship
                GameObject g = Instantiate(droneModel);
                drone = g.AddComponent<NPC>();
                g.transform.position = shipPos + droneModel.transform.position;

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

                g.transform.position = Hexagon.getWorldPosition(i, j);
                g.transform.parent = transform;
            }
        }
    }
}
