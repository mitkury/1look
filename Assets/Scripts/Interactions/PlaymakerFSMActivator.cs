using UnityEngine;
using System.Collections;

public class PlaymakerFSMActivator : Interaction {

	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		return false;
	}
	
	public override void Interact() {
		/*
		PlayMakerFSM playmakerFSM = GetComponent<PlayMakerFSM>();
		if (playmakerFSM != null) {
			playmakerFSM.SendEvent("Activate");
		}
		*/
	}
	
	public override void InteractWith (InteractiveThing thing) {
	}

}
