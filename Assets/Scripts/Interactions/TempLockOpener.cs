﻿using UnityEngine;
using System.Collections;

public class TempLockOpener : Interaction {

	public int lockID = 1;

	IEnumerator OpenLockCo(InteractiveThing thing) {

		var tempLock = thing.GetComponent<TempLock>();

		// Rotate the lock toward the key.
		var lockLookRotation = Quaternion.LookRotation(transform.position - tempLock.transform.position);
		LeanTween.rotate(tempLock.gameObject, lockLookRotation.eulerAngles, 0.5f);

		yield return new WaitForSeconds(0.5f);

		// Move the key towards the lock.
		LeanTween.move(gameObject, tempLock.keyOutsidePoint.position, 2f).setEase(LeanTweenType.easeOutSine);
		LeanTween.rotate(gameObject, tempLock.keyOutsidePoint.rotation.eulerAngles, 1f).setEase(LeanTweenType.easeOutSine);

		yield return new WaitForSeconds(2f + 0.25f);

		// Move the key inside the lock.
		LeanTween.move(gameObject, tempLock.keyInsidePoint.position, 0.25f).setEase(LeanTweenType.easeOutSine);

		yield return new WaitForSeconds(0.25f);

		// Rotate the key in the lock.
		LeanTween.rotate(gameObject, tempLock.keyInsidePoint.eulerAngles, 0.5f);

		yield return new WaitForSeconds(0.5f);

		tempLock.receiver.Interact();

		transform.parent = tempLock.transform;

		LeanTween.move(tempLock.gameObject, tempLock.transform.position + tempLock.transform.forward * 0.25f, 1f).setEase(LeanTweenType.easeOutSine);

		yield return new WaitForSeconds(1f);

		GetComponent<Rigidbody>().detectCollisions = false;
		tempLock.GetComponent<Rigidbody>().isKinematic = false;
		tempLock.GetComponent<InteractiveThing>().isAbleToInteract = false;

	}

	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		//if (HName.GetPure(thing.name) != "lock")
		if (thing.name != "Lock")
			return false;

		var targetLock = thing.interactions.Find(it => it is TempLock) as TempLock;

		if (targetLock != null && targetLock.lockID == lockID)
			return true;
		else
			return false;

		//return HName.GetPure(thing.name) == "lock" && (thing as ObtainableItem).;
	}

	public override void InteractWith (InteractiveThing thing) {
		if (!IsAbleToInteractWith(thing))
		    return;

		StartCoroutine(OpenLockCo(thing));
	}
}
