using UnityEngine;
using System.Collections;

public class PlaysSoundAfterSec : SoundInteraction {

	public float timer = 1f;
	public AudioClipData sound;

	void Start() {
		StartCoroutine(PlayOneShotAfterSecCo(sound, timer));
	}

	IEnumerator PlayOneShotAfterSecCo(AudioClipData data, float seconds) {
		yield return new WaitForSeconds(seconds);
		PlayOneShot(data);
	}

}
