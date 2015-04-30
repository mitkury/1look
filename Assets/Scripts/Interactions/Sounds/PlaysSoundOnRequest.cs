using UnityEngine;
using System.Collections;

public class PlaysSoundOnRequest : SoundInteraction {

	public AudioClipData[] sounds;

	public void PlayOneShot (int index) {
		var data = sounds[index];
		base.PlayOneShot (data);
	}

}
