using UnityEngine;
using System.Collections;

public class ScrollUp : MonoBehaviour {

	public float rate = 1.0f;

	void Update () 
	{
		Vector3 pos = transform.position;
		pos.y += rate * Time.deltaTime;
		transform.position = pos;
	}
}
