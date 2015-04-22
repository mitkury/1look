using UnityEngine;
using System.Collections;

public class PopcornMaker : Interaction {

	public ObtainableItem popcorn;

	IEnumerator MakePopcornCo(InteractiveThing thing) {
		LeanTween.move(gameObject, thing.transform.position, 2f).setEase(LeanTweenType.easeOutSine);
		
		yield return new WaitForSeconds(2f);
		
		popcorn.gameObject.SetActive(true);

		popcorn.GetComponent<Rigidbody>().AddForce(Vector3.back * 0.25f, ForceMode.Impulse);

		gameObject.SetActive(false);
	}

	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		return thing.name == "Fireplace";
	}

	public override void InteractWith (InteractiveThing thing) {
		StartCoroutine(MakePopcornCo(thing));
	}

}
