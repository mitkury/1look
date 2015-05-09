using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Basketball))]
public class SummonsBasketballHoop : Interaction {

	public PaintingHoop paintingHoop;

	public void OnItemTakeByVisitor (ObtainableItem item) {
		if (paintingHoop.hoopIndex == -1) {
			paintingHoop.NextHoop();
			paintingHoop.targetBall = GetComponent<Basketball>();
		}
	}

}
