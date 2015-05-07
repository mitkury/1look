using UnityEngine;
using System.Collections;

public class Telescope : Interaction {

	bool isZoomedIn;

	public Animator mountainsAnimator;
	public float durationOfLooking = 10f;

	IEnumerator InteractCo() {
		GetComponent<Animator>().SetTrigger("ZoomIn");
		yield return new WaitForSeconds(durationOfLooking);
		GetComponent<Animator>().SetTrigger("ZoomOut");
		yield return new WaitForSeconds(5f);
		GetComponent<InteractiveObject>().isAbleToInteract = true;
	}

	public override void Interact () {
		StartCoroutine(InteractCo());
	}

	public void SwitchZoom() {
		if (isZoomedIn)
			ZoomOut();
		else 
			ZoomIn();

		isZoomedIn = !isZoomedIn;
	}

	public void ZoomIn() {
		mountainsAnimator.SetTrigger("ZoomIn");
	}

	public void ZoomOut() {
		mountainsAnimator.SetTrigger("ZoomOut");
	}

}
