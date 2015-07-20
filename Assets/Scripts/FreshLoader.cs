using UnityEngine;
using System.Collections;

public class FreshLoader : MonoBehaviour {
	
	public string sceneName = "Game";
	public GameObject cameraRig;

	void Awake () {
		StartCoroutine(StartGameCo());
	}

	IEnumerator StartGameCo() {

		// If the king in in place it means that the game was restarted. Remove the king and the visitor in that case.
		if (King.Instance != null) {
			Destroy(King.Instance.gameObject);

			if (King.visitor != null) {
				cameraRig.SetActive(false);

				Destroy(King.visitor.gameObject);
			}

			yield return new WaitForEndOfFrame();
		}

		if (!cameraRig.activeSelf)
			cameraRig.SetActive(true);

		yield return new WaitForSeconds(0.5f);

		AudioListener.volume = 1f;

		var prevLoadingPriority = Application.backgroundLoadingPriority;
		Application.backgroundLoadingPriority = ThreadPriority.High;
		
		Application.LoadLevel(sceneName);
		
		Application.backgroundLoadingPriority = prevLoadingPriority;
	}
}
