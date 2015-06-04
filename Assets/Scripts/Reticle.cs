using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {

	float targetBodyScale = 1f;
	float targetBodyOpacity = 1f;
	Vector3 targetPosition;
	Vector3 targetScale;

	public Vector3 originalScale;
	public Vector3 bodyOriginalScale;
	public Circle idleFocus;
	public Circle activeFocus;
	public Transform anchor;
	public LayerMask layerMask;
	public InteractiveThing target;
	public Collider targetCollider;
	public Vector3 facingVector;
	public Transform body;
	public Transform[] bodies;
	public Circle[] activeIndicators;


	void Start () {
		originalScale = transform.localScale;
		bodyOriginalScale = body.localScale;
	}

	void Update() {
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 25f);
		transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 25f);
	}

	public void SetBody(int index) {
		activeFocus.completenes = 0;

		foreach (Transform b in bodies) {
			b.gameObject.SetActive(false);
		}

		body = bodies[index];
		body.gameObject.SetActive(true);
		activeFocus = activeIndicators[index];
	}

	public void SetFocus(Vector3 position, Vector3 scale) {
		targetPosition = position;
		targetScale = scale;
		/*
		if (Vector3.Distance(targetPosition, transform.position) > 1f) {
			transform.position = targetPosition;
		}
		*/

		//transform.localScale = scale;
	}

	public void SetBodyScale(float value) {
		//var v = (float) System.Math.Round((double)value, 2);
		if (targetBodyScale == value)
			return;

		targetBodyScale = value;

		var targetScale = new Vector3(bodyOriginalScale.x * value, bodyOriginalScale.y * value, bodyOriginalScale.z);
		//body.localScale = targetScale;
		LeanTween.cancel(body.gameObject);
		LeanTween.scale(body.gameObject, targetScale, 0.15f);
	}

	// Temp hack.
	public void ResetTargetBodyScale() {
		targetBodyScale = 0.999f;
	}

	public void SetBodyOpacity(float alpha) {
		if (targetBodyOpacity == alpha)
			return;
		
		targetBodyOpacity = alpha;

		LeanTween.alpha(body.gameObject, alpha, 0.15f);
	}

	/*
	void Update () {

		if (anchor == null)
			return;

		transform.LookAt(anchor.transform.position);

		facingVector = anchor.TransformDirection(Vector3.forward);
		RaycastHit hitInfo;
		float distance = 500;
		Vector3 targetPosition = facingVector * distance;

		if (Physics.Raycast(anchor.position, facingVector, out hitInfo, distance, layerMask)) { 
			targetPosition = hitInfo.point;
			distance = hitInfo.distance;
		}

		transform.position = targetPosition;
		//transform.position = Vector3.Lerp(transform.position, targetPosition, 1);
		transform.localScale = originalScale * distance;
		//transform.localScale = Vector3.Lerp(transform.localScale, originalScale * distance, 5f);


		target = null;

		targetCollider = hitInfo.collider;

		if(targetCollider != null) {
			var thing = targetCollider.gameObject.GetComponent<InteractiveThing>();

			if (thing != null) {
				target = thing;
			} else {
				var link = targetCollider.gameObject.GetComponent<LinkToInteractiveThing>();

				if (link != null) {
					target = link.target;
				}
			}
		}

	}
	*/
}