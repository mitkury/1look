using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	List<InventorySpot> spots = new List<InventorySpot>();

	public Transform body;
	public Vector2 size = new Vector2(1,1);
	public Vector2 amountOfSpots = new Vector2(2,2);
	public List<ObtainableItem> items { get; private set; }

	void Start() {
		CreateGridOfSpots();
	}

	void CreateGridOfSpots() {
		var colliderSize = new Vector3(size.x / amountOfSpots.x, 0.01f, size.y / amountOfSpots.y);
		var offsetX = (size.x / 2 - colliderSize.x / 2) * -1;
		var offsetZ = (size.y / 2 - colliderSize.z / 2) * -1;
		
		for (int x = 0; x < amountOfSpots.x; x++) {
			for (int y = 0; y < amountOfSpots.y; y++) {
				var inventorySpot = HComponent.Create<InventorySpot>("Spot") as InventorySpot;
				inventorySpot.transform.parent = transform;
				inventorySpot.transform.localPosition = new Vector3(x * colliderSize.x + offsetX, 0, y * colliderSize.z + offsetZ);
				inventorySpot.GetComponent<BoxCollider>().size = colliderSize;

				spots.Add(inventorySpot);
			}
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.gray;
		
		Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 0.01f, size.y));
	}

	public void AddItem(ObtainableItem item) {
		if (spots.Find(s => s.item == item) != null) {
			Debug.LogWarning("Item is already is in one of the inventory spots.");
			return;
		}

		var freeSpot = spots.Find(s => s.item == null);

		if (freeSpot == null) {
			Debug.LogWarning("There isn't any free InventorySpot available.");
			return;
		}

		freeSpot.Add(item);
	}

	public void RemoveItem(ObtainableItem item) {
		var spotWithItem = spots.Find(s => s.item == item);

		if (spotWithItem == null) {
			Debug.LogWarning("Couldn't find "+item+" in any of the InventorySpots");
			return;
		}

		spotWithItem.RemoveItem();
	}

	public void RegisterItem(ObtainableItem item) {
		if (items == null) {
			items = new List<ObtainableItem>();
		}

		items.Add(item);
	}

	public void DeregisterItem(ObtainableItem item) {
		items.Remove(item);
	}

}
