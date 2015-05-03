using UnityEngine;
using System.Collections;

public class PlaysSoundOnRequest : SoundInteraction {

	public AudioClipData[] sounds;

	IEnumerator PlayOneShotAfterSecCo(int index, float seconds) {
		yield return new WaitForSeconds(seconds);
		PlayOneShot(index);
	}

	public void PlayOneShot(int index) {
		var data = sounds[index];
		base.PlayOneShot(data);
	}
	
	public void  PlayOneShotAfterSec(int index, float seconds) {
		StartCoroutine(PlayOneShotAfterSecCo(index, seconds));
	}

}
