using UnityEngine;
using System.Collections;

public class PlaysAnimationOnSight : Interaction {

	public bool isAbleToInteractAfterAnimation;
	public GameObject receiver;

	IEnumerator PlayAnimation() {
		if (receiver == null) 
			receiver = gameObject;
		
		var animation = receiver.GetComponent<Animation>();
		animation.Play();

		while(animation.isPlaying)
			yield return null;

		if (isAbleToInteractAfterAnimation) {
			GetComponent<InteractiveThing>().isAbleToInteract = true;
		}
	}

	public override void Interact () {
		StartCoroutine(PlayAnimation());
	}

}
