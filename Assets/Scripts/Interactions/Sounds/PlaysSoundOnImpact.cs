using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class AudioClipDataOnImpact : AudioClipData {
	public float velocityToVolume = 0.2f;
	public float requiredMinVelocity = 1f;
}

public class PlaysSoundOnImpact : SoundInteraction {

	public List<AudioClipDataOnImpact> audioClipsData;
	Rigidbody _rigidbody;

	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision collision) {
		if (audioClipsData.Count == 0)
			return;

		var magnitude = collision.relativeVelocity.magnitude;
		var result = audioClipsData.Where(d => magnitude >= d.requiredMinVelocity).ToArray();

		if (result.Length == 0)
			return;

		// Get an audio with a max 'requiredMinVelocity'.
		var audioClipData = result[result.Length - 1];
		var hitVolume = audioClipData.velocityToVolume * magnitude;

		audioClipData.volume = hitVolume;
		PlayOneShot(audioClipData);
	}

}
