using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	public bool isPlaying { get; private set; }
	public GameObject[] letters;

	IEnumerator ActivateCo() {
		isPlaying = true;

		/*
		LeanTween.value(gameObject, delegate(float value) { 
			AudioListener.volume = value;
		}, 0f, 1f, 1f);

		yield return new WaitForSeconds(1f);
		*/

		/*
		foreach (GameObject letter in letters) {
			LeanTween.alpha(letter, 0f, 0.1f);
		}
		*/

		yield return new WaitForSeconds(1f);

		/*
		foreach (GameObject letter in letters) {
			LeanTween.alpha(letter, 1f, 2f);
		}
	 	*/

		AudioListener.volume = 1;
		GetComponent<PlaysSoundOnRequest>().PlayOneShot(0);

		yield return new WaitForSeconds(GetComponent<PlaysSoundOnRequest>().sounds[0].audioClip.length);

		isPlaying = false;
	}

	public void Activate() {
		StartCoroutine(ActivateCo());
	}

}
