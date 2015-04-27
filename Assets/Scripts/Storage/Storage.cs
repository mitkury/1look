using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Storage : MonoBehaviour {
	
	public SurfaceForItems surface;
	public float itemCheckDelaySec = 0.5f;
	public List<ObtainableItem> items { get; private set; }

	void OnEnable() {
		if (items == null)
			items = new List<ObtainableItem>();

		StartCoroutine(CheckItemsOnSurface());
	}
	
	IEnumerator AddItemCo(ObtainableItem item, Vector3 point) {
		//Debug.Log(item+" has been added to the cauldron.");
		item.transform.parent = transform;

		var itemRigidbody = item.GetComponent<Rigidbody>();
		itemRigidbody.useGravity = false;
		itemRigidbody.isKinematic = true;

		LeanTween.cancel(item.gameObject);
		LeanTween.move(item.gameObject, point + Vector3.up * 1f, 1f).setEase(LeanTweenType.easeOutCubic);
		LeanTween.rotate(item.gameObject, surface.transform.rotation.eulerAngles, 1f).setEase(LeanTweenType.easeInCubic);

		yield return new WaitForSeconds(1f);

		itemRigidbody.useGravity = true;
		itemRigidbody.isKinematic = false;

		yield return new WaitForSeconds(2f);

		items.Add(item);
		item.isAbleToInteract = true;

		//itemRigidbody.useGravity = false;
		//itemRigidbody.isKinematic = true;
	}
	
	IEnumerator RemoveItemCo(ObtainableItem item) {
		items.Remove(item);

		surface.isAbleToInteract = false;
		
		var targetPosition = item.transform.position + Vector3.up * 1f;

		var itemRigidbody = item.GetComponent<Rigidbody>();
		itemRigidbody.useGravity = false;
		itemRigidbody.isKinematic = true;

		LeanTween.cancel(item.gameObject);
		LeanTween.move(item.gameObject, targetPosition, 1f).setEase(LeanTweenType.easeOutCubic);
		
		yield return new WaitForSeconds(1f);
		
		item.isAbleToInteract = true;
		item = null;
		
		yield return new WaitForSeconds(1.5f);
		// Activate the surface with delay after removing an item,
		// so the item wont get back to inventory if Player continues to point at the spot after getting the item in hand.
		surface.isAbleToInteract = true;
	}
	
	IEnumerator CheckItemsOnSurface() {
		while (true) {
			if (items.Count == 0)
				yield return null;

			foreach (ObtainableItem item in items.ToArray()) {
				if (item != null) {
					var itemIsOnSurface = false;

					RaycastHit hitInfo;
					Physics.Raycast(item.transform.position, Vector3.down, out hitInfo);

					if (hitInfo.rigidbody != null) {
						if (hitInfo.rigidbody.GetComponent<SurfaceForItems>() != null || hitInfo.rigidbody.GetComponent<ObtainableItem>() != null) {
							itemIsOnSurface = true;
						}
					}

					// If the item doesn't remain on the surface put it back on it.
					if (!itemIsOnSurface) {
						items.Remove(item);
						StartCoroutine(AddItemCo(item, surface.transform.position));
					}
				}

				yield return new WaitForSeconds(itemCheckDelaySec);
			}
		}
	}

	public void OnItemAdd(ObtainableItem item) {
		if (items == null)
			items = new List<ObtainableItem>();

		StartCoroutine(AddItemCo(item, surface.transform.position));
	}

	public void OnItemAdd(ItemAddingToSurfaceInfo info) {
		if (items == null)
			items = new List<ObtainableItem>();
		
		StartCoroutine(AddItemCo(info.item, info.point));
	}

	public void OnItemTakeByVisitor(ObtainableItem item) {
		StartCoroutine(RemoveItemCo(item));
	}
	
}
