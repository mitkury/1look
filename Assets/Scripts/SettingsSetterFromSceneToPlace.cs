using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Place))]
[ExecuteInEditMode]
public class SettingsSetterFromSceneToPlace : MonoBehaviour {

#if UNITY_EDITOR
	void Update () {
		if (Application.isPlaying)
			return;

		GetComponent<Place>().ambientColor = RenderSettings.ambientSkyColor;
		GetComponent<Place>().fogDensity = RenderSettings.fog ? RenderSettings.fogDensity : 0f;
		GetComponent<Place>().fogColor = RenderSettings.fogColor;
	}
#endif

}
