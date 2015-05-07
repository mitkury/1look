using UnityEngine;
using System.Collections;

public class BeatingHeart : MonoBehaviour {

	PlaysSoundOnRequest soundPlayer;
	Vector3 initScale;

	public float betweenBeatShort = 0.2f;
	public float betweenBeatLong = 0.5f;

	void Start() {
		initScale = transform.localScale;
		soundPlayer = GetComponent<PlaysSoundOnRequest>();
		StartCoroutine(BeatCo());
	}

	IEnumerator BeatCo() {
		while(true) {
			//soundPlayer.PlayOneShot(0);

			LeanTween.scale(gameObject, initScale * 0.9f, betweenBeatShort).setEase(LeanTweenType.easeInCirc);

			yield return new WaitForSeconds(betweenBeatShort);

			soundPlayer.PlayOneShot(0);

			LeanTween.scale(gameObject, initScale, betweenBeatShort).setEase(LeanTweenType.easeOutCirc);

			yield return new WaitForSeconds(betweenBeatShort);

			LeanTween.scale(gameObject, initScale * 0.9f, betweenBeatShort).setEase(LeanTweenType.easeInCirc);

			yield return new WaitForSeconds(betweenBeatShort);

			LeanTween.scale(gameObject, initScale, betweenBeatShort).setEase(LeanTweenType.easeOutCirc);

			yield return new WaitForSeconds(betweenBeatLong);

        }
	}

}

