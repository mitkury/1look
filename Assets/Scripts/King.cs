using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlaceManager))]
public class King : SingletonComponent<King> {
	
	public static Visitor visitor { get; private set; }
	public static PlaceManager placeManager { get; private set; }

	void Awake() {
		#if UNITY_STANDALONE
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		#endif

		DontDestroyOnLoad(gameObject);
		visitor = (Visitor) FindObjectOfType<Visitor>();
		placeManager = GetComponent<PlaceManager>();
		Application.targetFrameRate = 60;

		if (visitor == null) {
			Debug.LogError("King couldn't locate a visitor's instance on the scene.");
		}
	}

	void Start() {
		if (placeManager.currentPlace == null) {
			//placeManager.NextPlace();
			Application.LoadLevelAsync(Application.loadedLevel+1);
		}
	}

}
