using UnityEngine;
using System.Collections;

public class FullGameStarter : MonoBehaviour {

	public GameObject logo;

	void Start() {
		StartCoroutine(InitCo());
	}

	IEnumerator InitCo() {
		if (King.visitor.sight == null)
			yield return null;

		King.visitor.sight.enabled = false;
	}

	IEnumerator ActivatePlaceCo() {

		float blinkInSec = 0.25f;

		King.visitor.screenFader.FadeIn(blinkInSec);
		yield return new WaitForSeconds(blinkInSec);

		if (King.placeManager.currentPlace == null || !King.placeManager.currentPlace.gameObject.activeSelf)
			yield return null;

		King.visitor.SetBackground(King.placeManager.currentPlace.background);

		King.placeManager.currentPlace.gameObject.SetActive(true);
		logo.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		King.visitor.screenFader.FadeOut(blinkInSec);

		yield return new WaitForSeconds(blinkInSec + 0.5f);
		King.visitor.sight.enabled = true;

		Destroy(logo);
		Destroy(gameObject);

		yield break;
	}

	public void ActivatePlace() {
		StartCoroutine(ActivatePlaceCo());
	}

}
