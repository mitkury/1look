using UnityEngine;
using System.Collections;

public class CandyCorn : Interaction {

	public float eatAfterSec = 2f;
	public AudioClipData eatingAudio;
	public AudioClipData[] mmmAudio;

	public void OnItemTakeByVisitor(ObtainableItem item) {
		StartCoroutine(EatCo());
	}

	IEnumerator EatCo() {
		yield return new WaitForSeconds(eatAfterSec);

		Vector3 initScale = transform.localScale;

		King.visitor.earsAnchor.GetComponent<PlaysSoundOnRequest>().PlayOneShot(eatingAudio);

		yield return new WaitForSeconds(0.5f);
		LeanTween.scale(gameObject, initScale * 0.66f, 0.25f).setEase(LeanTweenType.easeOutSine);
		yield return new WaitForSeconds(4f);
		LeanTween.scale(gameObject, initScale * 0.33f, 0.25f).setEase(LeanTweenType.easeOutSine);
		yield return new WaitForSeconds(4f);
		LeanTween.scale(gameObject, initScale * 0f, 0.25f).setEase(LeanTweenType.easeOutSine);
		yield return new WaitForSeconds(2f);

		King.visitor.earsAnchor.GetComponent<PlaysSoundOnRequest>().PlayOneShot(mmmAudio[0]);

		yield return new WaitForSeconds(1f);

		King.visitor.Drop(GetComponent<ObtainableItem>());
		gameObject.SetActive(false);
	}

}
