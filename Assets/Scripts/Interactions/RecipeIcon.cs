using UnityEngine;
using System.Collections;

public class RecipeIcon : Interaction {

	float hoverForSec = 0.5f;
	Vector3 initLocalPosition;
	Quaternion initLocalRotation;
	float initFocusTimeSec;
	bool isHoverd;
	bool isUsed;
	float startedHoveredTime;

	public string itemName { get; private set; }

	void Start() {
		initLocalPosition = transform.localPosition;
		initLocalRotation = transform.localRotation;
		initFocusTimeSec = GetComponent<InteractiveObject>().focusTimeSec;

		itemName = gameObject.name.Replace("Icon_", "");
	}

	void Update() {
		if (isHoverd && hoverForSec < Time.time - startedHoveredTime) {
			StartCoroutine(HoverDownCo());
		}
	}

	IEnumerator HoverUpCo() {
		if (isUsed)
			yield break;

		isHoverd = true;
		startedHoveredTime = Time.time;
		LeanTween.cancel(gameObject);
		LeanTween.moveLocal(gameObject, transform.forward * 0.025f, 0.25f).setEase(LeanTweenType.easeInOutCubic);
		//var rotationTowarsVisitor = Quaternion.LookRotation(King.visitor.sight.anchor.position - transform.position);
		//LeanTween.rotate(gameObject, rotationTowarsVisitor.eulerAngles, 0.5f).setEase(LeanTweenType.easeInOutCubic);
		yield return new WaitForSeconds(0.25f);

		if (isUsed)
			yield break;

		GetComponent<InteractiveObject>().isAbleToInteract = true;
		GetComponent<InteractiveObject>().hasItsOwnFocusTime = true;
		GetComponent<InteractiveObject>().focusTimeSec = 0f;
	}

	IEnumerator HoverDownCo() {
		if (isUsed)
			yield break;

		GetComponent<InteractiveObject>().isAbleToInteract = true;
		GetComponent<InteractiveObject>().hasItsOwnFocusTime = true;
		GetComponent<InteractiveObject>().focusTimeSec = initFocusTimeSec;
		isHoverd = false;
		LeanTween.cancel(gameObject);
		LeanTween.moveLocal(gameObject, initLocalPosition, 0.25f).setEase(LeanTweenType.easeInOutCubic);
		LeanTween.rotateLocal(gameObject, initLocalRotation.eulerAngles, 0.25f).setEase(LeanTweenType.easeInOutCubic);
	}

	IEnumerator HideCo() {
		if (isUsed)
			yield break;

		isUsed = true;

		GetComponent<InteractiveObject>().isAbleToInteract = false;
		isHoverd = false;
		LeanTween.cancel(gameObject);
		LeanTween.moveLocal(gameObject, transform.forward * 0.05f, 0.7f).setEase(LeanTweenType.easeInOutCubic);
		var newLocalRotation = new Vector3(initLocalRotation.eulerAngles.x + 180f, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
		LeanTween.rotateLocal(gameObject, newLocalRotation, 1f).setEase(LeanTweenType.easeInOutCubic);
		yield return new WaitForSeconds(1f);
		LeanTween.moveLocal(gameObject, initLocalPosition, 1f).setEase(LeanTweenType.easeInOutCubic);
	}

	public override void Interact () {
		if (isUsed)
			return;

		if (isHoverd) {
			startedHoveredTime = Time.time;
		} else { 
			StartCoroutine(HoverUpCo());
		}
	}

	public void Hide() {
		StartCoroutine(HideCo());
	}
	
}
