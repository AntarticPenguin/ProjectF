using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
	MonsterSpawner _spawner;

	private void Awake()
	{
		SetMapObjectType(eMapObjectType.ENEMY);
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

	public void SetSpawner(MonsterSpawner spawner) { _spawner = spawner; }

	public override void Kill()
	{
		_spawner.onKillEnemyCallback?.Invoke();
		base.Kill();
	}
}
