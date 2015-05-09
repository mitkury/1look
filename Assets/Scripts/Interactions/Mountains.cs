using UnityEngine;
using System.Collections;

public class Mountains : MonoBehaviour {

	bool isReverse;
	bool isMoving;
	float windInitPower;

	public AudioSource windAudio;
	public WindZone windZone;
	public float windMaxPower;
	
	void Start() {
		windInitPower = windZone.windMain;
	}

	public void Switch() {
		if (isMoving) {
			windAudio.Stop();
			windZone.windMain = windInitPower;
		} else {
			//windAudio.pitch = isReverse ? -1f : 1f;
			windZone.windMain = isReverse ? -windMaxPower : windMaxPower;
			
			windAudio.PlayOneShot(windAudio.clip);
			
			isReverse = !isReverse;
		}

		isMoving = !isMoving;
	}
}
