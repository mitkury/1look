using UnityEngine;
using System.Collections;



public class PlaysSoundOnImpact : Interaction {
	
	AudioSource _audioSource;
	Rigidbody _rigidbody;
	
	float lowPitchRange = 0.75f;
	float highPitchRange = 1.5f;
	float velocityToVolume = 0.2f;
	float velocityClipSplit = 10f;

	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
	}

	void OnCollisionEnter(Collision collision) {

		if (collision.relativeVelocity.magnitude > 2) {
			_audioSource.Play();
		}
	}

}
