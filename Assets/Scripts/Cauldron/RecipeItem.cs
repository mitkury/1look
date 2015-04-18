using UnityEngine;
using System.Collections;

[System.Serializable]
public class RecipeItem {

	public string itemName;
	public int amount = 1;
	[HideInInspector]
	public int collected;

}
