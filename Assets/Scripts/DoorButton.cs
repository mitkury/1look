using UnityEngine;
using System.Collections;

public class DoorButton : MonoBehaviour {

	Color initColor;

	public MeshRenderer plateMeshRenderer;
	public Color activatedColor;
	public Circle icon;
	
	void Start () {
		initColor = plateMeshRenderer.material.color;
	}

	IEnumerator ActivateCo() {
		LeanTween.color(plateMeshRenderer.gameObject, activatedColor, 0.5f);

		yield return new WaitForSeconds(0.5f);

		/*
		var animationTimeSec = 3f;
		var timePassed = 0f;

		while (timePassed <= animationTimeSec) {
			timePassed += Time.deltaTime;
			var step = timePassed / animationTimeSec;
			icon.completenes = Tweenings.easeInOutBack(0f, 1f, step);
			yield return null;
		}
		*/
	}

	public void Activate() {
		StartCoroutine(ActivateCo());
	}

}
