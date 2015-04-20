using UnityEngine;
using System.Collections;

public class TempLockOpener : Interaction {

	IEnumerator OpenLockCo(InteractiveThing thing) {

		var tempLock = thing.GetComponent<TempLock>();

		LeanTween.move(gameObject, tempLock.keyOutsidePoint.position, 2f).setEase(LeanTweenType.easeOutSine);
		LeanTween.rotate(gameObject, tempLock.keyOutsidePoint.rotation.eulerAngles, 1f).setEase(LeanTweenType.easeOutSine);

		yield return new WaitForSeconds(2f + 0.25f);

		LeanTween.move(gameObject, tempLock.keyInsidePoint.position, 0.25f).setEase(LeanTweenType.easeOutSine);

		yield return new WaitForSeconds(0.25f);

		LeanTween.rotate(gameObject, tempLock.keyInsidePoint.eulerAngles, 0.25f);

		tempLock.doorOpener.Interact();

		transform.parent = tempLock.transform;

		LeanTween.move(tempLock.gameObject, tempLock.transform.position + -tempLock.transform.forward * 0.25f, 0.5f).setEase(LeanTweenType.easeOutSine);

		yield return new WaitForSeconds(0.25f);

		GetComponent<Rigidbody>().detectCollisions = false;
		tempLock.GetComponent<Rigidbody>().isKinematic = false;

	}

	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		return HName.GetPure(thing.name) == "lock";
	}

	public override void InteractWith (InteractiveThing thing) {
		if (!IsAbleToInteractWith(thing))
		    return;

		StartCoroutine(OpenLockCo(thing));
	}
}
