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

	void Start () {
		Init();
	}

	protected virtual void Init() {
		//interactions = GetComponents<Interaction>().ToList();
		if (interactions == null)
			interactions = new List<Interaction>();
		
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

		if (transform.root != null) {
			transform.root.SendMessage("Add", this, SendMessageOptions.DontRequireReceiver);
		}

		if (transform.parent != null && transform.parent != transform.root) {
			transform.parent.SendMessage("Add", this, SendMessageOptions.DontRequireReceiver);
		}

		yield break;

		/*
		if (transform.parent != null && transform.parent != King.placeManager.currentPlace) {
			transform.parent.SendMessage("Add", this, SendMessageOptions.DontRequireReceiver);
		}
	
		while (King.placeManager.currentPlace == null)
			yield return null;

		King.placeManager.currentPlace.Add(this);
		*/
	}

	void OnEnable() {
		//StartCoroutine(AddToPlaceCo());
	}

	void OnDisable() {
		// TODO: remove from things;
	}

	public void AddInteraction(Interaction interaction) {
		if (interactions == null)
			interactions = new List<Interaction>();

		if (interactions.Find(i => i == interaction) == null)
			interactions.Add(interaction);
		else
			Debug.LogWarning("Trying to add an interaction "+interaction+" that is already added");
	}

}
