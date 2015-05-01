using UnityEngine;
using System.Collections;

public class PlaysSoundOnRequest : SoundInteraction {

	public AudioClipData[] sounds;

	IEnumerator PlayOneShotCo(int index, float afterSec) {
		yield return new WaitForSeconds(afterSec);
		PlayOneShot(index);
	}

	public void PlayOneShot(int index) {
		var data = sounds[index];
		base.PlayOneShot (data);
	}

	public void  PlayOneShot(int index, float afterSec) {
		StartCoroutine(PlayOneShotCo(index, afterSec));
	}

}
