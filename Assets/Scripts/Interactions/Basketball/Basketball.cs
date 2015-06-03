using UnityEngine;
using System.Collections;

public class Basketball : Interaction {

	bool dropIsInitiated;
	float maxDist = 0.25f;
	float maxVelocity = 1f;
	Vector3 targetPosition = Vector3.zero;

	public float throwForce = 5;
	public float unblockAfterThrowInSec = 3f;

	IEnumerator UnblockCo() {
		yield return new WaitForSeconds(unblockAfterThrowInSec);

		var obtainableItem = GetComponent<ObtainableItem>();
		obtainableItem.isAbleToInteract = true;

		if (dropIsInitiated)
			yield break;
		
		if (King.visitor.itemInHand != null) {
			Drop();
		} else { 
			King.visitor.Take(obtainableItem);
		}
	}

	IEnumerator DropCo(Vector3 visiblePoint) {
		if (King.visitor.itemInHand == GetComponent<ObtainableItem>())
			King.visitor.Drop(GetComponent<ObtainableItem>());
		
		var _rigidbody = GetComponent<Rigidbody>();
		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = true;
		dropIsInitiated = true;

		yield return new WaitForSeconds(2f);

		// Animation
		if (dropIsInitiated) {
			_rigidbody.isKinematic = true;

			LeanTween.move(gameObject, visiblePoint, 3f).setEase(LeanTweenType.easeInOutSine).setOnComplete(delegate() {
				_rigidbody.isKinematic = false;
			});
		}

		// Physically based.
		/*
		if (dropIsInitiated) {
			targetPosition = visiblePoint;
		}

		var timeCount = 8f;
		while (timeCount > 0f) {
			yield return null;
			timeCount -= Time.deltaTime;
			if (!dropIsInitiated) {
				yield break;
			}

		}

		// If after that time the ball is not in that place—teleport it.
		if (Vector3.Distance(transform.position, visiblePoint) > maxDist)
			transform.position = visiblePoint;
		*/
	}
	
	void FixedUpdate () {
		var _rigidbody = GetComponent<Rigidbody>();

		if (targetPosition == Vector3.zero)
			return;

		if (Vector3.Distance(targetPosition, transform.position) > maxDist)
			_rigidbody.AddForce((targetPosition - transform.position).normalized * (Time.fixedTime * 0.1f));
		else {
			_rigidbody.velocity = _rigidbody.velocity * 0.5f;
			//targetPosition = Vector3.zero;
		}
	}

	IEnumerator FadeOutMusicAfterFreeingCo() {
		yield return new WaitForSeconds(unblockAfterThrowInSec + 0.5f);
		if (King.visitor.itemInHand != GetComponent<ObtainableItem>()) {
			if (GetComponent<ChangesSoundPitchOnRequest>() != null) {
				GetComponent<ChangesSoundPitchOnRequest>().FadeOut(1.5f);
			}
		}
	}

	public void OnItemTakeByVisitor(ObtainableItem item) {
		dropIsInitiated = false;
		targetPosition = Vector3.zero;

		if (GetComponent<ChangesSoundPitchOnRequest>() != null) {
			GetComponent<ChangesSoundPitchOnRequest>().FadeIn(1.5f);
		}
	}

	public void OnItemFreeByVisitor(ObtainableItem item) {
		StartCoroutine(FadeOutMusicAfterFreeingCo());
	}

	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		return thing.name == "ThrowingSurface";
	}

	public override void Interact () {
		Drop();
	}

	public override void InteractWith(InteractiveThing thing) {

		var _rigidbody = GetComponent<Rigidbody>();

		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = true;

		var force = throwForce;
		
		if (thing.GetComponent<ForceOverrider> ()) {
			force = thing.GetComponent<ForceOverrider>().newForce;
		}

		_rigidbody.AddForce((King.visitor.sight.facingVector + Vector3.up * 1.5f) * force, ForceMode.Impulse);

		StartCoroutine(UnblockCo());
	}

	public void Drop(Vector3 visiblePoint) {
		StartCoroutine(DropCo(visiblePoint));
	}

	public void Drop() {
		// Gosh, so many dependencies.
		Drop(GetComponent<SummonsBasketballHoop>().paintingHoop.visiblePointForBall.position);
	}

}
