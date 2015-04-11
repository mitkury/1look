using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position + Vector3.up, 0.25f);
	}
	
}
