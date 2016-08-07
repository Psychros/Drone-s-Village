using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor :  Editor{

    bool showRessources = true;

	public override void OnInspectorGUI()
    {
        Inventory inventory = (Inventory)target;

        //Change the capacity
        inventory.capacity = EditorGUILayout.DelayedIntField("Capacity:", inventory.capacity);

        //Ressources
        getObjectsForArray("Ressources", ref showRessources, ref inventory.ressources, typeof(Ressources));
    }

    public void getObjectsForArray(string name, ref bool toggle, ref int[] array, System.Type enuM)
    {
        toggle = EditorGUILayout.Toggle(name, toggle);
        if (toggle)
        {
            //Change the size of the array
            int[] arrayNew = new int[EditorGUILayout.DelayedIntField("     Size:", array.Length)];
            if (arrayNew.Length != array.Length && arrayNew.Length > 0)
            {
                //Copy the values of array
                if (arrayNew.Length > array.Length)
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
                array[i] = EditorGUILayout.DelayedIntField("     " + System.Enum.GetName(enuM, i) + ":", array[i]);
            }
        }
    }
}
