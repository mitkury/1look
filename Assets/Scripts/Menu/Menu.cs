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
		Debug.Log("Restart");
	}

	void SetVisibility(bool show) {
		isVisible = show;

		var onShowPosition = King.visitor.transform.position + Vector3.forward * 1.3f + Vector3.up * 1.88f;
		var onHidePosition = onShowPosition + Vector3.up * 2f;
		//body.transform.position = King.visitor.transform.position + Vector3.forward * 1.3f + Vector3.up * 1.88f;

		reticle.SetBodyScale(0f);

		var targetPosition = onShowPosition;
		var targetScaleY = 1f;
		var targetEase = LeanTweenType.easeInOutBounce;

		King.visitor.sight.enabled = !show;

		if (!show) {
			targetPosition = onHidePosition;
			targetScaleY = 0f;
			targetEase = LeanTweenType.easeInSine;
		}

		if (show) {
			body.SetActive(true);
		}

		LeanTween.cancel(body);

		LeanTween.move(body, targetPosition, 1f).setOnComplete(delegate() {
			body.SetActive(show);
		}).setEase(targetEase);

		LeanTween.scaleY(body, targetScaleY, 1f);

	}

	void Start () {
		reticle.SetBody(0);

		body.SetActive(false);
	}

	void Update () {
		UpdateVisibility();
		UpdateMenuReticle();

		if (Input.GetMouseButtonDown(0) && buttonInFocus != null) {
			buttonInFocus.Press(this);
		}
	}

	void UpdateVisibility() {
		if (!isVisible) {
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				homeButtonDownTime = Time.realtimeSinceStartup;
			}
			else if (Input.GetKey(KeyCode.Escape) && (homeButtonDownTime > 0f && (Time.realtimeSinceStartup - homeButtonDownTime) >= longPressDelay))
			{
				// reset so something else doesn't trigger afterwards
				Input.ResetInputAxes();
				homeButtonDownTime = Time.realtimeSinceStartup;
				#if UNITY_ANDROID && !UNITY_EDITOR
				// show the platform UI
				OVRPluginEvent.Issue(RenderEventType.PlatformUI);
				#endif
			}
			else if (Input.GetKeyUp(KeyCode.Escape))
			{
				float elapsedTime = (Time.realtimeSinceStartup - homeButtonDownTime);
				if (elapsedTime < longPressDelay)
				{
					Show();
				}
				
				homeButtonDownTime = 0f;
			}
		} else {
			if (Input.GetKeyDown(KeyCode.Escape))
			{
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
