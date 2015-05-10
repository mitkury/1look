using UnityEngine;
using System.Collections;

public class CauldronUser : Interaction {

	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		return (thing is Cauldron);
	}

	public override void InteractWith (InteractiveThing thing) {
		var cauldron = thing as Cauldron;

		cauldron.Add(GetComponent<ObtainableItem>());
	}

}
