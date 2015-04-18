using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public abstract class InteractiveThing : MonoBehaviour {

	protected List<Interaction> interactions = new List<Interaction>();

	public bool hasItsOwnFocusTime;
	public float focusTimeSec;
	public bool isAbleToInteract = true;
	public Transform body;

	void Start () {
		interactions = GetComponents<Interaction>().ToList();

		/*
		if (transform.root != null) {
			transform.root.SendMessage("Add", this, SendMessageOptions.DontRequireReceiver);
		}
		*/

		StartCoroutine(AddToPlaceCo());
	}

	IEnumerator AddToPlaceCo() {
		while (King.placeManager.currentPlace == null)
			yield return null;

		King.placeManager.currentPlace.Add(this);
	}

}
