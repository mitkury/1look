using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnterExitSensor : MonoBehaviour {

	public List<string> registerNamesOrTags = new List<string>();
	public bool triggersEnter = true;
	public bool triggersExit = true;
	public List<GameObject> receivers = new List<GameObject>();

	void Start() {
		receivers.Remove(gameObject);
		receivers.Add(gameObject);
	}

	void OnTriggerEnter(Collider other) {
		if (triggersEnter && registerNamesOrTags.Find(s => s == other.tag || s == other.name) != null /*other.tag == "Player"*/) {
			foreach (GameObject receiver in receivers) {
				receiver.SendMessage("OnSensorEnter", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (triggersExit && registerNamesOrTags.Find(s => s == other.tag || s == other.name) != null /*other.tag == "Player"*/) {
			foreach (GameObject receiver in receivers) {
				receiver.SendMessage("OnSensorExit", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
