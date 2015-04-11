using UnityEngine;
using System.Collections;

public class KeyholeOpener : Interaction {

	public Door target;

	public override bool IsAbleToInteractWith (InteractiveThing thing)
	{
		return thing.name == "Keyhole";
	}

	public override void Interact() {}

	public override void InteractWith (InteractiveThing thing)
	{
		if (!IsAbleToInteractWith(thing)) {
			Debug.Log(this+" can't interact with "+thing);
			return;
		}

		thing.isAbleToInteract = false;

		StartCoroutine(OpenKeyhole(thing));
	}

	IEnumerator OpenKeyhole(InteractiveThing thing) {

		var options = new Hashtable ();
		options.Add ("ease", "easeOutCubic");

		LeanTween.move(gameObject, thing.transform.position, 2f).setEase(LeanTweenType.easeOutSine);
		LeanTween.rotate(gameObject, thing.transform.rotation.eulerAngles, 1f).setEase(LeanTweenType.easeOutSine);
		
		yield return new WaitForSeconds(2f);

		if (target != null) {
			target.SendMessage("OnKeyUse");
		} else {
			Debug.LogError(this+" doesn't have a target door");
		}

		//transform.parent.SendMessage("OnPointCollect", SendMessageOptions.DontRequireReceiver);
	}

}
