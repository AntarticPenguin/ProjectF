using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	private void Awake()
	{
		SetMapObjectType(eMapObjectType.ENEMY);
	}

	void Start()
	{
		
	}

	public override void InitState()
	{
		base.InitState();

		ReplaceState(eStateType.IDLE, new PatrolState());
		ReplaceState(eStateType.CHASE, new ChaseState());
		_curState = _stateMap[eStateType.IDLE];

		_curState.Start();
	}

	public override void InitStatus()
	{
		base.InitStatus();

		_status.speed = 1.0f;
	}
}
