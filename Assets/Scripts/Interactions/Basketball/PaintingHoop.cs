using UnityEngine;
using System.Collections;

public class PaintingHoop : Interaction {

	Animator _animator;
	string initName;
	Vector3 initScale;
	bool shieldIsExpanded;
	ForceOverrider forceOverrider;
	[HideInInspector]
	public Basketball targetBall;
	public MeshRenderer hoopRenderer;
	public Transform visiblePointForBall;
	
	public Vector3 hoopScale = new Vector3(1f, 2.38f, 1.14f);
	public GameObject shield;
	public GameObject prize;
	public GameObject[] objectsForActivation;
	public float hoopsGoal = 3;
	public float[] hoopForces;
	public int hoopIndex { get; private set; }

	void Start() {
		_animator = GetComponent<Animator>();
		initName = gameObject.name;
		initScale = shield.transform.localScale;
		forceOverrider = GetComponent<ForceOverrider>();
		hoopIndex = -1;
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

	public void DisableInteractivity() {
		GetComponent<InteractiveObject>().isAbleToInteract = false;
	}

	public void EnableInteractivity() {
		GetComponent<InteractiveObject>().isAbleToInteract = true;
	}

	public void SwitchShield() {
		if (shieldIsExpanded) {
			StartCoroutine(SrinkShieldCo());
		} else {
			StartCoroutine(ExpandShieldCo());
		}

		shieldIsExpanded = !shieldIsExpanded;
	}

	public void ExpandShield() {
		StartCoroutine(ExpandShieldCo());
	}

	public void OnGoal() {
		var soundPlayer = GetComponent<PlaysSoundOnRequest>();
		if (soundPlayer != null)
			soundPlayer.PlayOneShot(0);
		
		if (prize != null && hoopsGoal <= hoopIndex + 1) {
			prize.SetActive(true);
			prize.transform.parent = transform.parent;
			prize = null;

			if (targetBall != null)
				targetBall.Drop(visiblePointForBall.position);

			if (GetComponent<PlaysAudioRemarkOnRadio>() != null) {
				GetComponent<PlaysAudioRemarkOnRadio>().Play(0);
			}
		}

		NextHoop();
	}

	public void NextHoop() {
		_animator.SetTrigger("Next");
		hoopIndex += 1;

		if (hoopForces.Length > hoopIndex)
			forceOverrider.newForce = hoopForces[hoopIndex];

		GetComponent<InteractiveObject>().isAbleToBeActivatedOnItsOwn = false;
		hoopRenderer.gameObject.SetActive(true);
	}

	public void ResetHoopIndex() {
		hoopIndex = -1;
		GetComponent<InteractiveObject>().isAbleToBeActivatedOnItsOwn = true;

		if (targetBall != null)
			targetBall.Drop(visiblePointForBall.position);

		hoopRenderer.gameObject.SetActive(false);
	}

}
