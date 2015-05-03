using UnityEngine;
using System.Collections;

public class PlaysSoundOnInteraction : SoundInteraction {

	public AudioClipData audioClipData;

	void Start() {
		SetupAudioSource(audioClipData);
	}

	public override void Interact () {
		PlayOneShot(audioClipData);
	}
}
