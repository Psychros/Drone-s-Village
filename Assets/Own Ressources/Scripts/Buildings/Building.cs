using UnityEngine;
using System.Collections.Generic;

public abstract class Building : MonoBehaviour {
    public List<Cost> costs = new List<Cost>();

    public virtual void buildBuilding()
    {
        
    }
}
