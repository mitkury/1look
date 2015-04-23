using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))] 
public class ObtainableItem : InteractiveThing {
	
	protected override void Init ()
	{
		// Add new interactions before initializing the base class.
		if (GetComponent<CauldronUser>() == null) {
			gameObject.AddComponent<CauldronUser>();
		}

		base.Init();
	}

	public bool IsAbleToInteractWith(InteractiveThing thing) {
		if (!thing.isAbleToInteract)
			return false;

		foreach (Interaction interaction in interactions) {
			if (interaction.IsAbleToInteractWith(thing)) {
				return true;
			}
		}

		return false;
	}

	public void Obtains() {
		var rigidbody = GetComponent<Rigidbody>();
		if (rigidbody != null) {
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
		}

		if (transform.parent != null) {
			transform.parent.SendMessage("OnItemTakeByVisitor", SendMessageOptions.DontRequireReceiver);
		}

		if (GetComponent<Animation>()) {
			GetComponent<Animation>().enabled = false;
		}
	}

	public bool Interact(InteractiveThing thing) {
		Debug.Log("Visitor tries to use "+this+" with "+thing);

		// Use interaction script to determin if this thing can interact with an object.
		foreach (Interaction interaction in interactions) {
			if (interaction.IsAbleToInteractWith(thing)) {
				interaction.InteractWith(thing);
				isAbleToInteract = false;
			}
		}

		return true;
	}
}
