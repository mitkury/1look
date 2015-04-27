using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]
public class HingeDoorOpener : Interaction {

	// OLD;
	public Vector3 targetLocalRotation;

	public Vector3 axis = Vector3.forward;
	public float add = -45f;
	public float time = 1f;
	public Interaction receiver;

	public override void Interact (){
		StartCoroutine(OpenCo());
	}

	IEnumerator OpenCo() {
		//LeanTween.rotateLocal(gameObject, targetLocalRotation, 1f).setEase(LeanTweenType.easeOutSine);
		LeanTween.rotateAroundLocal(gameObject, axis, add, time).setEase(LeanTweenType.easeInOutSine);

		yield return new WaitForSeconds(time);

		if (receiver != null) {
			receiver.Interact();
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		
		Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + axis);
	}

}
