using UnityEngine;
using System.Collections;

// A crude, hacked version of the radio component.
public class RadioPrototype : SingletonComponent<RadioPrototype> {
	
	Animator _animator;
	Floater floater;
	Visitor visitor;
	PlaysSoundOnRequest voicePlayer;
	bool narrativeIsPlaying;

	public AudioSource music;

	public Cauldron cauldron;

	void Start() {
		_animator = GetComponent<Animator>();
		floater = GetComponent<Floater>();
		voicePlayer = GetComponent<PlaysSoundOnRequest>();

		StartCoroutine(StartAfterCo(3f));
	}

	IEnumerator StartAfterCo(float seconds) {
		StartCoroutine(HideReticleForSec(14.5f));
		yield return new WaitForSeconds(seconds);
		StartCoroutine(StartIntroAnimationCo());
		StartCoroutine(StartIntroCo());
	}

	IEnumerator StartIntroAnimationCo() {
		floater.enabled = false;

		var stateNameHash = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
		var stateHash = Animator.StringToHash("Start");

		_animator.Play(stateHash);

		// Wait a frame for a new state to setup.
		yield return null;

		// While state is playing.
		while(stateHash == _animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
			yield return null;

		_animator.enabled = false;

		floater.enabled = true;
	}

	IEnumerator StartIntroCo() {
		float totalSec = 60f;
		float sub = 0;

		narrativeIsPlaying = true;

		sub += 3f;
		yield return new WaitForSeconds(3f);
		FadeMusicTo(0.1f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		voicePlayer.PlayOneShot(0);
		yield return new WaitForSeconds(totalSec - sub);
		FadeMusicTo(1f, 0.5f);

		narrativeIsPlaying = false;
	}

	IEnumerator HideReticleForSec(float time) {
		yield return null;
		King.visitor.sight.enabled = false;
		yield return new WaitForSeconds(time);
		King.visitor.sight.enabled = true;
	}

	void FadeMusicTo(float volume, float time) {
		LeanTween.value(gameObject, delegate(float value) { 
			music.volume = value;
		}, music.volume, volume, time);	
	}

	IEnumerator PlayRemarkCo(AudioClip clip, float volume = 1) {
		if (narrativeIsPlaying)
			yield break;

		FadeMusicTo(0.1f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		Instance.voicePlayer.PlayOneShot(clip, volume);
		yield return new WaitForSeconds(clip.length);
		FadeMusicTo(1f, 0.5f);
	}

	public static void PlayRemark(AudioClip clip, float volume = 1) {
		Instance.StartCoroutine(Instance.PlayRemarkCo(clip, volume));
	}

}
