using UnityEngine;
using System.Collections;

public class Apartment : MonoBehaviour {

	int numberOfEnables;
	int enablesToWin = 2;

	public GameObject introStage;
	public GameObject endStage;

	void OnEnable() {
		numberOfEnables +=1;

		switch(numberOfEnables) {
		case 1:
			StartCoroutine(StartIntroCo());
			break;
		case 2:
			introStage.SetActive(false);
			endStage.SetActive(true);
			break;
		}
	}

	IEnumerator StartIntroCo() {
		yield return null;
		King.visitor.sight.enabled = false;
	}


}
