using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	public string sceneName;

	void Start () {
		var prevLoadingPriority = Application.backgroundLoadingPriority;
		Application.backgroundLoadingPriority = ThreadPriority.High;

		Application.LoadLevel(sceneName);

		Application.backgroundLoadingPriority = prevLoadingPriority;
	}
}
