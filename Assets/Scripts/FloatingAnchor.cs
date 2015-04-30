﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatingAnimationData {
	public float duration = 8f;
	public float elevation = 1f;
	public bool isRotating;
}

public class FloatingAnchor : MonoBehaviour {

	Animator animator;
	Vector3 initPosition;
	Vector3 elevatedPosition;

	public FloatingAnimationData data;

	void Start() {
		if (data != null)
			Init();
	}

	IEnumerator AnimateManuallyCo() {
		while(true) {
			LeanTween.move(gameObject, elevatedPosition, data.duration * 0.4f).setEase(LeanTweenType.easeInOutSine);
			yield return new WaitForSeconds(data.duration * 0.4f);
			LeanTween.move(gameObject, initPosition, data.duration * 0.6f).setEase(LeanTweenType.easeInOutSine);
			yield return new WaitForSeconds(data.duration * 0.6f);
		}
	}

	IEnumerator AnimateRotationManuallyCo() {
		while(true) {
			LeanTween.rotateAround(gameObject, transform.forward, 360f, data.duration * 2);
			yield return new WaitForSeconds(data.duration * 2);
		}
	}

	public void Init() {
		animator = GetComponent<Animator>();
		initPosition = transform.position;
		elevatedPosition = initPosition + Vector3.up * data.elevation;
		
		if (animator == null) {
			StartCoroutine(AnimateManuallyCo());
			if (data.isRotating)
				StartCoroutine(AnimateRotationManuallyCo());
		}
	}


}
