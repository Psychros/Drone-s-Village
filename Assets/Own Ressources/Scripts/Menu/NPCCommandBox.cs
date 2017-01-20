using UnityEngine;
using System.Collections;

public class NPCCommandBox : MonoBehaviour {

	public void cutTree()
    {
        InputManager.instance.selectedNPC.cutTree();
    }

    public void build(Building building)
    {
        InputManager.instance.selectedNPC.buildBuilding(building);
    }
}
