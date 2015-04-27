using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour {

	public FloatingAnimationData data;
	[HideInInspector]
	public FloatingAnchor anchor;

	void Start() {
		if (anchor == null) {
			anchor = HComponent.Create<FloatingAnchor>(gameObject.name + " floatingAnchor");
			anchor.transform.position = transform.position;
			anchor.transform.rotation = transform.rotation;
			anchor.transform.parent = transform.parent;
			anchor.data = data;
		}
	}

	void Update () {
		transform.position = Vector3.Lerp(transform.position, anchor.transform.position, 3f);
		transform.rotation = Quaternion.Lerp(transform.rotation, anchor.transform.rotation, 3f);
	}
}
