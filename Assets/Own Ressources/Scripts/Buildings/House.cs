using UnityEngine;
using System.Collections;

public class House : Building {
    public int moneyProduction = 3;

    void Start()
    {
        World.instance.buildings.Add(this);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public override void nextRound()
    {
        World.instance.GetComponent<Inventory>().addRessource(Ressources.Money, moneyProduction);
        createRessourceText(moneyProduction, World.instance.ressourceTexts[(int)Ressources.Money]);
    }
}
