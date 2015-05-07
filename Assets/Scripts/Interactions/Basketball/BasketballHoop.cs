using UnityEngine;
using System.Collections;

public class BasketballHoop : Interaction {
	
	bool ballWentThroughFirstSensor;
	bool ballWentThroughSecondSensor;

	public GameObject hiddenObject;
	
	void OnSensorEnter() {
		if (!ballWentThroughFirstSensor) {
			ballWentThroughFirstSensor = true;
			StartCoroutine(PreThroughHitCo());
		}
	}

	void OnSensorExit() {
		ballWentThroughSecondSensor = true;
	}

	IEnumerator PreThroughHitCo() {
		float checkForSec = 2.5f;
		float timePass = 0;

		while(timePass < checkForSec) {

			if (ballWentThroughSecondSensor) {
				Goal();
				ballWentThroughFirstSensor = false;
				ballWentThroughSecondSensor = false;
				yield break;
			}

			timePass += Time.deltaTime;
			yield return null;
		}

		ballWentThroughFirstSensor = false;
	}

	void Goal() {
		Debug.Log("Goal!");

		if (hiddenObject != null) {
			hiddenObject.SetActive(true);
			hiddenObject.transform.parent = transform.parent;
		}

		gameObject.SendMessage("OnGoal");
	}
}
