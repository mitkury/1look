using UnityEngine;
using System.Collections;

public class PlaysSoundOnInteraction : SoundInteraction {

	public AudioClipData audioClipData;

	public override void Interact () {
		PlayOneShot(audioClipData);
	}
}
