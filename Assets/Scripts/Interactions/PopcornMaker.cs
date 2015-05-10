using UnityEngine;
using System.Collections;

public class PopcornMaker : Interaction {

	PlaysSoundOnRequest soundPlayer;
	Vector3 initScale;

	public ObtainableItem[] popcorn;
	public GameObject cornBody;

	void Start() {
		soundPlayer = GetComponent<PlaysSoundOnRequest>();
		initScale = transform.localScale;
	}

	IEnumerator MakePopcornCo(InteractiveThing thing) {
		LeanTween.move(gameObject, thing.transform.position, 2f).setEase(LeanTweenType.easeOutSine);
		
		yield return new WaitForSeconds(3f);

		var currentScale = initScale;

		for (int i = 0; i < popcorn.Length; i++) {
			var pop = popcorn[i];
			pop.gameObject.SetActive(true);

			var prevScale = currentScale;
			currentScale = currentScale * 0.9f;

			soundPlayer.PlayOneShot(i);

			LeanTween.scale(cornBody, currentScale, 0.1f).setEase(LeanTweenType.easeInCirc);
			yield return new WaitForSeconds (0.1f);

			var randomForce = Random.Range(0.1f, 0.2f);
			
			pop.GetComponent<Rigidbody>().useGravity = true;
			pop.GetComponent<Rigidbody>().isKinematic = false;
			pop.GetComponent<Rigidbody>().AddForce(Vector3.back * randomForce, ForceMode.Impulse);

			LeanTween.scale(cornBody, prevScale, 0.1f).setEase(LeanTweenType.easeInOutCirc);
			yield return new WaitForSeconds (0.25f);
		}
		/*
		popcorn.gameObject.SetActive(true);

		popcorn.GetComponent<Rigidbody>().useGravity = true;
		popcorn.GetComponent<Rigidbody>().isKinematic = false;
		popcorn.GetComponent<Rigidbody>().AddForce(Vector3.back * 0.25f, ForceMode.Impulse);
		*/

		LeanTween.scale(cornBody, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInCirc);
		yield return new WaitForSeconds (0.2f);

		gameObject.SetActive(false);
	}

	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		return thing.name == "Fireplace";
	}

	public override void InteractWith (InteractiveThing thing) {
		StartCoroutine(MakePopcornCo(thing));
	}

}
