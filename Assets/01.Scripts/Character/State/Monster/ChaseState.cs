using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
	Pathfinding _pathfinder = new Pathfinding();
	float _chaseOverTime;
	bool _bIsCenter;

	public override void Update()
	{
		base.Update();

		_chaseOverTime -= Time.deltaTime;
		if (_chaseOverTime < 0.0f)
		{
			Debug.Log("Stop Chasing!!");
			_nextState = eStateType.IDLE;
			return;
		}

		if (!_bIsCenter)
		{
			Vector2 charPos = _character.GetTransform().position;
			Vector2 tilePos = _character.GetCurrentTileCell().GetPosition();
			float diffX = tilePos.x - charPos.x;
			float diffY = tilePos.y - charPos.y;

			if (diffX.EqualApproximately(0.05f, 0.1f) || diffY.EqualApproximately(0.05f, 0.1f))
			{
				_bIsCenter = true;
				return;
			}
			Vector2 newPosition = new Vector2(diffX, diffY);
			_character.UpdateNextPosition(newPosition);
		}

		bool finishMove = _pathfinder.Move();
		if (finishMove)
			_nextState = eStateType.IDLE;
	}

	public override void Start()
	{
		base.Start();

		_character.GetSpriteRenderer().color = Color.red;
		_chaseOverTime = 5.0f;

		Vector2 curtilecell = _character.GetCurrentTileCell().GetPosition();
		_bIsCenter = (_character.GetTransform().position.x.EqualApproximately(curtilecell.x, 0.1f) &&
						_character.GetTransform().position.y.EqualApproximately(curtilecell.y, 0.1f));

		_pathfinder.Init(_character);
		_pathfinder.MakePathToTarget(_character.GetTarget());
	}

	public override void Stop()
	{
		base.Stop();

		_character.ResetTarget();
		_character.ResetPath();
		GameManager.Instance.GetMap().ResetAllColor();
		_character.GetSpriteRenderer().color = Color.white;
	}
}
