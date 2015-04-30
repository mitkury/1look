using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class AudioClipDataOnImpact : AudioClipData {
	public float velocityToVolume = 0.2f;
	public float requiredMinVelocity = 1f;
}

public class PlaysSoundOnImpact : SoundInteraction {

	public List<AudioClipDataOnImpact> data;
	Rigidbody _rigidbody;
	
	float lowPitchRange = 0.75f;
	float highPitchRange = 1.5f;
	float velocityToVolume = 0.2f;
	float velocityClipSplit = 10f;

	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
	}


	void OnCollisionEnter(Collision collision) {
		Debug.Log(collision.relativeVelocity.magnitude);
		/*
		if (collision.relativeVelocity.magnitude > 2) {
			_audioSource.Play();
		}
		*/
	}

}
