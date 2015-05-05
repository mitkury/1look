using UnityEngine;
using System.Collections;

public class PotionBook : MonoBehaviour {

	Visitor user;
	bool bookIsShown;

	public float showBookAfterSec = 1f;
	public float hideBookAfterSec = 2f;
	public Transform bookReadPoint;
	public Transform bookHidePoint;

	void OnEnable() {
		transform.position = bookHidePoint.position;
		transform.rotation = bookHidePoint.rotation;

		StartCoroutine(FindUserCo());
	}

	void Update() {
		if (user == null)
			return;

		var targetPosition = bookHidePoint.position;
		var targetRotation = bookHidePoint.rotation;
		
		if (user.isLookingAtBookReadPosition) {
			targetPosition = bookReadPoint.position;
			targetRotation = bookReadPoint.rotation;
		}
		
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
	}

	IEnumerator FindUserCo() {
		if (King.visitor == null)
			yield return null;

		user = King.visitor;
	}

}
