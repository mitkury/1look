using UnityEngine;
using System.Collections;

public class DoorIsOpenState : IState {
	
	protected Door door;

	void SendOnDoorOpenMessage() {
		door.SendMessage("OnDoorOpen", SendMessageOptions.DontRequireReceiver);
	}
	
	public DoorIsOpenState(Door door) {
		this.door = door;
	}
	
	public void OnEnterState () {
		var tween = LeanTween.move(door.body, new Vector3(door.initialPosition.x - door.width + 0.025f, door.initialPosition.y, door.initialPosition.z), 1.0f);
		tween.setEase(LeanTweenType.easeOutCubic);
		tween.setOnComplete(SendOnDoorOpenMessage);

		if (door.GetComponent<AudioSource>() != null) {
			door.GetComponent<AudioSource>().Play();
		}


	}
	
	public void OnExitState () {}
	
	public void Update () {
		/*
		if (!door.sensorIsOn && door.accessibilityStateMachine.currentState is DoorIsUnlockedState) {
			door.stateMachine.currentState = new DoorIsClosedState(door);
		}
		*/
	}
}
