using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioClipData {
	public AudioClip audioClip;
	public float volume = 1f;
	public float lowPitchRange = 1f;
	public float highPitchRange = 1f;
	public float spatialBlend = 1f;
}

public class PlaysSoundOnInteraction : Interaction {

	bool isSetup;
	AudioSource _audioSource;

	public AudioClipData audioClipData;

	void Start() {
		if (!isSetup) {
			Setup(audioClipData);
		}
	}

	public void Setup(AudioClipData data) {
		isSetup = true;
		
		audioClipData = data;
		if (audioClipData.audioClip != null) {
			_audioSource = GetComponent<AudioSource>() != null ? GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
			_audioSource.clip = audioClipData.audioClip;
			_audioSource.volume = audioClipData.volume;
			_audioSource.spatialBlend = audioClipData.spatialBlend;
		}
	}

	public override void Interact ()
	{
		Debug.Log(_audioSource);
		if (_audioSource != null) {
			_audioSource.pitch = Random.Range(audioClipData.lowPitchRange, audioClipData.highPitchRange);
			//_audioSource.Play();
			_audioSource.PlayOneShot(audioClipData.audioClip, audioClipData.volume);
		}
	}
}
