using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cauldron : InteractiveObject {

	public Transform pointInside;
	public Recipe recipe;
	public ObtainableItem potion;

	IEnumerator AddItemCo(ObtainableItem item, RecipeItem recipeItem) {
		Debug.Log(item+" has been added to the cauldron.");

		var itemRigidbody = item.GetComponent<Rigidbody>();
		if (itemRigidbody != null) {
			itemRigidbody.useGravity = false;
			itemRigidbody.isKinematic = true;
		}
		
		LeanTween.move(item.gameObject, transform.position + Vector3.up * 1f, 1f).setEase(LeanTweenType.easeOutCubic);
		
		yield return new WaitForSeconds(1f);
		
		var targetPosition = pointInside.position;
		
		LeanTween.move(item.gameObject, targetPosition, 0.5f);

		yield return new WaitForSeconds(0.5f);

		item.gameObject.SetActive(false);
		isAbleToInteract = true;
		recipeItem.collected += 1;
		CheckIfPoitionIsReady();
	}

	void CheckIfPoitionIsReady() {

		var isReady = true;

		foreach (RecipeItem recipeItem in recipe.items) {
			if (recipeItem.collected < recipeItem.amount) {
				isReady = false;
				break;
			}
		}

		if (isReady) {
			GivePotion();
		}
	}

	IEnumerator GivePotionCo() {
		potion.gameObject.SetActive(true);
		LeanTween.move(potion.gameObject, transform.position + Vector3.up * 1f, 1f).setEase(LeanTweenType.easeOutCubic);
		yield return new WaitForSeconds(1f);
		King.visitor.Take(potion);
	}

	void GivePotion() {
		StartCoroutine(GivePotionCo());
	}

	public void Add(ObtainableItem item) {
		var recipeItem = recipe.items.Find(i => i.itemName == item.name);

		if (recipeItem != null) {
			StartCoroutine(AddItemCo(item, recipeItem));
		}
	}


}
