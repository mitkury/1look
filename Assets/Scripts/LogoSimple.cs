using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogoSimple : MonoBehaviour {

	public FullGameStarter gameStarter;
	public float showLogoAfterSec = 2f;

	void Start () {
		StartCoroutine(ShowLogoCo());
	}

	IEnumerator ShowLogoCo() {
		var animator = GetComponent<Animator>();
		yield return new WaitForSeconds(showLogoAfterSec);
		animator.SetTrigger("ShowLogo");
	}

	public void ActivatePlace() {
		gameStarter.ActivatePlace();
	}

}
