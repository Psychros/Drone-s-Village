using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor :  Editor{

    Inventory inventory;
    bool showRessources = true;
    bool showTextFields = true;
    bool showStore      = false;

	public override void OnInspectorGUI()
    {
        inventory = (Inventory)target;

        //Change the capacity
        inventory.capacity = EditorGUILayout.DelayedIntField("Capacity:", inventory.capacity);

        //StoreTextFields
        getTextFieldsForArray("TextFieldsForStore:", ref showStore, ref inventory.storeTextFields, null);

        //Ressources
        getObjectsForArray("Ressources:", ref showRessources, ref inventory.ressources, typeof(Ressources));

        //RessourceTextFields
        getTextFieldsForArray("TextFieldsForRessources:", ref showTextFields, ref inventory.ressourcesTextFields, typeof(Ressources));
        
    }

    public void getObjectsForArray(string name, ref bool toggle, ref int[] array, System.Type enuM = null)
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
                if(enuM != null)
                    array[i] = EditorGUILayout.DelayedIntField("     " + System.Enum.GetName(enuM, i) + ":", array[i]);
                else
                    array[i] = EditorGUILayout.DelayedIntField("     " + i + ":", array[i]);
            }
        }
    }


    //Get the Textfields for the ressources
    public void getTextFieldsForArray(string name, ref bool toggle, ref Text[] array, System.Type enuM)
    {
        toggle = EditorGUILayout.Toggle(name, toggle);
        if (toggle)
        {
            //Change the size of the array
            Text[] arrayNew = new Text[EditorGUILayout.DelayedIntField("     Size:", array.GetLength(0))];
            if (arrayNew.GetLength(0) != array.GetLength(0) && arrayNew.GetLength(0) > 0)
            {
                //Copy the values of array
                if (arrayNew.GetLength(0) > array.GetLength(0))
                    for (int i = 0; i < array.GetLength(0); i++)
                        for (int j = 0; j < Inventory.numberRessourceTextFields; j++)
                        {
                            arrayNew[i] = array[i];
                        }
                else
                    for (int i = 0; i < arrayNew.GetLength(0); i++)
                        for (int j = 0; j < Inventory.numberRessourceTextFields; j++)
                        {
                            arrayNew[i] = array[i];
                        }

                array = arrayNew;
            }


            //Shows the array
            for (int i = 0; i < array.GetLength(0); i++)
            {
                EditorGUILayout.LabelField("");
                if(enuM != null)
                    EditorGUILayout.LabelField("     " + System.Enum.GetName(enuM, i) + ":");

                array[i] = (Text)EditorGUILayout.ObjectField("     Text:", array[i], typeof(Text));
            }
        }
    }
}
