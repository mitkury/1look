using UnityEngine;
using System.Collections;

public class Pulsation : MonoBehaviour {

	Vector3 initScale;

	public float rate = 0.2f;
	public Vector3 targetScale;

	void Start () {
		initScale = transform.localScale;
	}

	void OnEnable() {
		StartCoroutine(PulsationCo());
	}

	IEnumerator PulsationCo() {
		while(initScale == Vector3.zero)
			yield return null;

		while(true) {
			LeanTween.scale(gameObject, targetScale, rate).setEase(LeanTweenType.easeInSine);
			
			yield return new WaitForSeconds(rate);
			
			LeanTween.scale(gameObject, initScale, rate).setEase(LeanTweenType.easeOutSine);
			
			yield return new WaitForSeconds(rate);
		}
	}

}
