using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
	Pathfinding _pathfinder = new Pathfinding();
	float _chaseOverTime;

	public override void Update()
	{
		base.Update();

		//_chaseOverTime -= Time.deltaTime;
		//if (_chaseOverTime < 0.0f)
		//	_nextState = eStateType.IDLE;

		bool finishMove = _pathfinder.Move();
		if (finishMove)
		{
			Debug.Log("MOVE FINISH!!");
			_nextState = eStateType.IDLE;
		}
	}

	public override void Start()
	{
		base.Start();

		_character.GetSpriteRenderer().color = Color.red;
		_chaseOverTime = 5.0f;

		_pathfinder.Init(_character);
		_pathfinder.MakePathToTarget(_character.GetTarget());
		Debug.Log("target:" + _character.GetTarget().name);
	}

	public override void Stop()
	{
		base.Stop();

		Debug.Log("Chase Finish. Reset taregt and path");
		_character.ResetTarget();
		_character.ResetPath();
		GameManager.Instance.GetMap().ResetAllColor();
		_character.GetSpriteRenderer().color = Color.white;
	}
}
