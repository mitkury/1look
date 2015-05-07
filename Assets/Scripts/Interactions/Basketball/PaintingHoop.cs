using UnityEngine;
using System.Collections;

public class PaintingHoop : Interaction {

	Animator _animator;
	string initName;
	Vector3 initScale;

	public Vector3 hoopScale = new Vector3(1f, 2.38f, 1.14f);
	public GameObject shield;
	public GameObject[] objectsForActivation;

	void Start() {
		_animator = GetComponent<Animator>();
		initName = gameObject.name;
		initScale = shield.transform.localScale;
	}

	IEnumerator ExpandShieldCo() {
		LeanTween.scale(shield, hoopScale, 3f).setEase(LeanTweenType.easeInOutElastic);
		yield return new WaitForSeconds(3f);
		gameObject.name = "ThrowingSurface";

		foreach (GameObject go in objectsForActivation) {
			go.SetActive(true);
		}
	}

	IEnumerator SrinkShieldCo() {
		LeanTween.scale(shield, initScale, 3f).setEase(LeanTweenType.easeInOutElastic);
		yield return new WaitForSeconds(3f);
		gameObject.name = initName;
		
		foreach (GameObject go in objectsForActivation) {
			go.SetActive(false);
		}
	}

	public override void Interact () {
		_animator.SetTrigger("Intro");
	}

	public void EnableInteractivity() {
		GetComponent<InteractiveObject>().isAbleToInteract = true;
	}

	bool shieldIsExpanded;

	public void SwitchShield() {
		if (shieldIsExpanded)
			StartCoroutine(SrinkShieldCo());
		else
			StartCoroutine(ExpandShieldCo());

		shieldIsExpanded = !shieldIsExpanded;
	}

	public void ShowHoop() {
		GetComponent<InteractiveObject>().isAbleToBeActivatedOnItsOwn = false;
		_animator.SetTrigger("GoToHoop1");
	}

	public void ExpandShield() {
		StartCoroutine(ExpandShieldCo());
	}

	public void OnGoal() {
		var soundPlayer = GetComponent<PlaysSoundOnRequest>();
		if (soundPlayer != null)
			soundPlayer.PlayOneShot(0);

		_animator.SetTrigger("GoBack");
		GetComponent<InteractiveObject>().isAbleToInteract = false;
	}

}
