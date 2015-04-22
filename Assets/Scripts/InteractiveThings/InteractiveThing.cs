using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public abstract class InteractiveThing : MonoBehaviour {

	//protected List<Interaction> interactions = new List<Interaction>();
	public List<Interaction> interactions { get; private set; }

	public bool hasItsOwnFocusTime;
	public float focusTimeSec;
	public bool isAbleToInteract = true;
	public Transform body;

	void Start () {
		Init();
	}

	protected virtual void Init() {
		interactions = GetComponents<Interaction>().ToList();
		
		/*
		if (transform.root != null) {
			transform.root.SendMessage("Add", this, SendMessageOptions.DontRequireReceiver);
		}
		*/
		StartCoroutine(AddToPlaceCo());
	}

	IEnumerator AddToPlaceCo() {
		if (King.placeManager == null)
			yield break;

		if (transform.parent != null && transform.parent != King.placeManager.currentPlace) {
			transform.parent.SendMessage("Add", this, SendMessageOptions.DontRequireReceiver);
		}

		while (King.placeManager.currentPlace == null)
			yield return null;

		King.placeManager.currentPlace.Add(this);
	}

}
