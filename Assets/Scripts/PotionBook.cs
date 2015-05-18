using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotionBook : MonoBehaviour {

	Visitor visitor;
	bool bookIsShown;
	PlaysSoundOnRequest audioPlayer;
	bool isInitiatedShowing;
	bool isInitiatedHiding;
	int forcedShowNum = 0;

	public List<RecipeIcon> recipeIcons = new List<RecipeIcon>();
	public float showBookAfterSec = 1f;
	public float hideBookAfterSec = 2f;
	public float showAnimationDuration = 2f;
	public float hideAnimationDuration = 2f;
	public Transform bookReadPoint;
	public Transform bookHidePoint;

	void Start() {
		audioPlayer = GetComponent<PlaysSoundOnRequest>();

		foreach(RecipeIcon icon in recipeIcons) {
			icon.GetComponent<InteractiveObject>().isAbleToInteract = false;
		}
	}

	void OnEnable() {
		transform.position = bookHidePoint.position;
		transform.rotation = bookHidePoint.rotation;

		StartCoroutine(FindVisitorCo());
	}

	void Update() {
		if (visitor == null)
			return;

		if (forcedShowNum > 0)
			return;

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

	IEnumerator ShowOnRecipeCo(RecipeIcon icon) {
		forcedShowNum += 1;
		Show();

		yield return new WaitForSeconds(showAnimationDuration);

		icon.Hide();

		var duration = 4f;

		while(duration > 0) {
			yield return null;
			duration -= Time.deltaTime;

			// If there is another, more recent coroutines—remove the current one.
			if (forcedShowNum > 1) {
				forcedShowNum -= 1;
				yield break;
			}
		}

		forcedShowNum -= 1;

		if (!visitor.isLookingAtBookReadPosition)
			Hide();
	}

	IEnumerator FindVisitorCo() {
		if (King.visitor == null)
			yield return null;
		
		visitor = King.visitor;
	}

	IEnumerator ShowCo() {
		bookIsShown = true;
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, bookReadPoint.position, showAnimationDuration).setEase(LeanTweenType.easeInOutCubic);
		LeanTween.rotate(gameObject, bookReadPoint.rotation.eulerAngles, showAnimationDuration).setEase(LeanTweenType.easeInOutCubic);
		
		if (audioPlayer != null)
			audioPlayer.PlayOneShot(0);

		yield return new WaitForSeconds(showAnimationDuration);

		if (!bookIsShown)
			yield break;

		foreach(RecipeIcon icon in recipeIcons) {
			icon.GetComponent<InteractiveObject>().isAbleToInteract = true;
		}
	}

	void Show() {
		StartCoroutine(ShowCo());
	}

	void Hide() {
		bookIsShown = false;
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, bookHidePoint.position, hideAnimationDuration).setEase(LeanTweenType.easeInOutCubic);
		LeanTween.rotate(gameObject, bookHidePoint.rotation.eulerAngles, hideAnimationDuration).setEase(LeanTweenType.easeInOutCubic);

		if (audioPlayer != null)
			audioPlayer.PlayOneShot(1);

		foreach(RecipeIcon icon in recipeIcons) {
			icon.GetComponent<InteractiveObject>().isAbleToInteract = false;
		}
	}

	public void OnAddToCauldron(RecipeItem item) {
		var matchingIcon = recipeIcons.Find(i => i.itemName == item.itemName);
		
		if (matchingIcon != null) {
			StartCoroutine(ShowOnRecipeCo(matchingIcon));
		}
	}

}
