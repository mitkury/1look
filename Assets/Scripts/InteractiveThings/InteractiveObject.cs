using UnityEngine;
using System.Collections;

public class InteractiveObject : InteractiveThing {

	public bool isAbleToBeActivatedOnItsOwn = true;

	/*
	public bool IsAbleToBeActivatedWith(InteractiveThing thing) {
		return false;
	}
	*/

	public void Activate() {
		if (!isAbleToBeActivatedOnItsOwn || interactions.Count == 0)
			return;

		// It looses the ability to interact right after its activation. 
		// In order to resume the ability to interact change this variable from an Interaction component.
		isAbleToInteract = false;

		foreach (Interaction interaction in interactions) {
			interaction.Interact();
		}
	}

	/*
	public void ActivateWith(InteractiveThing thing) {}
	*/

}
