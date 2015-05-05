using UnityEngine;
using System.Collections;

public class RecipeIcon : Interaction {

	public float hoverForSec = 1f;

	Vector3 initLocalPosition;
	Quaternion initLocalRotation;
	float initFocusTimeSec;
	bool isHoverd;
	float startedHoveredTime;

	void Start() {
		initLocalPosition = transform.localPosition;
		initLocalRotation = transform.localRotation;
		initFocusTimeSec = GetComponent<InteractiveObject>().focusTimeSec;
	}

	void Update() {
		if (isHoverd && hoverForSec < Time.time - startedHoveredTime) {
			StartCoroutine(HoverDownCo());
		}
	}

	IEnumerator HoverUpCo() {
		isHoverd = true;
		startedHoveredTime = Time.time;
		LeanTween.moveLocal(gameObject, transform.forward * 0.025f, 0.25f).setEase(LeanTweenType.easeInOutCubic);
		//var rotationTowarsVisitor = Quaternion.LookRotation(King.visitor.sight.anchor.position - transform.position);
		//LeanTween.rotate(gameObject, rotationTowarsVisitor.eulerAngles, 0.5f).setEase(LeanTweenType.easeInOutCubic);
		yield return new WaitForSeconds(0.25f);
		GetComponent<InteractiveObject>().isAbleToInteract = true;
		GetComponent<InteractiveObject>().hasItsOwnFocusTime = true;
		GetComponent<InteractiveObject>().focusTimeSec = 0f;
	}

	IEnumerator HoverDownCo() {
		GetComponent<InteractiveObject>().isAbleToInteract = true;
		GetComponent<InteractiveObject>().hasItsOwnFocusTime = true;
		GetComponent<InteractiveObject>().focusTimeSec = initFocusTimeSec;
		isHoverd = false;
		LeanTween.moveLocal(gameObject, initLocalPosition, 0.25f).setEase(LeanTweenType.easeInOutCubic);
		LeanTween.rotateLocal(gameObject, initLocalRotation.eulerAngles, 0.25f).setEase(LeanTweenType.easeInOutCubic);
		yield return new WaitForSeconds(0.25f);
	}

	public override void Interact () {
		if (isHoverd) {
			startedHoveredTime = Time.time;
		} else { 
			StartCoroutine(HoverUpCo());
		}
	}
	
}
