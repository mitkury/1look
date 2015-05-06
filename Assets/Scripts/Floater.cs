using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Floater : MonoBehaviour {

	bool isFlyingToAnchor;

	public FloatingAnimationData data;
	[HideInInspector]
	public FloatingAnchor anchor;

	void Awake() {
		if (anchor == null) {
			anchor = HComponent.Create<FloatingAnchor>(gameObject.name + " floatingAnchor");
			anchor.transform.position = transform.position;
			anchor.transform.rotation = transform.rotation;
			anchor.transform.parent = transform.parent;
			anchor.data = data;
		}
	}

	void OnDisable() {
		isFlyingToAnchor = true;
	}

	void OnEnable() {}

	void Update () {
		if (anchor == null)
			return;

		if (!isFlyingToAnchor) {
			transform.position = anchor.transform.position;
			transform.rotation = anchor.transform.rotation;
			//transform.position = Vector3.Lerp(transform.position, anchor.transform.position, Time.deltaTime * 25f);
			//transform.rotation = Quaternion.Lerp(transform.rotation, anchor.transform.rotation, Time.deltaTime * 25f);
		} else {
			transform.position = Vector3.Lerp(transform.position, anchor.transform.position, Time.deltaTime * 2f);
			transform.rotation = Quaternion.Lerp(transform.rotation, anchor.transform.rotation, Time.deltaTime * 2f);

			var remainingDistance = Vector3.Distance(transform.position, anchor.transform.position);
			var remainingAngle = Quaternion.Angle(transform.rotation, anchor.transform.rotation);

			if (remainingDistance < 0.01f && remainingAngle < 1f)
				isFlyingToAnchor = false;
		}
	}

	public void OnItemTakeByVisitor(ObtainableItem item) {
		enabled = false;
	}
}
