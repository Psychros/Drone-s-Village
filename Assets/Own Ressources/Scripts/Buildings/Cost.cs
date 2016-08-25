using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Cost{
	public Ressources ressource;
	public int number;

	public Cost(Ressources ressource, int number){
		this.ressource = ressource;
		this.number = number;
	}
}
