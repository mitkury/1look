using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject body;
	public Reticle reticle;

	bool isVisible;
	float longPressDelay = 0.75f;
	float homeButtonDownTime = 0.0f;
	RaycastHit hitInfo;
	LayerMask layerMask = 1;
	MenuButton buttonInFocus;
	PlaysSoundOnRequest soundPlayer;

	Vector3 onShowPosition;
	Vector3 onHidePosition;

	public void Show() { 
		SetVisibility(true);
	}

	public void Hide() {
		SetVisibility(false);
	}

	public void QuiteGame() {
		#if UNITY_ANDROID && !UNITY_EDITOR
		// show the platform UI
		OVRPluginEvent.Issue(RenderEventType.PlatformUIConfirmQuit);
		#endif
	}

	public void RestartGame() {
		Hide();
		King.RestartGame();
	}

	void SetVisibility(bool show) {
		isVisible = show;

		var targetPosition = onShowPosition;
		var targetScaleY = 1f;
		var targetEase = LeanTweenType.easeOutBounce;

		if (!show) {
			reticle.SetBodyScale(0f);

			targetPosition = onHidePosition;
			targetScaleY = 0f;
			targetEase = LeanTweenType.easeInSine;

			if (King.placeManager.currentPlace != null && King.placeManager.currentPlace.placeState == Place.PlaceState.Playable) {
				King.visitor.sight.enabled = true;
			}

			buttonInFocus = null;

			soundPlayer.PlayOneShot(1);
		} else {
			body.SetActive(true);

			soundPlayer.PlayOneShot(0);
		}

		LeanTween.cancel(body);

		LeanTween.move(body, targetPosition, 1f).setOnComplete(delegate() {
			body.SetActive(show);

		}).setEase(targetEase);

		LeanTween.scaleY(body, targetScaleY, 1f).setEase(targetEase);

	}

	void Start () {
		soundPlayer = GetComponent<PlaysSoundOnRequest>();

		onShowPosition = King.visitor.transform.position + Vector3.forward * 1.3f + Vector3.up * 1.88f;
		onHidePosition = onShowPosition + Vector3.up * 1.6f;

		body.transform.position = onHidePosition;

		reticle.SetBody(0);

		body.SetActive(false);
	}

	void Update () {
		UpdateVisibility();
		UpdateMenuReticle();

		if (Input.GetMouseButtonDown(0) && isVisible && buttonInFocus != null) {
			buttonInFocus.Press(this);
		}

		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Q)) {
			System.DateTime now = System.DateTime.Now;
			string format = "yyyy_MMM-d_HH-mm-ff";
			string name = now.ToString(format);
			Application.CaptureScreenshot("Screenshots/"+name+".png", 2);
		}
		#endif
	}

	void UpdateVisibility() {

		if (Input.GetKeyDown(KeyCode.Escape)) {
			homeButtonDownTime = Time.realtimeSinceStartup;
		} else if (Input.GetKeyUp(KeyCode.Escape)) {
			homeButtonDownTime = 0f;
		}

		float elapsedTime = homeButtonDownTime != 0f ? Time.realtimeSinceStartup - homeButtonDownTime : 0f;

		if (elapsedTime >= longPressDelay) {
			homeButtonDownTime = 0f;
			Debug.Log("Open the platform UI");
			#if UNITY_ANDROID && !UNITY_EDITOR
			// Show the platform UI
			OVRPluginEvent.Issue(RenderEventType.PlatformUI);
			#endif
		}

		if (isVisible) {
			if (King.visitor.sight.enabled) {
				King.visitor.sight.enabled = false;
			}
		}

		if (King.placeManager.currentPlace == null || King.placeManager.currentPlace.placeState != Place.PlaceState.Playable) {
			if (isVisible) {
				Hide();
			}

			return;
		}

		if (!isVisible) {
			if (Input.GetKeyUp(KeyCode.Escape)) {
				Show();
			}
		} else {
			if (Input.GetKeyUp(KeyCode.Escape)) {
				Hide();
			}	
		}
	}

	void UpdateMenuReticle() {
		if (!isVisible)
			return;

		var facingVector = King.visitor.sight.anchor.TransformDirection(Vector3.forward);
		Physics.Raycast(King.visitor.sight.anchor.position, facingVector, out hitInfo, 500f, layerMask);

		Vector3 targetPosition = hitInfo.collider != null ? hitInfo.point : facingVector * 500f;
		var reticleDistance = hitInfo.collider != null ? hitInfo.distance : 500f;
		var targetScale = Vector3.one * reticleDistance;

		reticle.transform.LookAt(King.visitor.sight.anchor.transform.position);
		reticle.SetFocus(targetPosition, targetScale);

		if (hitInfo.collider != null && hitInfo.collider.GetComponent<MenuButton>() != null) {
			buttonInFocus = hitInfo.collider.GetComponent<MenuButton>();
			reticle.SetBodyScale(1f);
			reticle.activeFocus.completenes = 1f;
		} else {
			buttonInFocus = null;
			reticle.SetBodyScale(0.5f);
			reticle.activeFocus.completenes = 0;
		}
	}
}
