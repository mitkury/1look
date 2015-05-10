using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Door))]
public class DoorPuzzleOpener : Interaction {

	public DoorButton button;
	public int keysAmountGoal = 1;
	public int keysInUse;

	public override void Interact() {
		var door = GetComponent<Door>();

		if (door.accessibilityStateMachine.currentState is DoorIsUnlockedState) {
			door.stateMachine.currentState = new DoorIsOpenState(door);
		} else {
			GetComponent<InteractiveThing>().isAbleToInteract = true;
		}
	}

	public void OnKeyUse() {
		keysInUse++;

		if (keysInUse >= keysAmountGoal) {
			var door = GetComponent<Door>();
			door.accessibilityStateMachine.currentState = new DoorIsUnlockedState(door);

			var interactiveObject = GetComponent<InteractiveObject>();
			interactiveObject.isAbleToInteract = true;

			if (button != null) {
				button.Activate();
			}
		}
	}

	public void OnSensorExit() {
		var door = GetComponent<Door>();
		door.stateMachine.currentState = new DoorIsClosedState(door);
	}

}
