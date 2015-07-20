using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameInstantiator))]
public class Place : MonoBehaviour {

	public enum PlaceState {
		Playable,
		Transition
	}

	public Color background = Color.blue;
	public Color ambientColor = Color.grey;
	public Color fogColor = Color.grey;
	public float fogDensity = 0f;
	public Waypoint vantagePoint;
	public PlaceState placeState;

	public List<InteractiveThing> interactiveThings { get; private set; }

	void Awake() {
		if (King.Instance != null)
			gameObject.SetActive(false);

		interactiveThings = new List<InteractiveThing>();

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
