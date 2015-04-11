using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Door))]
public class DoorOpener : Interaction {

	public void OnSensorEnter() {
		var door = GetComponent<Door>();
		door.stateMachine.currentState = new DoorIsOpenState(door);
	}

	public void OnSensorExit() {
		var door = GetComponent<Door>();
		door.stateMachine.currentState = new DoorIsClosedState(door);
	}

}
