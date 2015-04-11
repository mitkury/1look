using UnityEngine;
using System.Collections;

public interface IState
{
	void OnEnterState();

	void OnExitState();

	void Update();
}

