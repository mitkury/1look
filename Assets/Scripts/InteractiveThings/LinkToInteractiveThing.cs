using UnityEngine;
using System.Collections;

public class LinkToInteractiveThing : MonoBehaviour {

	public InteractiveThing target = null;

	void Start () {
		if (target == null) {
			Debug.LogError("Missing a link to an InteractiveThing");
		}
	}
}
