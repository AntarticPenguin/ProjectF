using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
	float _patrolCooltime;
	float _patrolDuration;

	public override void Update()
	{
		base.Update();

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
			Vector2 position = Vector2.zero;
			position.x += lookDirection.x;
			position.y += lookDirection.y;
			position = _character.GetStatus().speed * position.normalized * Time.deltaTime;
			_character.UpdateNextPosition(position + (Vector2)_character.GetTransform().position);
		}
		else
		{
			TileMap map = GameManager.Instance.GetMap();
			TileCell destination = null;
			TileCell curTileCell = _character.GetCurrentTileCell();
			var mapObjects  = map.FindObjectsByRange(eMapObjectType.PLAYER, _character.GetCurrentLayer(), curTileCell, 2);
			if(null != mapObjects)
			{
				//타겟이 1명
				if(1 == mapObjects.Count)
				{
					_character.SetTarget(mapObjects[0]);
					destination = mapObjects[0].GetCurrentTileCell();
				}
			}
			else
			{
				eDirection direction = (eDirection)Random.Range(0, 9);
				sTilePosition nextTile = _character.GetTilePosition();
				TileHelper.GetNextTilePosByDirection(direction, ref nextTile);
				destination = GameManager.Instance.GetMap().GetTileCell(nextTile);
				if (null != destination)
				{
					_character.SetDestination(destination);
				}
				else
					return;
			}

			//update animation
			Vector2Int lookDirection = TileHelper.GetDirectionVector(curTileCell, destination);
			_character.UpdateDirectionWithAnimation(lookDirection);

			if (_character.HasTarget())
				_nextState = eStateType.CHASE;

			//Debug.Log("<color=red>Start Move</color>");
		}
	}

	public override void Start()
	{
		base.Start();
		_patrolCooltime = 1.0f;
		_patrolDuration = _patrolCooltime;
	}

	public override void Stop()
	{
		base.Stop();
	}
}
