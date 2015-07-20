using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))] 
public class ObtainableItem : InteractiveThing {

	public bool hasItsOwnOnGrabDistance;
	public float onGrabDistance;
	public Vector3 customOnGrabRotation;
	public Animation onGrabLegacyAnimation;

	protected override void Init ()
	{
		/*
		// Add new interactions before initializing the base class.
		if (GetComponent<CauldronUser>() == null) {
			gameObject.AddComponent<CauldronUser>();
		}
		*/
		/*
		if (GetComponent<SurfaceUser>() == null) {
			gameObject.AddComponent<SurfaceUser>();
		}
		*/

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

		// Stop any animations that may prevent the object from being obtained.
		if (GetComponent<Animator>()) {
			GetComponent<Animator>().enabled = false;
		}

		gameObject.SendMessage("OnItemTakeByVisitor", this, SendMessageOptions.DontRequireReceiver);
		
		if (transform.parent != null) {
			transform.parent.SendMessage("OnItemTakeByVisitor", this, SendMessageOptions.DontRequireReceiver);
		}

		if (onGrabLegacyAnimation != null) {
			onGrabLegacyAnimation.enabled = true;
			onGrabLegacyAnimation.Play();
		}
	}

	public void Frees() {
		/*
		if (onGrabLegacyAnimation != null) {
			onGrabLegacyAnimation.enabled = false;
		}
		*/

		gameObject.SendMessage("OnItemFreeByVisitor", this, SendMessageOptions.DontRequireReceiver);
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
