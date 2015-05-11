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

public class SoundInteraction : Interaction {

	bool isSetup;
	AudioSource _audioSource;

	public void SetupAudioSource(AudioClipData data) {
		if (data == null)
			return;

		isSetup = true;
		
		if (data.audioClip != null) {
			_audioSource = GetComponent<AudioSource>() != null ? GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
			_audioSource.clip = data.audioClip;
			_audioSource.volume = data.volume;
			_audioSource.spatialBlend = data.spatialBlend;
		}
	}

	public virtual void PlayOneShot(AudioClipData data) {
		SetupAudioSource(data);
		if (_audioSource != null) {
			_audioSource.pitch = Random.Range(data.lowPitchRange, data.highPitchRange);
			_audioSource.PlayOneShot(data.audioClip, data.volume);
		}
	}

	public virtual void PlayOneShot(AudioClip clip, float volume = 1f) {
		if (_audioSource == null) {
			_audioSource = GetComponent<AudioSource>() != null ? GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
		}

		_audioSource.PlayOneShot(clip, volume);
	}

}
