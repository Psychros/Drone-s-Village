using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    public int capacity = 20;
    public int money = 0;
    public int[] ressources = new int[System.Enum.GetNames(typeof(Ressources)).Length];
    public static int numberRessourceTextFields = 3;
    public Text[] ressourcesTextFields1 = new Text[System.Enum.GetNames(typeof(Ressources)).Length];
    public Text[] ressourcesTextFields2 = new Text[System.Enum.GetNames(typeof(Ressources)).Length];
    public Text[] storeTextFields = new Text[1];
    bool firstUpdate = true;


    void Update()
    {
        if (firstUpdate)
        {
            //Show the capacity
            addCapacity(0);

            //Show all startressources
            for (int i = 0; i < ressourcesTextFields1.Length; i++)
            {
                addRessource((Ressources)i, 0);
            }
            firstUpdate = false;
        }
    }

    /*
     * Add ressources to the inventory.
     * If the the final value is out of the bounds of the inventory, the difference is given back
     * The bounds are not important for money
     */
    public int addRessource(Ressources ressource, int number)
    {
        int currentNumber = ressources[(int)ressource];
        ressources[(int)ressource] += number;

        if(ressource != Ressources.Money && ressources[(int)ressource] > capacity)
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
            if(ressourcesTextFields1[(int)ressource] != null)
                ressourcesTextFields1[(int)ressource].text = formatNumber(ressources[(int)ressource]);
            if (ressourcesTextFields2[(int)ressource] != null)
                ressourcesTextFields2[(int)ressource].text = formatNumber(ressources[(int)ressource]);
        }

        return 0;
    }

    public void removeRessources(List<Cost> costs)
    {
        foreach (Cost c in costs)
        {
            addRessource(c.ressource, -c.number);
        }
    }


    //Change the capacity
    public void addCapacity(int number)
    {
        capacity += number;
        if (capacity < 0)
            capacity = 0;

        for (int i = 0; i < storeTextFields.Length; i++)
        {
            storeTextFields[i].text = formatNumber(capacity);
        }
    }

    public bool hasRessource(Ressources ressource, int number)
    {
        return ressources[(int)ressource] >= number;
    }

    public bool hasRessources(List<Cost> costs)
    {
        foreach (Cost c in costs)
        {
            if (!hasRessource(c.ressource, c.number))
                return false;
        }
        return true;
    }

    public int getNumber(Ressources ressource)
    {
        return ressources[(int)ressource];
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
