using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
	private void Awake()
	{
		SetMapObjectType(eMapObjectType.PLAYER);
	}

	void Start()
	{
		
	}

	public override void InitState()
	{
		base.InitState();
		_curState = _stateMap[eStateType.IDLE];
	}
}
