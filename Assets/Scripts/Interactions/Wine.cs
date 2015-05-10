using UnityEngine;
using System.Collections;

public class Wine : Interaction {

	bool particlesAreActivated;
	InteractiveThing interactiveThing;

	public string targetPlaceName = "Darkness";
	public float moveAfterSec = 3f;

	void Start () {
		interactiveThing = GetComponent<InteractiveThing>();
		interactiveThing.isAbleToInteract = false;
	}

	void Update() {
		if (!particlesAreActivated && interactiveThing.isAbleToInteract)
			ActivateParticles();
	}

	void ActivateParticles() {
		particlesAreActivated = true;

		Debug.Log("Ready to drink some wine!");
	}

	IEnumerator GoToSleepInSecCo(float time) {
		yield return new WaitForSeconds(time);

		King.placeManager.ActivatePlace(targetPlaceName);
	}
	
	public override void Interact () {

	}

	public void OnItemTakeByVisitor(ObtainableItem item) {
		StartCoroutine(GoToSleepInSecCo(moveAfterSec));
	}
}
