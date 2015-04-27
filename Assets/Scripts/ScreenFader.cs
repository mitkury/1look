using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour {

	[HideInInspector]
	public Transform anchor;
	public Transform blinkPlane;
	public float nearPlane = 0.15f;

	void Start() {
		var color = blinkPlane.GetComponent<MeshRenderer>().material.color;
		blinkPlane.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 0f);
		blinkPlane.gameObject.SetActive(false);
	}

	void Update () {
		// At the start an anchor in OVRCameraRig always returns infinity for a few frames.
		if (anchor == null || anchor.position.sqrMagnitude == Mathf.Infinity)
			return;
		
		blinkPlane.rotation = Quaternion.LookRotation(blinkPlane.position - anchor.position);
		blinkPlane.position = anchor.position + anchor.forward * nearPlane;
	}

	/*
	IEnumerator FadeToInSecCo(float alpha, float seconds) {
		yield break;
	}
	*/

	public void FadeToInSec(float alpha, float seconds) {
		//StartCoroutine(FadeToInSecCo(alpha, seconds));

		blinkPlane.gameObject.SetActive(true);
		
		LeanTween.cancel(blinkPlane.gameObject);
		LeanTween.alpha(blinkPlane.gameObject, alpha, seconds).setOnComplete(delegate() {
			if (alpha <= 0) {
				blinkPlane.gameObject.SetActive(false);
			}
		});
	}

	public void FadeIn(float seconds) {
		FadeToInSec(1f, seconds);
	}

	public void FadeOut(float seconds) {
		FadeToInSec(0f, seconds);
	}

}
