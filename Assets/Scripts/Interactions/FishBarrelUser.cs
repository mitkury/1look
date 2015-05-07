using UnityEngine;
using System.Collections;

public class FishBarrelUser : Interaction {
	
	public InteractiveObject barrel;

	public void OnItemTakeByVisitor(ObtainableItem item) {
		barrel.GetComponent<PlaysAnimationOnSight>().isAbleToInteractAfterAnimation = false;
	}

}
