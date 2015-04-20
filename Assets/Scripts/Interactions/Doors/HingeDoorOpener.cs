using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]
public class HingeDoorOpener : Interaction {

	// OLD;
	public Vector3 targetLocalRotation;

	public Vector3 axis = Vector3.forward;
	public float add = -45f;
	public float time = 1f;


	public override void Interact ()
	{
		//LeanTween.rotateLocal(gameObject, targetLocalRotation, 1f).setEase(LeanTweenType.easeOutSine);

		LeanTween.rotateAround(gameObject, axis, add, time);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		
		Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + axis);
	}

}
