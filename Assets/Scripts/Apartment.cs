using UnityEngine;
using System.Collections;

public class Apartment : MonoBehaviour {

	int numberOfEnables;
	int enablesToWin = 2;

	public GameObject introStage;
	public GameObject endStage;

	void OnEnable() {
		numberOfEnables +=1;

		if (numberOfEnables == enablesToWin) {
			introStage.SetActive(false);
			endStage.SetActive(true);
		}
	}
}
