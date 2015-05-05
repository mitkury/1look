using UnityEngine;
using System.Collections;

public class StrokesCat : Interaction {

	PlaysSoundOnRequest soundPlayer;

	void Start() {
		soundPlayer = GetComponent<PlaysSoundOnRequest>();
	}

	IEnumerator StrokeCo() {
		yield return null;
		GetComponent<InteractiveObject>().isAbleToInteract = true;
		Meow();
	}

	void Meow() {
		var soundIndex = Random.Range(0, soundPlayer.sounds.Length);
		soundPlayer.PlayOneShot(soundIndex);
	}

	public override void Interact () {
		StartCoroutine(StrokeCo());
	}

}
