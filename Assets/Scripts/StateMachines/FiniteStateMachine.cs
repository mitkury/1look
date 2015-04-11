using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FiniteStateMachine : MonoBehaviour {

	private IState _currentState;

	public IState currentState {
		get {
			return _currentState;
		}
		set {
			if(_currentState != null)
				_currentState.OnExitState();
			
			_currentState = value;
			_currentState.OnEnterState();
		}
	}

	void Update() {
		currentState.Update();
	}
}
