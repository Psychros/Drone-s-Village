using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor {

    static bool showBiomModels = false;
    static bool showStructureModels = false;
    static bool showBioms = false;

    public override void OnInspectorGUI()
    {
        World world = (World)target;

        //Edit the worldsize
        Vector2 worldSize = EditorGUILayout.Vector2Field("WorldSize: ", new Vector2(world.width, world.height));
        world.width = (int)worldSize.x;
        world.height = (int)worldSize.y;

        //BiomModels
        getObjectsForArray("BiomModels: ", ref showBiomModels, ref world.biomModels, typeof(BiomModels));

        //StructureModels
        getObjectsForArray("StructureModels: ", ref showStructureModels, ref world.structureModels, typeof(Structures));

        //Bioms
        showBioms = EditorGUILayout.Toggle("Bioms: ", showBioms);
        if (showBioms)
        {
            //Change the size of the array
            BiomData[] arrayNew = new BiomData[EditorGUILayout.DelayedIntField("     Size:", world.biomsData.Length)];
            if (arrayNew.Length != world.biomsData.Length && arrayNew.Length > 0)
            {
                //Copy the values of array
                if (arrayNew.Length > world.biomsData.Length)
                    for (int i = 0; i < world.biomsData.Length; i++)
                        arrayNew[i] = world.biomsData[i];
                else
                    for (int i = 0; i < arrayNew.Length; i++)
                        arrayNew[i] = world.biomsData[i];

                world.biomsData = arrayNew;
            }

            for (int i = 0; i < world.biomsData.Length; i++)
            {
                EditorGUILayout.LabelField("     " + System.Enum.GetName(typeof(Bioms), i));
                world.biomsData[i].biomModel = (BiomModels)EditorGUILayout.EnumPopup("          BiomModel:", world.biomsData[i].biomModel);
                world.biomsData[i].structure = (Structures)EditorGUILayout.EnumPopup("          Structure:", world.biomsData[i].structure);
                EditorGUILayout.LabelField("");
            }
        }
    }


    public void getObjectsForArray(string name, ref bool toggle, ref GameObject[] array, System.Type enuM)
    {
        toggle = EditorGUILayout.Toggle(name, toggle);
        if (toggle)
        {
            //Change the size of the array
            GameObject[] arrayNew = new GameObject[EditorGUILayout.DelayedIntField("     Size:", array.Length)];
            if(arrayNew.Length != array.Length && arrayNew.Length > 0)
            {
                //Copy the values of array
                if(arrayNew.Length > array.Length)
                    for (int i = 0; i < array.Length; i++)
                        arrayNew[i] = array[i];
                else
                    for (int i = 0; i < arrayNew.Length; i++)
                        arrayNew[i] = array[i];

                array = arrayNew;
            }

            //Shows the array
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (GameObject)EditorGUILayout.ObjectField("     " + System.Enum.GetName(enuM, i) + ":", array[i], typeof(GameObject));
            }
        }
    }
}
