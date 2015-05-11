using UnityEngine;
using System.Collections;

public class FinalPotion : Interaction {

	public string targetPlaceName = "Apartment";
	public float moveAfterSec = 3f;

	IEnumerator GoToPlaceInSec(float time) {
		yield return new WaitForSeconds(time);
		
		King.placeManager.ActivatePlace(targetPlaceName);
	}
	
	public void OnItemTakeByVisitor(ObtainableItem item) {
		StartCoroutine(GoToPlaceInSec(moveAfterSec));
	}
}
