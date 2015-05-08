using UnityEngine;
using System.Collections;

public class SummonsBasketballHoop : Interaction {

	public PaintingHoop paintingHoop;

	public void OnItemTakeByVisitor (ObtainableItem item) {
		if (paintingHoop.hoopIndex == -1)
			paintingHoop.NextHoop();
	}

}
