using UnityEngine;
using System.Collections.Generic;

public class Store : Building {
    public static int size = 10;

	// Use this for initialization
	void Start () {
        costs.Add(new Cost(Ressources.Wood, 5));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void buildBuilding()
    {
        World.instance.inventory.addCapacity(size);
    }
}
