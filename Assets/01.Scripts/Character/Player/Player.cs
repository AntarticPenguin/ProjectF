using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	void Start()
	{
		_objectType = eMapObjectType.PLAYER;
	}

	public override void InitState()
	{
		base.InitState();
		ReplaceState(eStateType.IDLE, new PlayerIdleState());

		_curState = _stateMap[eStateType.IDLE];
	}
}
