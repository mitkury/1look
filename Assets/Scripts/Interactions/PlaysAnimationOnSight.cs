using UnityEngine;
using System.Collections;

public class PlaysAnimationOnSight : Interaction {

	public bool isAbleToInteractAfterAnimation;
	public string stateName = "Start";
	public int stateLayer = 0;
	public GameObject receiver;

	IEnumerator PlayAnimation() {
		if (receiver == null) 
			receiver = gameObject;
		
		var animator = receiver.GetComponent<Animator>();
		var idleStateNameHash = animator.GetCurrentAnimatorStateInfo(stateLayer).shortNameHash;
		var stateHash = Animator.StringToHash(stateName);

		animator.Play(stateName, stateLayer);

		// Wait a frame for a new state to setup.
		yield return null;

		// While state is playing.
		while(stateHash == animator.GetCurrentAnimatorStateInfo(stateLayer).shortNameHash)
			yield return null;

		if (isAbleToInteractAfterAnimation) {
			GetComponent<InteractiveThing>().isAbleToInteract = true;
		}
	}

	public override void Interact () {
		StartCoroutine(PlayAnimation());
	}

}
