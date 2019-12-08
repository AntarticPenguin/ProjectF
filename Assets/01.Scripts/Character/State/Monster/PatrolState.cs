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

			_character.UpdateNextPosition(newPosition);
		}
		else
		{
			TileCell destination = null;
			eDirection direction = (eDirection)Random.Range(0, 9);
			sTilePosition nextTile = _character.GetTilePosition();
			TileHelper.GetNextTilePosByDirection(direction, ref nextTile);
			//destination = GameManager.Instance.GetMap().GetTileCell(nextTile);
			destination = TileSystem.Instance.GetTileCell(nextTile);
			if (null != destination)
			{
				_character.SetDestination(destination);
			}
			else
				return;

			//update animation
			Vector2Int lookDirection = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), destination);
			_character.UpdateDirectionWithAnimation(lookDirection);
		}

		//searching for the enemy
		if (_searchingCooltime < _searchingDuration)
		{
			//var mapObjects = GameManager.Instance.GetMap().FindObjectsByRange(eMapObjectType.PLAYER,
			//	_character.GetCurrentLayer(), _character.GetCurrentTileCell(), 4);
			var mapObjects = TileSystem.Instance.FindObjectsByRange(eMapObjectType.PLAYER, _character.GetCurrentLayer(),
				_character.GetCurrentTileCell(), 4);

			if (null != mapObjects)
			{
				//타겟이 1명
				if (1 == mapObjects.Count)
				{
					_character.SetTarget(mapObjects[0]);
					_nextState = eStateType.CHASE;
					return;
				}
			}
		}
		else
		{
			return;	
		}
	}

	public override void Start()
	{
		base.Start();
		_patrolCooltime = 1.0f;
		_patrolDuration = _patrolCooltime;
		_searchingDuration = 0.0f;
		_searchingCooltime = 3.0f;
	}

	public override void Stop()
	{
		base.Stop();
	}
}
