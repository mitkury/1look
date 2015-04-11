using UnityEngine;
using System.Collections;

public class CircleAnimator : MonoBehaviour {
	
	Circle circle;

	public float animationTimeSec = 1.5f;

	void Start () {
		circle = GetComponent<Circle>();

		StartCoroutine(AnimateCo());
	}

	IEnumerator AnimateCo() {
		circle.completenes = 0f;
		float timePassed = 0f;

		while (timePassed <= animationTimeSec) {
			timePassed += Time.deltaTime;
			var step = timePassed / animationTimeSec;
			circle.completenes = Tweenings.easeInOutBack(0f, 1f, step);
			yield return null;
		}
	}
}
