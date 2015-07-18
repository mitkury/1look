using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlaceLoader))]
public class PlaceManager : MonoBehaviour {

	AsyncOperation levelLoaderAsync;
	bool placeIsLoading;
	
	public Place currentPlace { get; private set; }
	public bool placesAreLoading { get; private set; }
	public List<Place> places = new List<Place>();
	public List<int> scenesToExcept = new List<int>();

	/*
	void LoadPlace(int levelIndex) {
		if (placeIsLoading)
			return;

		placeIsLoading = true;

		// Initiate async loading of a new scene.
		levelLoaderAsync = Application.LoadLevelAsync(levelIndex);
		levelLoaderAsync.allowSceneActivation = false;
		Application.backgroundLoadingPriority = ThreadPriority.Low;
	}

	IEnumerator ActivateLoadedPlaceCo() {
		if (levelLoaderAsync == null) {
			Debug.LogError("There is no inactive scene to activate!");
			yield break;
		}

		// Continue when the progress reaches 90%. It reaches max 90% before an 'allowSceneActivation' sets to TRUE.
		while (!levelLoaderAsync.isDone && levelLoaderAsync.progress < 0.9f)
			yield return null;
		
		OnLoadCompleteBeforeSceneActivation();
		
		// Activate the loaded scene.
		levelLoaderAsync.allowSceneActivation = true;
		yield return levelLoaderAsync;
		
		// Wait until a new place that comes with the new scene register itself in PlaceManager.
		while(currentPlace == null)
			yield return null;
		
		OnLoadCompleteAfterSceneActivation();
		
		placeIsLoading = false;
		levelLoaderAsync = null;
		yield break;
	}

	void OnLoadCompleteBeforeSceneActivation() {
		var player = King.visitor;
		
		LeanTween.cancel(player.gameObject);
	}

	void OnLoadCompleteAfterSceneActivation() {
		var player = King.visitor;
		
		//player.MoveTo(currentPlace.vantagePoint.transform.position);
	}

	public void NextPlace(bool manualActivation = false) {
		if (placeIsLoading)
			return;

		if (Application.loadedLevel + 1 < Application.levelCount) {
			LoadPlace(Application.loadedLevel + 1);
		} else {
			LoadPlace(1);
		}
		
		if (!manualActivation) {
			StartCoroutine(ActivateLoadedPlaceCo());
		}
	}

	public void ActivateLoadedPlace() {
		StartCoroutine(ActivateLoadedPlaceCo());
	}

	public void AddPlace(Place place) {
		currentPlace = place;
	}
	*/
	
	// NEW STUFF:

	Place newlyLoadedPlace;

	public void AddPlace(Place place) {
		places.Add(place);
		newlyLoadedPlace = place;
	}
	
	IEnumerator PreloadPlacesCo() {
		placesAreLoading = true;

		King.visitor.sight.enabled = false;
		King.visitor.SetBackground(Color.black);
		yield return new WaitForSeconds(0.1f);

		var spinningBottle = GameObject.FindObjectOfType<Title>().gameObject;

		var prevLoadingPriority = Application.backgroundLoadingPriority;
		Application.backgroundLoadingPriority = ThreadPriority.High;

		for(int i = 0; i < Application.levelCount; i++) {
			if (i == Application.loadedLevel || scenesToExcept.Find(s => s == i) == i)
				continue;

			newlyLoadedPlace = null;
			
			// Initiate async loading of a new scene.
			levelLoaderAsync = Application.LoadLevelAdditiveAsync(i);
			levelLoaderAsync.allowSceneActivation = false;

			// Continue when the progress reaches 90%. It reaches max 90% before an 'allowSceneActivation' sets to TRUE.
			while (!levelLoaderAsync.isDone && levelLoaderAsync.progress < 0.9f)
				yield return null;

			// Activate the loaded scene.
			levelLoaderAsync.allowSceneActivation = true;
			yield return levelLoaderAsync;
			
			// Wait until a new place that comes with the new scene register itself in PlaceManager.
			while(newlyLoadedPlace == null)
				yield return null;
		}

		placesAreLoading = false;

		Application.backgroundLoadingPriority = prevLoadingPriority;
	}


	void Update() {
		if (placesAreLoading)
			return;

		if (Input.GetKeyDown(KeyCode.Keypad1)) {
			ActivatePlace("Apartment");
		}
		if (Input.GetKeyDown(KeyCode.Keypad2)) {
			ActivatePlace("TowerRoom");
		}
	}

	void SetSettingsForPlace(Place targetPlace) {
		King.visitor.SetBackground(targetPlace.background);
		RenderSettings.ambientSkyColor = targetPlace.ambientColor;
		if (targetPlace.fogDensity > 0f) {
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.Exponential;
			RenderSettings.fogColor = targetPlace.fogColor;
			RenderSettings.fogDensity = targetPlace.fogDensity;
		} else
			RenderSettings.fog = false;
	}

	IEnumerator ActivatePlaceCo(string placeName) {
		if (currentPlace == null || currentPlace.name != placeName) {

			LeanTween.value(gameObject, delegate(float value) { 
				AudioListener.volume = value;
			}, 1f, 0f, 2f);

			King.visitor.screenFader.FadeIn(2f);
			yield return new WaitForSeconds(2.5f);
		} else {
			King.visitor.screenFader.FadeIn(0f);
		}

		yield return new WaitForSeconds(0.1f);

		if (King.visitor.itemInHand != null) {
			King.visitor.Drop(King.visitor.itemInHand);
		}

		King.visitor.sight.enabled = false;

		var targetPlace = places.Find(p => p.name == placeName);

		foreach (Place place in places) {
			if (place != targetPlace)
				place.gameObject.SetActive(false);
		}

		targetPlace.gameObject.SetActive(true);
		currentPlace = targetPlace;

		SetSettingsForPlace(currentPlace);

		King.visitor.sight.enabled = true;

		yield return new WaitForSeconds(1f);

		King.visitor.screenFader.FadeOut(2f);

		LeanTween.value(gameObject, delegate(float value) { 
			AudioListener.volume = value;
		}, 0f, 1f, 2f);
	}

	public void PreloadPlaces() {
		StartCoroutine(PreloadPlacesCo());
	}

	public void ActivatePlace(string placeName) {
		StartCoroutine(ActivatePlaceCo(placeName));
	}

	public void SetPlaceManually(Place targetPlace) {
		currentPlace = targetPlace;
		SetSettingsForPlace(targetPlace);
	}



}
