using UnityEngine;
using System.Collections;

public struct ItemAddingToSurfaceInfo {
	public ObtainableItem item;
	public Vector3 point;
}

public class SurfaceForItems : InteractiveObject {

	[HideInInspector]
	public GameObject receiver;

	public void AddItem(ObtainableItem item) {
		if (receiver == null && transform.parent != null)
			receiver = transform.parent.gameObject;

		ItemAddingToSurfaceInfo info;
		info.item = item;
		info.point = King.visitor.sight.hitInfo.point;

		receiver.gameObject.SendMessage("OnItemAdd", info);
	}

}
