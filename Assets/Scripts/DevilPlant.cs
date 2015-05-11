using UnityEngine;
using System.Collections;

public class DevilPlant : Interaction {

	public override void Interact () {
		StartCoroutine(CommentCo());
	}

	IEnumerator CommentCo() {
		if (GetComponent<PlaysAudioRemarkOnRadio>() != null) {
			GetComponent<PlaysAudioRemarkOnRadio>().Play(0);
			yield return new WaitForSeconds(GetComponent<PlaysAudioRemarkOnRadio>().remarks[0].audioClip.length);
			GetComponent<InteractiveObject>().isAbleToInteract = true;
		}

		yield break;
	}
}
