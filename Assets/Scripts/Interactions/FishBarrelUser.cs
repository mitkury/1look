using UnityEngine;
using System.Collections;

public class FishBarrelUser : Interaction {

	public InteractiveObject barrel;

	public override void Interact () {
		barrel.isAbleToInteract = true;
	}

	public void ReActivateBarrel() {
		Interact();
	}
}
