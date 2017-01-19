using UnityEngine;
using System.Collections;

public class Woodcutter : Building {

	// Use this for initialization
	void Start () {
        World.instance.buildings.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void nextRound()
    {
        World.instance.GetComponent<Inventory>().addRessource(Ressources.Wood, 1);
    }
}
