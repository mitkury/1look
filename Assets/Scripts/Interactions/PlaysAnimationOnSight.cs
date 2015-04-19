using UnityEngine;
using System.Collections;

public class PlaysAnimationOnSight : Interaction {

	public override void Interact () {
		var animation = GetComponent<Animation>();
		animation.Play();
	}

}
