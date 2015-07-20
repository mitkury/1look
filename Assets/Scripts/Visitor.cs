using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Sight))]
public class Visitor : MonoBehaviour {

	List<Camera> cameras = new List<Camera>();
	PlaysSoundOnRequest soundPlayer;

	public Transform vrCameraRig;
	public Transform vrCenterOfView;
	public Transform regularCameraRig;
	public Transform regularCenterOfView;
	[HideInInspector]
	public Sight sight;
	public float movementSpeedUnitsPerSec = 1;
	public float defaultDistanceToFloatingObject = 1f;
	public float defaultFocusTimeSec = 1f;
	public Transform waistAnchor;
	public Transform faceAnchor;
	public Transform earsAnchor;
	[HideInInspector]
	public Vector3 localPositionAtPrevLobby;
	public Inventory inventory { get; private set; }
	public ObtainableItem itemInHand { get; private set; }
	public ScreenFader screenFader;


	void Awake() {
		DontDestroyOnLoad(gameObject);
		//prevPosition = transform.position;

		sight = GetComponent<Sight>();
		inventory = GetComponentInChildren<Inventory>();
	}
	
	void Start () {


		//regularCameraRig.gameObject.SetActive(false);
		//vrCameraRig.gameObject.SetActive(false);

		// VR mode.
		if (King.isInVRMode) {
			regularCameraRig.gameObject.SetActive(false);

			if (!vrCameraRig.gameObject.activeSelf)
				vrCameraRig.gameObject.SetActive(true);
			
			sight.anchor = vrCenterOfView;
			cameras.Add(vrCameraRig.GetComponent<OVRCameraRig>().leftEyeCamera);
			cameras.Add(vrCameraRig.GetComponent<OVRCameraRig>().rightEyeCamera);
		} 
		// Regular mode.
		else {
			regularCameraRig.gameObject.SetActive(true);
			vrCameraRig.gameObject.SetActive(false);
			
			sight.anchor = regularCenterOfView;
			cameras.Add(regularCameraRig.GetComponentInChildren<Camera>());
		}



		// Using Unity's native VR.
		/*
		regularCameraRig.gameObject.SetActive(true);
		vrCameraRig.gameObject.SetActive(false);
		
		sight.anchor = regularCenterOfView;
		cameras.Add(regularCameraRig.GetComponentInChildren<Camera>());

		if (King.isInVRMode)
			regularCameraRig.GetComponent<MouseCameraControl>().enabled = false;
		else
			regularCameraRig.GetComponent<MouseCameraControl>().enabled = true;
		*/

		soundPlayer = GetComponent<PlaysSoundOnRequest>();

		// Prepare an object for fading main camera in/out.
		//var blinkTextureGO = iTween.CameraFadeAdd();
		//blinkTextureGO.transform.parent = transform;

		screenFader.anchor = sight.anchor;
	}

	void Update() {
		UpdateItemInHand();
		UpdateInteractions();
		UpdateFocusIndicator();
		CheckObjects();
		//prevPosition = transform.position;

		#if UNITY_STANDALONE
		if (Input.GetKeyDown(KeyCode.Escape)) {
			StartCoroutine(RunDemoClosingCo());
		}
		#endif
	}

	IEnumerator RunDemoClosingCo() {
		screenFader.FadeIn(2f);
		yield return new WaitForSeconds(2f);

		LeanTween.value(gameObject, delegate(float value) { 
			AudioListener.volume = value;
		}, 1f, 0f, 3f);
	}

	void FadeIn()
	{
		//iTween.CameraFadeFrom(1.0f, 1.0f);
		//iTween.CameraFadeTo(0f, 1.0f);
	}

	void FadeOut()
	{
		//iTween.CameraFadeTo(1f, 1.0f);
	}

	void UpdateFocusIndicator() {
		var increaseFocusProgress = false;

		if (!sight.enabled)
			return;

		if (sight.target != null && sight.target.isAbleToInteract) {
			if (itemInHand != null) {
				if (itemInHand.IsAbleToInteractWith(sight.target)) {
					increaseFocusProgress = true;
				} else if (sight.target is InventorySpot) {
					increaseFocusProgress = true;
				}
			} else {
				if (sight.target is ObtainableItem) {
					increaseFocusProgress = true;
				} else if (sight.target is InteractiveObject) {
					var iob = sight.target as InteractiveObject;
					// If an InteractiveObject is able to interact on its own.
					if (iob.isAbleToBeActivatedOnItsOwn) {
						increaseFocusProgress = true;
						// Don't increase if focus time = 0;
						if (iob.hasItsOwnFocusTime && iob.focusTimeSec == 0)
							increaseFocusProgress = false;
					}
				}
			}
		}

		if (increaseFocusProgress) {
			var focusTimeSec = sight.target.hasItsOwnFocusTime ? sight.target.focusTimeSec : defaultFocusTimeSec;
			sight.focusOnTargetAlpha = sight.focusOnTargetWithoutInterruptionSec / focusTimeSec;

		} else {
			sight.focusOnTargetAlpha = 0;
		}
	}

	void UpdateItemInHand() {
		if (itemInHand == null || !itemInHand.isAbleToInteract)
			return;

		var target = sight.target;
		var targetPosition = Vector3.zero;
		var targetRotation = itemInHand.transform.rotation;
		var targetEulerAngle = Vector3.zero;
		var itemRigidobdy = itemInHand.GetComponent<Rigidbody>();
		var highestPointOnTarget = Vector3.zero;
		var distanceToItem = Vector3.Distance(itemInHand.transform.position, sight.anchor.position);

		if (itemRigidobdy != null) {
			highestPointOnTarget = itemRigidobdy.ClosestPointOnBounds(itemInHand.transform.position + Vector3.up * 10);
			//Debug.DrawLine(highestPointOnTarget, highestPointOnTarget + Vector3.up, Color.red, Time.deltaTime);
		}

		var positionChangeRate = 1.5f;
		var rotationChangeRate = 1.5f;

		if (distanceToItem < 2f) {
			positionChangeRate = 3f;
			//rotationChangeRate = 3f;
		}

		if (isLookingBelow) {
			// Hover the object in a fixed point.

			targetPosition = waistAnchor.transform.position;

			if (itemRigidobdy != null) {
				// Hover objects of a different height on the same level when looking down.
				targetPosition = waistAnchor.transform.position - (highestPointOnTarget - itemInHand.transform.position);
			}
		
			if (distanceToItem < 2f) {
				if (itemInHand.customOnGrabRotation != Vector3.zero) {
					targetRotation = Quaternion.Euler(itemInHand.customOnGrabRotation);
				} else {
					targetRotation = Quaternion.identity;
				}
			}
		} else {
			// Hover the object in front, following a visitor's head.

			var distanceToFloatingObject = itemInHand.hasItsOwnOnGrabDistance ? itemInHand.onGrabDistance : defaultDistanceToFloatingObject;
			var centerOfFocusPoint = sight.anchor.position + sight.facingVector * distanceToFloatingObject;
			// Finding a perpendicular vector to a facing vector (http://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html)
			var orthoToFacingVector = Vector3.Cross(sight.facingVector, -sight.anchor.right);
			var distanceBelowFocusPoint = 0.1f;
			var belowFocusPoint = centerOfFocusPoint + orthoToFacingVector * distanceBelowFocusPoint;

			targetPosition = belowFocusPoint;

			if (itemRigidobdy != null) {
				// Align object so its highest point is matching the belowFocusPoint.
				// It makes the same distance between the reticle and a highest point on target for objects of any size.
				targetPosition = belowFocusPoint - (highestPointOnTarget - itemInHand.transform.position);
			}

			if (distanceToItem < 2f) {
				if (itemInHand.customOnGrabRotation != Vector3.zero) {
					targetRotation = Quaternion.Euler(itemInHand.customOnGrabRotation) * Quaternion.LookRotation(sight.anchor.forward);
				} else {
					targetRotation = Quaternion.LookRotation(sight.anchor.forward);
				}
			}
		}

		itemInHand.transform.position = Vector3.Lerp(itemInHand.transform.position, targetPosition, Time.deltaTime * positionChangeRate);
		itemInHand.transform.rotation = Quaternion.Lerp(itemInHand.transform.rotation, targetRotation, Time.deltaTime * rotationChangeRate);
	}

	void UpdateInteractions() {
		if (!sight.enabled || sight.target == null || !sight.target.isAbleToInteract)
			return;

		InteractiveThing target = sight.target;
		var focusTimeSec = target.hasItsOwnFocusTime ? target.focusTimeSec : defaultFocusTimeSec;

		if (itemInHand != null && itemInHand.isAbleToInteract) {
			if (target != null && target != itemInHand && sight.focusOnTargetWithoutInterruptionSec > focusTimeSec) {
				if (itemInHand.IsAbleToInteractWith(target)) {
					itemInHand.Interact(target);
					/*
					itemInHand = null;
					sight.reticle.SetBody(0);
					sight.ResetTarget();
					*/
					Drop(itemInHand);
				}
				
				if (target is InventorySpot) {
					var inventorySpot = target as InventorySpot;
					if (inventorySpot.item == null) {
						inventorySpot.Add(itemInHand);
						/*
						itemInHand = null;
						sight.reticle.SetBody(0);
						sight.ResetTarget();
						*/
						Drop(itemInHand);
					}
				}
			}
		} else if (itemInHand == null) {
			if (target != null && sight.focusOnTargetWithoutInterruptionSec > focusTimeSec) {
				if (target is ObtainableItem) {
					Take(target as ObtainableItem);
				} else if (target is InteractiveObject) {
					var interactiveObject = target as InteractiveObject;
					if (interactiveObject.isAbleToBeActivatedOnItsOwn) {
						//Debug.Log("Visitor activates "+target);
						interactiveObject.Activate();
					}
				}
			}
		}
	}

	void CheckObjects() {
		if (!sight.enabled)
			return;

		if (King.Instance == null || King.placeManager.currentPlace == null || !King.placeManager.currentPlace.gameObject.activeSelf)
			return;
			
		var components = new List<Component>();
		// Things that are in the current place, that are able to interact and not in the hand.
		var things = King.placeManager.currentPlace.interactiveThings.FindAll(it => it.isAbleToInteract && it != itemInHand);

		if (itemInHand != null) {
			// Things that're able to interact with the item in hand.
			things = things.FindAll(it => itemInHand.IsAbleToInteractWith(it));
		}

		// Cast things to the base class—Component.
		components.AddRange(things.Cast<Component>().ToList());
		
		if (inventory != null) {
			if (inventory.items != null && inventory.items.Count > 0) {
				// Add items that are currently in the inventory.
				// Cast them to the base class—Component.
				components.AddRange(inventory.items.Cast<Component>().ToList());
			}
			
			// Add the inventory body.
			components.Add(inventory.body);
		}
		
		sight.SetReticleVisibility(components);
	}
	
	IEnumerator SetBackgroundCo(Color color) {
		while(cameras.Count == 0)
			yield return null;

		foreach (Camera camera in cameras) {
			camera.backgroundColor = color;
		}
	}

	/*
	IEnumerator CheckObjectsCo() {
		while(true) {
			CheckObjects();
			yield return new WaitForSeconds(0.1f);
		}
	}
	*/

	//Vector3 prevPosition;
	//float currentSpeedPerUnit;

	/*
	IEnumerator MoveToCo(Vector3 targetPosition) {
		sight.enabled = false;

		var distance = Vector3.Distance(transform.position, targetPosition);
		var time = distance / movementSpeedUnitsPerSec;

		LeanTween.move(gameObject, targetPosition, time).setOnComplete(delegate() {
			sight.enabled = true;
		});

		yield break;
	}
	*/

	public bool isLookingBelow {
		get {
			if (!sight.enabled)
				return false;

			if (sight.anchor == null)
				return false;
			
			var eulerAnhorAngle = sight.anchor.eulerAngles;
			return eulerAnhorAngle.x >= 22 && eulerAnhorAngle.x <= 120 ? true : false;
		}
	}
	
	public bool isLookingAtBookReadPosition {
		get {
			if (!sight.enabled)
				return false;

			if (sight.anchor == null)
				return false;
			
			var eulerAnhorAngle = sight.anchor.eulerAngles;
			return eulerAnhorAngle.x >= 22 && eulerAnhorAngle.x <= 120 && eulerAnhorAngle.y >= 288 && eulerAnhorAngle.y <= 340 ? true : false;
		}
	}

	public void SetBackground(Color color) {
		StartCoroutine(SetBackgroundCo(color));
	}

	public void Take(ObtainableItem item) {
		Debug.Log("Visitor takes "+item);
		itemInHand = item;
		item.Obtains();
		sight.ResetTarget();
		sight.reticle.SetBody(1);

		if (soundPlayer != null) {
			soundPlayer.PlayOneShot(0);
		}
	}

	public void Drop(ObtainableItem item) {
		Debug.Log("Visitor drops "+item);
		itemInHand.Frees();
		itemInHand = null;
		sight.ResetTarget();
		sight.reticle.SetBody(0);
	}

	/*
	public void MoveTo(Vector3 position) {
		StartCoroutine(MoveToCo(position));
	}

	public void MoveToExit() {
		var targetPosition = King.placeManager.currentPlace.exitPoint.transform.position;
		MoveTo(targetPosition);
	}
	*/
}
