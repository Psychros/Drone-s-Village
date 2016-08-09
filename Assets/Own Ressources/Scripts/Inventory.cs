using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour {
    public int capacity = 20;
    public int money = 0;
    public int[] ressources = new int[System.Enum.GetNames(typeof(Ressources)).Length];
    public static int numberRessourceTextFields = 3;
    public Text[] ressourcesTextFields = new Text[System.Enum.GetNames(typeof(Ressources)).Length];
    bool firstUpdate = true;


    void Update()
    {
        if (firstUpdate)
        {
            for (int i = 0; i < ressourcesTextFields.Length; i++)
            {
                ressourcesTextFields[i].text = formatNumber(ressources[i]);
            }
            firstUpdate = false;
        }
    }

    /*
     * Add ressources to the inventory.
     * If the the final value is out of the bounds of the inventory, the difference is given back
     */
    public int addRessources(Ressources ressource, int number)
    {
        int currentNumber = ressources[(int)ressource];
        ressources[(int)ressource] += number;

        if(ressources[(int)ressource] > capacity)
        {
            ressources[(int)ressource] = capacity;
            return ressources[(int)ressource] + number - capacity;
        }
        else if(ressources[(int)ressource] < 0)
        {
            ressources[(int)ressource] = 0;
            return currentNumber + number;
        }

        //Update the textFields
        for (int i = 0; i < numberRessourceTextFields; i++)
        {
            ressourcesTextFields[(int)ressource].text = formatNumber(ressources[(int)ressource]);
        }

        return 0;
    }


    //Format the number to a string
    public string formatNumber(int number)
    {
        string s = "";
        string ending = "";
        float number2 = number;

        //Get the correct ending
        if (number > 999) {
            ending = "K";
            number2 = number / 1000f;
        }
        if (number > 999999) {
            ending = "M";
            number2 = number / 1000f;
        }
        if (number > 999999999) {
            ending = "G";
            number2 = number / 1000f;
        }

        //Format the number
        print(number2);
        if (!ending.Equals(""))
        {
            if (number2 >= 10f)
                s += string.Format("{0:0}", number2);
            else if (number >= 10)
            {
                s += string.Format("{0:0.0}", number2);
            }

            //Add the ending
            s += ending;
        }
        else
        {
            s += number;
        }

        return s;
    }
}
