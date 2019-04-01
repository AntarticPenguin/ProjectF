using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	private void Awake()
	{
		_objectType = eMapObjectType.PLAYER;
	}

	void Start()
	{
		
	}

	public override void InitState()
	{
		base.InitState();
		ReplaceState(eStateType.IDLE, new PlayerIdleState());

		_curState = _stateMap[eStateType.IDLE];
	}


}
