using UnityEngine;
using System.Collections;

public class ActivatesInteractiveThingAfterSec : MonoBehaviour {

	public float timer;
	public InteractiveThing target;

	void Start () {
		target = target == null ? GetComponent<InteractiveThing>() : target;
		StartCoroutine(ActivateAfterSecCo());
	}

	IEnumerator ActivateAfterSecCo() {
		yield return new WaitForSeconds(timer);

		target.isAbleToInteract = true;
	}
	

}
