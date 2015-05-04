using UnityEngine;
using System.Collections;

// A crude, hacked version of the radio component.
public class RadioPrototype : MonoBehaviour {
	
	Animator _animator;
	Floater floater;
	Visitor visitor;
	PlaysSoundOnRequest voicePlayer;

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
		float totalSec = 70f;
		float sub = 0;

		sub += 3f;
		yield return new WaitForSeconds(3f);
		voicePlayer.PlayOneShot(0);
		music.volume = 0.1f;
		yield return new WaitForSeconds(totalSec - sub);
		music.volume = 1f;
	}

	IEnumerator HideReticleForSec(float time) {
		yield return null;
		King.visitor.sight.reticle.gameObject.SetActive(false);
		yield return new WaitForSeconds(time);
		King.visitor.sight.reticle.gameObject.SetActive(true);
	}

}
