using UnityEngine;
using System.Collections;

public class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T instance;

	public static T Instance {
		get {
			if (instance == null) {
				instance = (T) FindObjectOfType(typeof(T));
			}
		
			return instance;
		}
	}
	
}