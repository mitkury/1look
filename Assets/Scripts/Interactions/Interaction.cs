using UnityEngine;
using System.Collections;

/* 
 * Interaction plays a few roles:
 * a) Determines if an InteractiveThing is able to interact with a given InteractiveThing.
 * b) Produces an interaction between InteractiveThings or sends an event to a Playmaker's FSM.
 * c) Uses separatley from InteractiveObjects and InteractiveThings for any generalized or specialized interactions.
 */

public abstract class Interaction : MonoBehaviour {

	void Awake() {
		transform.SendMessage("AddInteraction", this, SendMessageOptions.DontRequireReceiver);
	}

	public virtual bool IsAbleToInteractWith(InteractiveThing thing) {
		return false;
	}

	public virtual void Interact() { }

	public virtual void InteractWith(InteractiveThing thing) { }
}
