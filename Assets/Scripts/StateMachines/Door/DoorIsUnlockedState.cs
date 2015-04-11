using UnityEngine;
using System.Collections;

public class DoorIsUnlockedState : IState {

	protected Door door;

	public DoorIsUnlockedState(Door door) {
		this.door = door;
	}

	public void OnEnterState () {
		//door.body.GetComponent<Renderer>().material = door.defaultMetarial;
	}
	
	public void OnExitState () {}
	
	public void Update () {}

}
