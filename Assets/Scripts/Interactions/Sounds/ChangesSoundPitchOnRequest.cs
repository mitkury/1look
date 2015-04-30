using UnityEngine;
using System.Collections;

public class ChangesSoundPitchOnRequest : Interaction {

	public AudioSource audioSourceReceiver;
	public float max = 1f;
	public float min = 0.5f;

    void UpdateAudioValue(float value){
		audioSourceReceiver.pitch = value;
	}
	
	public void FadeIn(float time) {
		LeanTween.cancel(gameObject);
		LeanTween.value(gameObject, UpdateAudioValue, audioSourceReceiver.pitch, max, time);
	}

	public void FadeOut(float time) {
		LeanTween.cancel(gameObject);
		LeanTween.value(gameObject, UpdateAudioValue, audioSourceReceiver.pitch, min, time);
	}
}
