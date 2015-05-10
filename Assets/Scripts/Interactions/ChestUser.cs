using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestUser : Interaction {
	
	public Transform dropPoint;
	[HideInInspector]
	public List<ObtainableItem> items = new List<ObtainableItem>();
	
	IEnumerator LootChestCo() {
		if (items.Count == 0)
			yield break;

		foreach(ObtainableItem item in items) {
			LeanTween.move(item.gameObject, item.transform.position + Vector3.up, 1f).setEase(LeanTweenType.easeOutCubic);;
			yield return new WaitForSeconds(1f);
			LeanTween.move(item.gameObject, dropPoint.position + Vector3.up, 1f).setEase(LeanTweenType.easeOutCubic);;
			yield return new WaitForSeconds(1f);
			item.GetComponent<Rigidbody>().isKinematic = false;
			item.GetComponent<Rigidbody>().useGravity = true;
		}
	}

	public override void Interact () {

		StartCoroutine(LootChestCo());

	}

	public void Add(InteractiveThing thing) {
		if (!(thing is ObtainableItem))
			return;

		items.Add(thing as ObtainableItem);
	}

}
