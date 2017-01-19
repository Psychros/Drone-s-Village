using UnityEngine;
using System.Collections;

public class NPCBox : MonoBehaviour {

	public void cutTree()
    {
        InputManager.instance.selectedNPC.cutTree();
    }
}
