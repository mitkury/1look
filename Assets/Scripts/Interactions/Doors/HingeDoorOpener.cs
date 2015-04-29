using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody))]
public class HingeDoorOpener : Interaction {

	AudioSource _audioSource;
	
	// OLD;
	public Vector3 targetLocalRotation;

	public Vector3 axis = Vector3.forward;
	public float add = -45f;
	public float time = 1f;
	public Interaction receiver;
	public AudioClip soundOnOpen;

	void Start() {
		if (soundOnOpen != null) {
			_audioSource = GetComponent<AudioSource>() != null ? GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
			_audioSource.clip = soundOnOpen;
		}
	}

	IEnumerator OpenCo() {
		//LeanTween.rotateLocal(gameObject, targetLocalRotation, 1f).setEase(LeanTweenType.easeOutSine);
		LeanTween.rotateAroundLocal(gameObject, axis, add, time).setEase(LeanTweenType.easeInOutSine);

		if (_audioSource != null)
			_audioSource.Play();

		yield return new WaitForSeconds(time);

		if (_audioSource != null)
			_audioSource.Stop();

		if (receiver != null) {
			receiver.Interact();
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		
		//Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + axis);
	}

	public override void Interact (){
		StartCoroutine(OpenCo());
	}

}
