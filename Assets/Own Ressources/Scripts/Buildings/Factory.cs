using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Factory : Building {
    public List<Cost> productCosts = new List<Cost>();
    public Cost product;

    void Start()
    {
        World.instance.buildings.Add(this);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public override void nextRound()
    {
        Inventory i = World.instance.GetComponent<Inventory>();
        if (i.hasRessources(productCosts))
        {
            i.removeRessources(productCosts);
            World.instance.GetComponent<Inventory>().addRessource(product.ressource, product.number);
        }
    }
}
