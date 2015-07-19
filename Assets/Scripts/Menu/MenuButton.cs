using UnityEngine;
using System.Collections;

public class MenuButton : InteractiveThing {

	public enum FunctionalityState {
		Continue,
		Restart,
		Quit
	}

	public FunctionalityState functionality;

	public void Press(Menu menu) {
		switch (functionality) {
		case FunctionalityState.Continue:
			menu.Hide();
			break;
		case FunctionalityState.Restart:
			menu.RestartGame();
			break;
		case FunctionalityState.Quit:
			menu.QuiteGame();
			break;
		}
	}
}
