using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMoveState : State
{
	Stack<TileCell> _pathStack;
	Vector2Int _direction;
	TileCell _pathTargetCell;

	Vector2Int _saveDirection;
	Vector2 _prevPosition;
	float _stuckCheckDuration;
	float _stuckCheckTime;
	bool _bWasStucked;

	public override void Update()
	{
		base.Update();
		_stuckCheckDuration += Time.deltaTime;
		_prevPosition = _character.GetTransform().position;
		
		bool bIsArrived = (_character.GetTransform().position.x.EqualApproximately(_pathTargetCell.GetPosition().x, 0.1f) &&
							_character.GetTransform().position.y.EqualApproximately(_pathTargetCell.GetPosition().y, 0.1f));
		if (bIsArrived)
		{
			if (0 == _pathStack.Count)
			{
				_nextState = eStateType.IDLE;
				return;
			}

			_pathTargetCell = _pathStack.Pop();
			_direction = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), _pathTargetCell);
		}

		if (_bWasStucked)
		{
			_direction = _saveDirection;
			_bWasStucked = false;
		}

		if (_prevPosition.Equals(_character.GetTransform().position) &&
			_stuckCheckDuration > _stuckCheckTime)
		{
			ChangeDirection();
			_bWasStucked = true;
			_stuckCheckDuration = 0.0f;
		}

		Vector2 newPosition = TileHelper.GetSlopeDirection(_direction);
		_character.UpdateDirectionWithAnimation(_direction);
		_character.UpdatePosition(newPosition);
	}

	public override void Start()
	{
		base.Start();

		_pathStack = _character.GetPathStack();
		if (0 != _pathStack.Count)
		{
			_pathTargetCell = _pathStack.Pop();
			_direction = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), _pathTargetCell);
		}
		_prevPosition = Vector2.zero;
		_stuckCheckDuration = 0.0f;
		_stuckCheckTime = 1.5f;
		_bWasStucked = false;
	}

	public override void Stop()
	{
		base.Stop();
		Debug.Log("Arrived");

		_character.ResetPath();
		_character.ResetDestination();
	}

	void ChangeDirection()
	{
		TileSystem tileSystem = TileSystem.Instance;
		sTilePosition checkTilePos = _character.GetCurrentTileCell().GetTilePosition();
		eDirection direction = TileHelper.ConvertToeDirection(_direction);

		_saveDirection = _direction;

		Debug.Log("STUCK: " + _character.GetCurrentTileCell().GetTilePosition().ToString());

		switch (direction)
		{
			case eDirection.NORTH:
				{
					if (!tileSystem.CanMoveTileCell(checkTilePos.tileX, checkTilePos.tileY + 1)) //NORTH WEST
						_direction = new Vector2Int(1, 0);
					else if (!tileSystem.CanMoveTileCell(checkTilePos.tileX + 1, checkTilePos.tileY)) //NORTH EAST
						_direction = new Vector2Int(-1, 0);
				}
				break;
			case eDirection.SOUTH:
				{
					if (!tileSystem.CanMoveTileCell(checkTilePos.tileX - 1, checkTilePos.tileY)) //SOUTH WEST
						_direction = new Vector2Int(1, 0);
					else if (!tileSystem.CanMoveTileCell(checkTilePos.tileX, checkTilePos.tileY - 1)) //SOUTH EAST
						_direction = new Vector2Int(-1, 0);
				}
				break;
			case eDirection.EAST:
				{
					if (!tileSystem.CanMoveTileCell(checkTilePos.tileX + 1, checkTilePos.tileY)) //NORTH EAST
						_direction = new Vector2Int(0, -1);
					else if (!tileSystem.CanMoveTileCell(checkTilePos.tileX, checkTilePos.tileY - 1)) //SOUTH EAST
						_direction = new Vector2Int(0, 1);
				}
				break;
			case eDirection.WEST:
				{
					if (!tileSystem.CanMoveTileCell(checkTilePos.tileX, checkTilePos.tileY + 1)) //NORTH WEST
						_direction = new Vector2Int(0, -1);
					else if (!tileSystem.CanMoveTileCell(checkTilePos.tileX - 1, checkTilePos.tileY)) //SOUTH WEST
						_direction = new Vector2Int(0, 1);
				}
				break;
			default:
				break;
		}
	}
}
