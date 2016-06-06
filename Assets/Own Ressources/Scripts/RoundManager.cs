using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {
    [HideInInspector]
    public RoundManager instance;

    [HideInInspector]
    public int round = 0;
    public Text text;

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void nextRound()
    {
        round++;
        text.text = "Round: " + round;
    }
}
