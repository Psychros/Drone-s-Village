using UnityEngine;
using System.Collections;

public class Pausemenu : MonoBehaviour {

	public void closeGame()
    {
        Application.Quit();
    }

    public void options()
    {

    }

    public void save()
    {

    }

    public void continueGame()
    {
        InputManager.instance.deactivateMenu(InputManager.instance.pausemenu);
    }
}
