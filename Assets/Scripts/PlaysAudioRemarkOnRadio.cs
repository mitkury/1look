using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioRemarkOnRadioData {
	public AudioClip audioClip;
	public float volume = 1f;
}

public class PlaysAudioRemarkOnRadio : MonoBehaviour {
	
	public List<AudioRemarkOnRadioData> remarks;

	public void Play(int index) {
		RadioPrototype.PlayRemark(remarks[index].audioClip, remarks[index].volume);
	}

}
