using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour {
    [HideInInspector]
    public RoundManager instance;

    [HideInInspector]
    public int round = 0;

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
    }
}
