using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	
	[System.NonSerialized]
	public FiniteStateMachine stateMachine;
	[System.NonSerialized]
	public FiniteStateMachine accessibilityStateMachine;
	[System.NonSerialized]
	public Vector3 initialPosition;
	public GameObject body;
	public int width = 2;
	public Material defaultMetarial;
	public Material lockedMaterial;
	
	void Start() {
		Init();
	}

	public void Init() {
		initialPosition = body.transform.position;
		
		accessibilityStateMachine = gameObject.AddComponent<FiniteStateMachine>();
		stateMachine = gameObject.AddComponent<FiniteStateMachine>();
		
		accessibilityStateMachine.currentState = new DoorIsLockedState(this);
		stateMachine.currentState = new DoorIsClosedState(this);
	}
}
