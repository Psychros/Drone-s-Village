using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour{
    [HideInInspector] public int[,] chunkBiomes;
    [HideInInspector] public GameObject[,] hexagons;
    [HideInInspector] public GameObject[,] structures;
    [HideInInspector] public int posX, posZ;        //HexCoords
    public static int chunkSize = 8;

    public Chunk(int x, int z)
    {
        posX = x;
        posZ = z;
        transform.position = Hexagon.getWorldPosition(x, z);

        hexagons = new GameObject[chunkSize, chunkSize];
        structures = new GameObject[chunkSize, chunkSize];
    }


    /*
     * Must be called for generating the chunk
     */
    public void generateChunk()
    {
        generateBioms();
        generateModel();
    }


    /*
     * Generates the biomes
     */
    public void generateBioms()
    {
        chunkBiomes = new int[chunkSize, chunkSize];

        for (int i = 0; i < chunkSize; i++)
        {
            for (int j = 0; j < chunkSize; j++)
            {
                //Select the correct terrain
                float biom = Mathf.PerlinNoise(World.instance.offsetX + (float)i / chunkSize * 5f, World.instance.offsetZ + (float)j / chunkSize * 5f);

                if (biom > .85f)
                    chunkBiomes[i, j] = (int)Bioms.HighMountain;
                else if (biom > .8f)
                    chunkBiomes[i, j] = (int)Bioms.Mountain;
                else if (biom > .75f)
                {
                    if (Random.value > .3)
                        chunkBiomes[i, j] = (int)Bioms.Mountain;
                    else
                        chunkBiomes[i, j] = (int)Bioms.StonePlain;
                }
                else if (biom > .52f)
                    chunkBiomes[i, j] = Random.value > .5 ? (Random.value > .2 ? (int)Bioms.Plain : (int)Bioms.PlainDandelion) : (int)Bioms.Forest;
                else if (biom > .49f)
                    chunkBiomes[i, j] = Random.value > .2 ? (int)Bioms.Desert : (int)Bioms.DesertShell;
                else if (biom > .4f)
                    chunkBiomes[i, j] = Random.value > .05 ? (int)Bioms.Ocean : (int)Bioms.OceanMountain;
                else
                    chunkBiomes[i, j] = (int)Bioms.Ocean;
            }
        }
    }


    public void generateModel()
    {
        for (int i = 0; i < chunkSize; i++)
        {
            for (int j = 0; j < chunkSize; j++)
            {
                BiomData biomData = World.instance.biomsData[chunkBiomes[i, j]];

                //Generate the ground
                GameObject g = Instantiate(World.instance.biomModels[(int)biomData.biomModel]);
                g.transform.position = Hexagon.getWorldPosition(i + posX, j + posZ);
                g.transform.parent = transform;
                hexagons[i, j] = g;

                //Generate the structure
                if ((int)biomData.structure != (int)Structures.None)
                {
                    structures[i, j] = Instantiate<GameObject>(World.instance.structureModels[(int)biomData.structure]);
                    structures[i, j].transform.position = Hexagon.getWorldPosition(i + posX, j + posZ);
                    structures[i, j].transform.parent = this.transform;
                }
                else
                    structures[i, j] = null;
            }
        }
    }

    public void changeBiomChunkCoords(int x, int z, Bioms newBiom)
    {

    }

    public void changeBiomGlobalCoords(int x, int z, Bioms newBiom)
    {

    }

    public void changeStructureChunkCoords(int x, int z, Structures newStructure)
    {

    }

    public void changeStructureGlobalCoords(int x, int z, Structures newStructure)
    {

    }
}
