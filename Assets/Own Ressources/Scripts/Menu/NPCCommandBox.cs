using UnityEngine;
using System.Collections;

public class NPCCommandBox : MonoBehaviour {

	public void cutTree()
    {
        InputManager.instance.selectedNPC.cutTree();
    }

    public void build(string building)
    {
        if(building.Equals("StoreHouse"))
            InputManager.instance.selectedNPC.buildBuilding(Structures.StoreHouse);
        else if(building.Equals("Woodcutter"))
            InputManager.instance.selectedNPC.buildBuilding(Structures.Woodcutter);

    }
}
