using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class PokesBySight : Interaction {

	public Vector3 force = Vector3.zero;
	[FormerlySerializedAs("audioClipData")]
	public AudioClipData audioOnPoke;
	public List<AudioClipDataOnImpact> audioOnImpact;

	void Start() {
		var playsOnInteractoin = gameObject.AddComponent<PlaysSoundOnInteraction>();
		playsOnInteractoin.audioClipData = audioOnPoke;

		var playsOnImpact = gameObject.AddComponent<PlaysSoundOnImpact>();
		playsOnImpact.audioClipsData = audioOnImpact;
	}

	IEnumerator ActivateInSecCo(float time) {
		yield return new WaitForSeconds(time);

		GetComponent<InteractiveObject>().isAbleToInteract = true;
	}

	public override void Interact ()
	{
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		
		//StartCoroutine(ActivateInSecCo(1f));
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;

		var length = 1 / GetComponent<Rigidbody>().mass;
		Gizmos.DrawLine(transform.position, transform.position + force * length);
	}

}
