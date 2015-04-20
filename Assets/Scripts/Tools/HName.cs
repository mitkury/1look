using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public static class HName {

	public static string GetPure(string name) {
		// Remove any digits at the end, trim empty stuff, set to lower case.
		return Regex.Replace(name, @"[\d-]", string.Empty).Trim().ToLower();
	}

}
