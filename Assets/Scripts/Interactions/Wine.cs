using UnityEngine;
using System.Collections;

public class Wine : Interaction {

	bool particlesAreActivated;
	InteractiveThing interactiveThing;

	public string targetPlaceName = "Darkness";
	public float activateAfterSec = 35f;
	public float goToSleepAfterSec = 3f;
	public GameObject goForActivation;

	void Start () {
		interactiveThing = GetComponent<InteractiveThing>();
		interactiveThing.isAbleToInteract = false;
		King.visitor.sight.enabled = false;

		StartCoroutine(ActivateAfterSec(activateAfterSec));
	}

	void Update() {
		if (!particlesAreActivated && (interactiveThing.isAbleToInteract || King.visitor.sight.enabled))
			ActivateParticles();
	}

	void ActivateParticles() {
		particlesAreActivated = true;

		if (goForActivation != null)
			goForActivation.SetActive(true);

		interactiveThing.isAbleToInteract = true;
		King.visitor.sight.enabled = true;
	}

	IEnumerator ActivateAfterSec(float time) {
		yield return new WaitForSeconds(time);

		interactiveThing.isAbleToInteract = true;
	}

	IEnumerator GoToSleepInSecCo(float time) {
		yield return new WaitForSeconds(1.5f);
		GetComponent<PlaysSoundOnRequest>().PlayOneShot(0);

		yield return new WaitForSeconds(time);

		King.placeManager.ActivatePlace(targetPlaceName);
	}
	
	public override void Interact () {

	}

	public void OnItemTakeByVisitor(ObtainableItem item) {
		StartCoroutine(GoToSleepInSecCo(goToSleepAfterSec));
	}
}
