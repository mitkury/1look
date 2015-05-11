using UnityEngine;
using System.Collections;

public class GameInstantiator : MonoBehaviour {

	bool isInitialized;

	public GameObject kingPrefab;
	public GameObject playerPrefab;

	void Awake() {
		Init();
	}

	public void Init() {
		if (isInitialized)
			return;

		if (King.Instance != null) {
			enabled = false;
			return;
		}
		
		Instantiate(playerPrefab);
		Instantiate(kingPrefab);

		King.placeManager.SetPlaceManually(GetComponent<Place>());
	}
}
