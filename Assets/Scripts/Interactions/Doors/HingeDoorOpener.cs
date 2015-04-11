using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HingeDoorOpener : Interaction {

	public Vector3 targetLocalRotation;

	public override void Interact ()
	{
		LeanTween.rotateLocal(gameObject, targetLocalRotation, 1f).setEase(LeanTweenType.easeOutSine);
	}

}
