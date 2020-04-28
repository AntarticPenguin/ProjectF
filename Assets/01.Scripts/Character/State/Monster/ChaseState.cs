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
			_nextState = eStateType.IDLE;
			return;
		}

		//player in attack range
		var mapObjects = TileSystem.Instance.FindObjectsByRange(eMapObjectType.PLAYER, eTileLayer.GROUND, _character.GetCurrentTileCell(),
			_character.GetStatus().attackRange);

		if (null != mapObjects)
		{
			if (_character.IsAttackReady())
			{
				foreach(var target in mapObjects)
				{
					_character.SetAttackInfo(new sAttackInfo(eAttackType.NORMAL, 1, _character.GetStatus().attack));
					_character.SetAttackTarget((Character)target);
					_nextState = eStateType.ATTACK;
					break;
				}
				
			}
		}

		//위치 보정작업
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
			_character.UpdatePosition(newPosition);
		}

		_pathfinder.MakePathToTarget(_character.GetPathTarget());
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
		_pathfinder.MakePathToTarget(_character.GetPathTarget());
	}

	public override void Stop()
	{
		base.Stop();

		_character.ResetPathTarget();
		_character.ResetPath();

		//test
		TileSystem.Instance.ResetAllColor();
		_character.GetSpriteRenderer().color = Color.white;
	}
}
