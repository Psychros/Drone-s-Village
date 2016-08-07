using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
    public int capacity = 20;
    public int[] ressources = new int[System.Enum.GetNames(typeof(Ressources)).Length];

}
