using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class InventorySpot : InteractiveThing {

	Transform prevItemParent;
	BoxCollider _collider;

	public ObtainableItem item { get; private set; } 

	void Start() {
		_collider = GetComponent<BoxCollider>();
	}

	IEnumerator AddItemCo() {
		if (item == null)
			yield break;

		var itemRigidbody = item.GetComponent<Rigidbody>();
		if (itemRigidbody != null) {
			itemRigidbody.useGravity = false;
			itemRigidbody.isKinematic = true;
		}
		
		LeanTween.move(item.gameObject, transform.position + Vector3.up * 0.5f, 1f).setEase(LeanTweenType.easeOutCubic);
		LeanTween.rotate(item.gameObject, transform.rotation.eulerAngles, 1f).setEase(LeanTweenType.easeInCubic);
		
		yield return new WaitForSeconds(1f);

		var targetPosition = transform.position;

		RaycastHit hit;
		if (Physics.Raycast(item.transform.position, Vector3.down, out hit)) {
			var itemRigidobdy = item.GetComponent<Rigidbody>();
			Vector3 lowestPointOnTarget;
			
			if (itemRigidobdy != null) {
				lowestPointOnTarget = itemRigidobdy.ClosestPointOnBounds(item.transform.position + Vector3.down * 10);
				//waistAnchor.transform.position - (highestPointOnTarget - itemInHand.transform.position);
				targetPosition = hit.point - (lowestPointOnTarget - item.transform.position);
			}
		}

		LeanTween.move(item.gameObject, targetPosition, 0.5f);
	}

	IEnumerator RemoveItemCo() {

		isAbleToInteract = false;

		var targetPosition = transform.position + Vector3.up * 0.5f;
		LeanTween.move(item.gameObject, targetPosition, 1f).setEase(LeanTweenType.easeOutCubic);

		yield return new WaitForSeconds(1f);

		item.isAbleToInteract = true;
		item = null;

		yield return new WaitForSeconds(3f);
		// Activate the spot with delay after removing an item,
		// so the item wont get back to inventory if Player continues to point at the spot after getting the item in hand.
		isAbleToInteract = true;
	}

	public void Add(ObtainableItem item) {
		if (this.item != null) 
			return;

		this.item = item;
		prevItemParent = item.transform.parent;
		item.transform.parent = transform;
		transform.parent.SendMessage("RegisterItem", item);

		_collider.enabled = false;

		Debug.Log(this.item+" has been added to the inventory.");

		StartCoroutine(AddItemCo());
	}

	public void RemoveItem() {
		if (prevItemParent != null) {
			item.transform.parent = prevItemParent;
		} else {
			Debug.LogWarning("There's probably something fishy going on.");
			item.transform.parent = King.placeManager.currentPlace.transform;
		}

		_collider.enabled = true;

		Debug.Log(this.item+" has been removed from the inventory.");

		item.isAbleToInteract = false;

		StartCoroutine(RemoveItemCo());
	}

	public void OnItemTakeByVisitor() {
		RemoveItem();
	}

}
