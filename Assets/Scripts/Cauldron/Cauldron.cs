using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Cauldron : InteractiveObject {
	
	PlaysSoundOnRequest audioPlayer;
	int ingredientsUsed;

	public Transform pointInside;
	public Transform pointOnSurface;
	public Transform pointForRejected;
	public Recipe recipe;
	public ObtainableItem potion;
	public List<GameObject> subscribers = new List<GameObject>();

	protected override void Init ()
	{
		base.Init ();

		audioPlayer = GetComponent<PlaysSoundOnRequest>();
	}

	IEnumerator AddItemCo(ObtainableItem item, RecipeItem recipeItem) {
		Debug.Log(item+" has been added to the cauldron.");
		ingredientsUsed += 1;
		recipeItem.collected += 1;

		var itemRigidbody = item.GetComponent<Rigidbody>();
		if (itemRigidbody != null) {
			itemRigidbody.useGravity = false;
			itemRigidbody.isKinematic = true;
		}
		
		LeanTween.move(item.gameObject, transform.position + Vector3.up * 1f, 1f).setEase(LeanTweenType.easeOutCubic);
		
		yield return new WaitForSeconds(1f);
		
		var targetPosition = pointInside.position;
		
		LeanTween.move(item.gameObject, targetPosition, 0.5f);

		audioPlayer.PlayOneShotAfterSec(0, 0.25f);

		if (ingredientsUsed == 1 && GetComponent<PlaysAudioRemarkOnRadio>() != null && !potionIsReady)
			GetComponent<PlaysAudioRemarkOnRadio>().Play(0);

		yield return new WaitForSeconds(0.5f);

		foreach (GameObject subscriber in subscribers) {
			subscriber.SendMessage("OnAddToCauldron", recipeItem, SendMessageOptions.DontRequireReceiver);
		}

		item.gameObject.SetActive(false);
		isAbleToInteract = true;


		if (potionIsReady) 
			GivePotion();
	}

	IEnumerator RejectItemCo(ObtainableItem item) {
		Debug.Log(item+" has been rejected by the cauldron.");

		var itemRigidbody = item.GetComponent<Rigidbody>();
		if (itemRigidbody != null) {
			itemRigidbody.useGravity = false;
			itemRigidbody.isKinematic = true;
		}
		
		LeanTween.move(item.gameObject, transform.position + Vector3.up * 1f, 1f).setEase(LeanTweenType.easeOutCubic);
		
		yield return new WaitForSeconds(1f);
		
		var targetPosition = pointOnSurface.position;
		
		LeanTween.move(item.gameObject, targetPosition, 0.5f);
		
		yield return new WaitForSeconds(0.5f);

		if (GetComponent<PlaysAudioRemarkOnRadio>()) {
			var maxIndex = GetComponent<PlaysAudioRemarkOnRadio>().remarks.Count;
			GetComponent<PlaysAudioRemarkOnRadio>().Play(Random.Range(2, maxIndex));
		}

		audioPlayer.PlayOneShot(1);

		foreach (GameObject subscriber in subscribers) {
			subscriber.SendMessage("OnRejectFromCauldron", item, SendMessageOptions.DontRequireReceiver);
		}

		LeanTween.move(item.gameObject, transform.position + Vector3.up * 1f, 1f).setEase(LeanTweenType.easeOutCubic);

		yield return new WaitForSeconds(1f);

		LeanTween.move(item.gameObject, pointForRejected.position, 1f).setEase(LeanTweenType.easeOutCubic);

		yield return new WaitForSeconds(1f);

		if (itemRigidbody != null) {
			itemRigidbody.useGravity = true;
			itemRigidbody.isKinematic = false;
		}

		item.isAbleToInteract = true;

		yield return new WaitForSeconds(1.5f);

		itemRigidbody.isKinematic = true;
	}

	bool potionIsReady {
		get {
			var isReady = true;

			foreach (RecipeItem recipeItem in recipe.items) {
				if (recipeItem.collected < recipeItem.amount) {
					isReady = false;
					break;
				}
			}

			return isReady;
		}
	}

	IEnumerator GivePotionCo() {
		if (GetComponent<PlaysAudioRemarkOnRadio>() != null)
			GetComponent<PlaysAudioRemarkOnRadio>().Play(1);

		yield return new WaitForSeconds(1f);
		potion.gameObject.SetActive(true);
		potion.transform.position = pointInside.position;
		LeanTween.move(potion.gameObject, transform.position + Vector3.up * 1f, 1f).setEase(LeanTweenType.easeOutCubic);
		yield return new WaitForSeconds(1f);

		if (King.visitor.itemInHand != null) {
			var item = King.visitor.itemInHand;
			King.visitor.Drop(item);
			item.GetComponent<Rigidbody>().isKinematic = false;
			item.GetComponent<Rigidbody>().useGravity = true;
		}

		King.visitor.Take(potion);
	}

	void GivePotion() {
		StartCoroutine(GivePotionCo());
	}

	public void Add(ObtainableItem item) {
		var recipeItem = recipe.items.Find(i => HName.GetPure(i.itemName) == HName.GetPure(item.name));

		if (recipeItem != null) {
			StartCoroutine(AddItemCo(item, recipeItem));
		} else {
			StartCoroutine(RejectItemCo(item));
		}
	}


}
