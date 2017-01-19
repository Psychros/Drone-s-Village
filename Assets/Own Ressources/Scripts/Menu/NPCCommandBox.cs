using UnityEngine;
using System.Collections;

public class NPCCommandBox : MonoBehaviour {

	public void cutTree()
    {
        InputManager.instance.selectedNPC.cutTree();
    }

    public void build()
    {
        InputManager.instance.selectedNPC.buildBuilding();
    }
}
