using UnityEngine;
using System.Collections;

public class DoorIsClosedState : IState {

	protected Door door;
	
	public DoorIsClosedState(Door door) {
		this.door = door;
	}
	
	public void OnEnterState () {
		var tween = LeanTween.move(door.body, door.initialPosition, 0.25f);
		tween.setEase(LeanTweenType.easeInCubic);

		if (door.GetComponent<AudioSource>() != null) {
			door.GetComponent<AudioSource>().Play ();
		}

		door.SendMessage("OnDoorClosed", SendMessageOptions.DontRequireReceiver);
	}
	
	public void OnExitState () {}
	
	public void Update () {
		/*
		if (door.sensorIsOn && door.accessibilityStateMachine.currentState is DoorIsUnlockedState) {
			door.stateMachine.currentState = new DoorIsOpenState(door);
		}
		*/
	}
	
}
