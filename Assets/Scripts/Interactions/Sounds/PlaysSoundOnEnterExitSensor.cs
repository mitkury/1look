using UnityEngine;
using System.Collections;

public class PlaysSoundOnEnterExitSensor : SoundInteraction {

	public AudioClipData audioOnEnter;
	public AudioClipData audioOnExit;

	public void OnSensorEnter() {
		PlayOneShot(audioOnEnter);
	}

	public void OnSensorExit() {
		PlayOneShot(audioOnExit);
	}

}
