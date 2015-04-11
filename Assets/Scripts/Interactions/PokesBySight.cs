using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PokesBySight : Interaction {

	public Vector3 force = Vector3.zero;

	public override void Interact ()
	{
		GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

		//StartCoroutine(ActivateInSecCo(1f));
	}

	IEnumerator ActivateInSecCo(float time) {
		yield return new WaitForSeconds(time);

		GetComponent<InteractiveObject>().isAbleToInteract = true;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;

		Gizmos.DrawLine(transform.position, transform.position + force);
	}

}
