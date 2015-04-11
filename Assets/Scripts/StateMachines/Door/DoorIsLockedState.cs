using UnityEngine;
using System.Collections;

public class DoorIsLockedState : IState {
	
	protected Door door;
	
	public DoorIsLockedState(Door door) {
		this.door = door;
	}
	
	public void OnEnterState () {
		//door.body.GetComponent<Renderer>().material = door.lockedMaterial;

		door.stateMachine.currentState = new DoorIsClosedState(door);
	}
	
	public void OnExitState () {}
	
	public void Update () {}
	
}