using UnityEngine;
using System.Collections;

public class Basketball : Interaction {

	public float throwForce = 5;
	public float unblockAfterThrowInSec = 3f;

	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		return thing.name == "ThrowingSurface";
	}

	public override void InteractWith(InteractiveThing thing) {

		var rigidbody = GetComponent<Rigidbody>();

		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;

		rigidbody.AddForce(King.visitor.sight.facingVector * throwForce, ForceMode.Impulse);

		StartCoroutine(UnblockCo());
	}

	IEnumerator UnblockCo() {
		yield return new WaitForSeconds(unblockAfterThrowInSec);

		var obtainableItem = GetComponent<ObtainableItem>();

		obtainableItem.isAbleToInteract = true;

		if (King.visitor.itemInHand != null) {
			King.visitor.inventory.AddItem(obtainableItem);
		} else { 
			King.visitor.Take(obtainableItem);
		}


	}
}
