using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
	private void Awake()
	{
		SetMapObjectType(eMapObjectType.ENEMY);
	}

	private void LateUpdate()
	{
		GetComponentInChildren<Text>().text = GetCurStateType().ToString();
	}

	public override void InitState()
	{
		base.InitState();

		ReplaceState(eStateType.IDLE, new PatrolState());
		ReplaceState(eStateType.CHASE, new ChaseState());
		_curState = _stateMap[eStateType.IDLE];
	}

	public override void InitStatus()
	{
		base.InitStatus();

		_status.speed = 1.0f;
		_status.attackRange = 1;
		_attackCoolTime = 8.0f;
	}
}
