using UnityEngine;
using System.Collections;

public class SurfaceUser : Interaction {
	
	public override bool IsAbleToInteractWith (InteractiveThing thing) {
		return (thing is SurfaceForItems);
	}
	
	public override void InteractWith (InteractiveThing thing) {
		var surface = thing as SurfaceForItems;
		
		surface.AddItem(GetComponent<ObtainableItem>());
	}
	
}