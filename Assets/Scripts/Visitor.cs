using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Sight))]
public class Visitor : MonoBehaviour {
	
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
	[HideInInspector]
	public Vector3 localPositionAtPrevLobby;
	public Inventory inventory { get; private set; }
	public ObtainableItem itemInHand { get; private set; }
	public Transform book;
	public Transform bookReadPoint;
	public Transform bookHidePoint;

	void Awake() {
		DontDestroyOnLoad(gameObject);
		prevPosition = transform.position;
	}
	
	void Start () {
		sight = GetComponent<Sight>();
		inventory = GetComponentInChildren<Inventory>();

		// Regular mode.
		if (Application.platform == RuntimePlatform.OSXEditor ||
		    Application.platform == RuntimePlatform.WindowsEditor || 
		    Application.platform == RuntimePlatform.WindowsPlayer || 
		    Application.platform == RuntimePlatform.OSXPlayer) {
			
			regularCameraRig.gameObject.SetActive(true);
			vrCameraRig.gameObject.SetActive(false);

			sight.anchor = regularCenterOfView;
		// VR mode.
		} else {
			regularCameraRig.gameObject.SetActive(false);
			vrCameraRig.gameObject.SetActive(true);

			sight.anchor = vrCenterOfView;
		}
	}

	void Update() {
		UpdateItemInHand();
		UpdateInteractions();
		UpdateFocusIndicator();
		UpdateBook();
		CheckObjects();

		currentSpeedPerUnit = Vector3.Distance(transform.position, prevPosition) / Time.deltaTime;
		prevPosition = transform.position;
	}

	bool isLookingBelow {
		get {
			if (sight.anchor == null)
				return false;
			
			var eulerAnhorAngle = sight.anchor.eulerAngles;
			return eulerAnhorAngle.x >= 22 && eulerAnhorAngle.x <= 120 ? true : false;
		}
	}

	bool isLookingAtBookReadPosition {
		get {
			if (sight.anchor == null)
				return false;
			
			var eulerAnhorAngle = sight.anchor.eulerAngles;
			return eulerAnhorAngle.x >= 22 && eulerAnhorAngle.x <= 120 && eulerAnhorAngle.y >= 250 && eulerAnhorAngle.y <= 330 ? true : false;
		}
	}

	void UpdateFocusIndicator() {
		var increaseFocusProgress = false;

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
					// If an InteractiveObject is able to interact on its own.
					if ((sight.target as InteractiveObject).isAbleToBeActivatedOnItsOwn) {
						increaseFocusProgress = true;
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

	void UpdateBook() {
		var targetPosition = bookHidePoint.position;

		if (isLookingAtBookReadPosition) {
			targetPosition = bookReadPoint.position;
		}

		book.position = Vector3.Lerp(book.position, targetPosition, Time.deltaTime * 3);
	}

	void UpdateItemInHand() {
		if (itemInHand == null || !itemInHand.isAbleToInteract)
			return;

		var target = sight.target;
		var targetPosition = Vector3.zero;
		var targetEulerAngle = Vector3.zero;
		var itemRigidobdy = itemInHand.GetComponent<Rigidbody>();
		var highestPointOnTarget = Vector3.zero;

		if (itemRigidobdy != null) {
			highestPointOnTarget = itemRigidobdy.ClosestPointOnBounds(itemInHand.transform.position + Vector3.up * 10);
		}

		if (isLookingBelow) {
			// Hover the object in a fixed point.

			targetPosition = waistAnchor.transform.position;

			if (itemRigidobdy != null) {
				// Hover objects of a different height on the same level when looking down.
				targetPosition = waistAnchor.transform.position - (highestPointOnTarget - itemInHand.transform.position);
			}
		
		
		} else {
			// Hover the object in front, following a visitor's head.

			var centerOfFocusPoint = sight.anchor.position + sight.facingVector * defaultDistanceToFloatingObject;
			// Finding a perpendicular vector to a facing vector (http://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html)
			var orthoToFacingVector = Vector3.Cross(sight.facingVector, Vector3.left);
			var distanceBelowFocusPoint = 0.1f;
			var belowFocusPoint = centerOfFocusPoint + orthoToFacingVector * distanceBelowFocusPoint;

			targetPosition = belowFocusPoint;

			if (itemRigidobdy != null) {
				// Align object so its highest point is matching the belowFocusPoint.
				// It makes the same distance between the reticle and a highest point on target for objects of any size.
				targetPosition = belowFocusPoint - (highestPointOnTarget - itemInHand.transform.position);
			}
		}

		itemInHand.transform.position = Vector3.Lerp(itemInHand.transform.position, targetPosition, Time.deltaTime * 3);
		itemInHand.transform.eulerAngles = targetEulerAngle;

	}

	void UpdateInteractions() {
		if (sight.target == null || !sight.target.isAbleToInteract)
			return;

		InteractiveThing target = sight.target;
		var focusTimeSec = target.hasItsOwnFocusTime ? target.focusTimeSec : defaultFocusTimeSec;

		if (itemInHand != null && itemInHand.isAbleToInteract) {
			if (target != null && target != itemInHand && sight.focusOnTargetWithoutInterruptionSec > focusTimeSec) {
				if (itemInHand.IsAbleToInteractWith(target)) {
					itemInHand.Interact(target);
					itemInHand = null;
				}
				
				if (target is InventorySpot) {
					var inventorySpot = target as InventorySpot;
					if (inventorySpot.item == null) {
						inventorySpot.Add(itemInHand);
						itemInHand = null;
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
		if (King.placeManager.currentPlace == null)
			return;
			
		var components = new List<Component>();
		// Things that are in the current place, that are able to interact and not in the hand.
		var things = King.placeManager.currentPlace.interactiveThings.FindAll(it => it.isAbleToInteract && it != itemInHand);
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

	/*
	IEnumerator CheckObjectsCo() {
		while(true) {
			CheckObjects();
			yield return new WaitForSeconds(0.1f);
		}
	}
	*/

	Vector3 prevPosition;
	float currentSpeedPerUnit;

	IEnumerator MoveToCo(Vector3 targetPosition) {
		sight.enabled = false;

		var distance = Vector3.Distance(transform.position, targetPosition);
		var time = distance / movementSpeedUnitsPerSec;

		LeanTween.move(gameObject, targetPosition, time).setOnComplete(delegate() {
			sight.enabled = true;
		});

		yield return null;

		/*
		LeanTween.cancel(gameObject);

		var startPosition = transform.position;
		var totalDistance = Vector3.Distance(transform.position, targetPosition);
		var accelerationDistance = 2f;
		var inbetweenDistance = totalDistance - accelerationDistance * 2;
		var time = inbetweenDistance / movementSpeedUnitsPerSec;
		var aTime = accelerationDistance / movementSpeedUnitsPerSec;
		var pointA = (startPosition + targetPosition) * (accelerationDistance / totalDistance);
		var pointB = (startPosition + targetPosition) * (accelerationDistance / totalDistance + 1);

		// Accelerating to pointA.
		LeanTween.move(gameObject, pointA, aTime).setEase(LeanTweenType.easeInSine).setOnComplete(delegate() {
			Debug.Log("1!");
			// Moving to pointB linearly.
			LeanTween.move(gameObject, pointB, time).setOnComplete(delegate() {
				Debug.Log("2!");
				// De-accelerating to targetPosition.
				LeanTween.move(gameObject, targetPosition, aTime).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate() {
					Debug.Log("3!");
					sight.enabled = true;
				});			 
			});
		});
		*/

		yield break;
	}

	public void Take(ObtainableItem item) {
		Debug.Log("Visitor takes "+item);
		itemInHand = item;
		item.Obtains();
		sight.ResetTarget();
	}

	public void Drop(ObtainableItem item) {
		Debug.Log("Visitor drops "+item);
		itemInHand = null;
	}

	public void MoveTo(Vector3 position) {
		StartCoroutine(MoveToCo(position));
	}

	public void MoveToExit() {
		var targetPosition = King.placeManager.currentPlace.exitPoint.transform.position;
		MoveTo(targetPosition);
	}

	public void SetOnPlace(Place place) {
		
	}
}
