using UnityEngine;
using System.Collections;

public class Darkness : MonoBehaviour {

	public string targetPlaceName = "TowerRoom";
	public float moveAfterSec = 12f;
	
	void Start () {
		StartCoroutine(StartCo());
	}

	IEnumerator StartCo() {
		while (King.Instance == null || King.visitor.sight == null)
			yield return null;

		King.visitor.sight.enabled = false;

		yield return new WaitForSeconds(moveAfterSec);

		King.placeManager.ActivatePlace(targetPlaceName);
	}

}
