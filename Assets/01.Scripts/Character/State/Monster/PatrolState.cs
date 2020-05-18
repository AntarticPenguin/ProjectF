using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
	float _patrolCooltime;
	float _patrolDuration;
	float _searchingDuration;
	float _searchingCooltime;

	public override void Update()
	{
		base.Update();

		_searchingDuration += Time.deltaTime;

		if (_character.HasDestination())
		{
			_patrolDuration -= Time.deltaTime;
			if (_patrolDuration < 0.0f)
			{
				_character.ResetDestination();
				_patrolDuration = _patrolCooltime;
				return;
			}

			Vector2 lookDirection = _character.GetLookDirection();
			Vector2 newPosition = TileHelper.GetSlopeDirection(lookDirection);
			newPosition.x += lookDirection.x;
			newPosition.y += lookDirection.y;

			_character.UpdatePosition(newPosition);
		}
		else
		{
			TileCell destination = null;
			eDirection direction = (eDirection)Random.Range(0, (int)eDirection.MAX);
			sTilePosition nextTile = _character.GetTilePosition();
			TileHelper.GetNextTilePosByDirection(direction, ref nextTile);
			destination = TileSystem.Instance.GetTileCell(nextTile);
			if (null != destination)
			{
				_character.SetDestination(destination);
			}
			else
				return;

			//update animation
			Vector2Int lookDirection = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), destination);
			_character.UpdateDirection(lookDirection);
		}

		if (_searchingCooltime < _searchingDuration)
		{
			var mapObjects = TileSystem.Instance.FindObjectsByRange(eMapObjectType.PLAYER, _character.GetCurrentLayer(),
				_character.GetCurrentTileCell(), 4);

			if (null != mapObjects)
			{
				foreach(var target in mapObjects)
				{
					_character.SetPathTarget(target);
					_nextState = eStateType.CHASE;
					return;
				}
			}
			_searchingDuration = 0.0f;
		}
	}

	public override void Start()
	{
		base.Start();
		_patrolCooltime = 1.0f;
		_patrolDuration = _patrolCooltime;
		_searchingDuration = 0.0f;
		_searchingCooltime = 2.0f;

		_character.GetAnimPlayer().Play(_character.LookAt().ToString());
	}

	public override void Stop()
	{
		base.Stop();
	}
}
