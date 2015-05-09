using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObtainableItem))] 
[RequireComponent(typeof(Animator))]
public class PlaysAnimationWhenObtained : MonoBehaviour {

	public void OnItemTakeByVisitor(ObtainableItem item) {
		var _animator = GetComponent<Animator>();


	}

}
