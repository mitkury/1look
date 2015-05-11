using UnityEngine;
using System.Collections;

public class ScrollUp : MonoBehaviour {

	public float rate = 1.0f;
	private float defaultY;
	public float loopY;

	void Start ()
	{
		defaultY = transform.position.y;
	}

	void Update () 
	{
		Vector3 pos = transform.position;
		pos.y += rate * Time.deltaTime;

		// Move credits back to starting position so they can loop
		if(pos.y > loopY)
			pos.y = defaultY;

		transform.position = pos;

	}
}
