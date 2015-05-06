using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotionBook : MonoBehaviour {

	Visitor visitor;
	bool bookIsShown;
	bool isInitiatedShowing;
	bool isInitiatedHiding;
	public List<RecipeIcon> recipeIcons = new List<RecipeIcon>();

	public float showBookAfterSec = 1f;
	public float hideBookAfterSec = 2f;
	public float showAnimationDuration = 2f;
	public float hideAnimationDuration = 2f;
	public Transform bookReadPoint;
	public Transform bookHidePoint;

	public void OnAddToCauldron(RecipeItem item) {
		Debug.Log(item.itemName);
		var matchingIcon = recipeIcons.Find(i => i.itemName == item.itemName);

		if (matchingIcon != null)
			matchingIcon.Hide();
	}

	void OnEnable() {
		transform.position = bookHidePoint.position;
		transform.rotation = bookHidePoint.rotation;

		StartCoroutine(FindVisitorCo());
	}

	void Update() {
		if (visitor == null)
			return;

		/*
		var targetPosition = bookHidePoint.position;
		var targetRotation = bookHidePoint.rotation;
		
		if (user.isLookingAtBookReadPosition) {
			targetPosition = bookReadPoint.position;
			targetRotation = bookReadPoint.rotation;
		}
		
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
		*/

		if (visitor.isLookingAtBookReadPosition) {
			// Initiate showing the book if it's not yet shown.
			if (!isInitiatedShowing && !bookIsShown) {
				// If the book is in process of hiding—show it immediately.
				if (isInitiatedHiding)
					Show();
				else
					StartCoroutine(InitiateShowCo());
			}
		} 
		else {
			// Initiate hiding the book if it's shown.
			if (!isInitiatedHiding && bookIsShown)
				StartCoroutine(InitiateHideCo());
		}
	}

	IEnumerator InitiateShowCo() {
		isInitiatedShowing = true;
		yield return new WaitForSeconds(showBookAfterSec);
		if (visitor.isLookingAtBookReadPosition) {
			Show();
		}
		isInitiatedShowing = false;
	}

	IEnumerator InitiateHideCo() {
		isInitiatedHiding = true;
		yield return new WaitForSeconds(hideBookAfterSec);
		if (!visitor.isLookingAtBookReadPosition) {
			Hide();
		}
		isInitiatedHiding = false;
	}

	void Show() {
		bookIsShown = true;
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, bookReadPoint.position, showAnimationDuration).setEase(LeanTweenType.easeInOutCubic);
		LeanTween.rotate(gameObject, bookReadPoint.rotation.eulerAngles, showAnimationDuration).setEase(LeanTweenType.easeInOutCubic);
	}

	void Hide() {
		bookIsShown = false;
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, bookHidePoint.position, hideAnimationDuration).setEase(LeanTweenType.easeInOutCubic);
		LeanTween.rotate(gameObject, bookHidePoint.rotation.eulerAngles, hideAnimationDuration).setEase(LeanTweenType.easeInOutCubic);
	}

	IEnumerator FindVisitorCo() {
		if (King.visitor == null)
			yield return null;

		visitor = King.visitor;
	}

}
