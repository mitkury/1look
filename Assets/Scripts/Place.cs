using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameInstantiator))]
public class Place : MonoBehaviour {

	public Color background;
	public Waypoint vantagePoint;

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
