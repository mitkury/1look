using UnityEngine;
using System.Collections;

public class BasketballHoop : Interaction {
	
	bool ballWentThroughFirstSensor;
	bool ballWentThroughSecondSensor;
	
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
		gameObject.SendMessage("OnGoal");
	}
}
