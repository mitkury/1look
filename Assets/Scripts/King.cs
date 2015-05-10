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

		#if UNITY_EDITOR
		QualitySettings.antiAliasing = 4;
		QualitySettings.vSyncCount = 1;
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

		/*
		if (placeManager.currentPlace == null) {
			//placeManager.NextPlace();
			Application.LoadLevelAsync(Application.loadedLevel+1);
		}
		*/

		/*
		if (placeManager.currentPlace == null) {
			LoadPlace(1);
		} else {
			visitor.SetBackground(placeManager.currentPlace.background);
		}
		*/

		StartCoroutine(BeginJourneyCo());
	}

	IEnumerator BeginJourneyCo() {
		AudioListener.volume = 0;

		while (King.visitor.sight == null)
			yield return null;

		King.visitor.sight.enabled = false;

		if (FindObjectOfType<Place>() == null) {
			placeManager.PreloadPlaces();
		}



		while(placeManager.placesAreLoading)
			yield return null;

		placeManager.ActivatePlace("Apartment");

		LeanTween.value(gameObject, delegate(float value) { 
			AudioListener.volume = value;
		}, 0f, 1f, 1f);

		yield return new WaitForSeconds(35f);

		King.visitor.sight.enabled = true;
	}


	// TODO: move into a separate class.

	/*
	AsyncOperation async;
	public bool placeIsLoading { get; private set; }

	void LoadPlace(int levelIndex) {
		if (placeIsLoading)
			return;
		
		placeIsLoading = true;
		
		// Initiate async loading of a new scene.
		async = Application.LoadLevelAdditiveAsync(levelIndex);
		async.allowSceneActivation = false;
		Application.backgroundLoadingPriority = ThreadPriority.Low;

		StartCoroutine(ActivateLoadedPlaceCo());
	}

	IEnumerator ActivateLoadedPlaceCo() {
		if (async == null) {
			Debug.LogError("There is no inactive scene to activate!");
			yield break;
		}
		
		// Continue when the progress reaches 90%. It reaches max 90% before an 'allowSceneActivation' sets to TRUE.
		while (!async.isDone && async.progress < 0.9f)
			yield return null;
		
		OnLoadCompleteBeforeSceneActivation();
		
		// Activate the loaded scene.
		async.allowSceneActivation = true;
		yield return async;
		
		// Wait until a new place that comes with the new scene register itself in PlaceManager.
		while(King.placeManager.currentPlace == null)
			yield return null;
		
		OnLoadCompleteAfterSceneActivation();
		
		placeIsLoading = false;
		async = null;
		yield break;
	}
	
	void OnLoadCompleteBeforeSceneActivation() {

	}
	
	void OnLoadCompleteAfterSceneActivation() {
		King.placeManager.currentPlace.gameObject.SetActive(false);
	}
	*/

	#if UNITY_EDITOR
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Break();
		}
	}
	#endif

	public static bool isInVRMode {
		get {
			#if UNITY_EDITOR || UNITY_STANDALONE
			return false;
			#endif

			return true;
		}
	}

}
