using UnityEngine;
using System.Collections;

public class TestMovement : MonoBehaviour {

	public Transform[] points;

	void Start () {
		Move();
	}

	void Move() {
		transform.position = points[0].position;

		Hashtable optional = new Hashtable();


		LeanTween.move(gameObject, points[1].position, 10f).setOnComplete(Move);

		
	}

	void Update() {
		Debug.Log(GetComponent<Rigidbody>().velocity);
	}

}
