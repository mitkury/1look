using UnityEngine;
using System.Collections;

public static class HComponent {

	public static T Create<T>() where T : Component {
		return HComponent.Create<T>("GameObject");
	}
	
	public static T Create<T>(string name) where T : Component {
		GameObject gameObject = new GameObject(name);
		
		// In case if ask for a 'standard' component such as Transform.
		T component = gameObject.GetComponent<T>();
		
		// If it's not a 'standard' component then add requared one to the gameObject.
		component = component != null ? component : gameObject.AddComponent<T>();
		
		return component;
	}
}
