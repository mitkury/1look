using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameInstantiator))]
public class Place : MonoBehaviour {
	
	public Waypoint enterPoint;
	public Waypoint vantagePoint;
	public Waypoint exitPoint;

	// Lobbies are used for helping to transport a player from one place to another place seamlesly. 
	// An exiting lobby of a current place has to be an exact copy of an enterring lobby of a next place.
	public Transform enterLobby;
	public Transform exitLobby;

	public List<InteractiveThing> interactiveThings { get; private set; }

	void Awake() {
		GetComponent<GameInstantiator>().Init();

		King.placeManager.AddPlace(this);
	}

	public void Add(object something) {
		if (interactiveThings == null) {
			interactiveThings = new List<InteractiveThing>();
		}

		if (something is InteractiveThing) {
			interactiveThings.Add(something as InteractiveThing);
		}
	}


}
