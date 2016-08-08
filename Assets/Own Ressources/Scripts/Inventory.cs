using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
    public int capacity = 20;
    public int money = 0;
    public int[] ressources = new int[System.Enum.GetNames(typeof(Ressources)).Length];


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

        return 0;
    }
}
