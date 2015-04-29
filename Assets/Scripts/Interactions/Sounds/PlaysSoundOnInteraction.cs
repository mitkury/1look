using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioClipData {
	public AudioClip audioClip;
	public float volume = 1f;
	public float lowPitchRange = 1f;
	public float highPitchRange = 1f;
}

public class PlaysSoundOnInteraction : Interaction {

	AudioSource _audioSource;

	public AudioClipData audioClipData;

	void Start() {
		Setup(audioClipData);
	}

	public void Setup(AudioClipData data) {
		audioClipData = data;

		if (audioClipData.audioClip != null) {
			_audioSource = GetComponent<AudioSource>() != null ? GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
			Debug.Log(_audioSource);
			_audioSource.clip = audioClipData.audioClip;
			_audioSource.volume = audioClipData.volume;
		}
	}

	public override void Interact ()
	{
		if (_audioSource != null) {
			_audioSource.pitch = Random.Range(audioClipData.lowPitchRange, audioClipData.highPitchRange);
			_audioSource.Play();
		}
	}
}
