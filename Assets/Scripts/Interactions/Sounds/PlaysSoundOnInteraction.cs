using UnityEngine;
using System.Collections;

public class PlaysSoundOnInteraction : Interaction {

	AudioSource _audioSource;

	public AudioClip soundOnOpen;
	public float lowPitchRange = 1f;
	public float highPitchRange = 1f;

	void Start() {
		if (soundOnOpen != null) {
			_audioSource = GetComponent<AudioSource>() != null ? GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
			_audioSource.clip = soundOnOpen;
		}	
	}

	public override void Interact ()
	{
		if (_audioSource != null) {
			_audioSource.pitch = Random.Range(lowPitchRange, highPitchRange);
			_audioSource.Play();
		}
	}
}
