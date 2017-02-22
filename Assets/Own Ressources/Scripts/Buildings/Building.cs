using UnityEngine;
using System.Collections.Generic;

public abstract class Building : MonoBehaviour {
    public List<Cost> costs = new List<Cost>();
    public List<Bioms> bioms = new List<Bioms>();   //The bioms where the player can build this building

    void Start()
    {

    }

    public virtual void buildBuilding()
    {
        
    }

    public virtual void nextRound()
    {
        World.instance.GetComponent<Inventory>().addRessource(Ressources.Wood, 1);
    }

    public void createRessourceText(int n, GameObject text)
    {
        for (int i = 0; i < n; i++)
        {
            RessourceText t = Instantiate(text, transform).GetComponent<RessourceText>();
            t.transform.position = transform.position - new Vector3(0, -.3f * i);
            t.startMovement();
        }
    }
}
