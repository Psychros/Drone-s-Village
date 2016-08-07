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
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (GameObject)EditorGUILayout.ObjectField("     " + System.Enum.GetName(enuM, i) + ":", array[i], typeof(GameObject));
            }
        }
    }
}
