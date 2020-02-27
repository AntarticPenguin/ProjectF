using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
	struct sPathCommand
	{
		public TileCell tileCell;
		public float heuristic;
	}

	Character _character;

	public void Init(Character character)
	{
		_character = character;
	}

	public void MakePathToTarget(TileCell destination)
	{
		//GameManager.Instance.GetMap().ResetPathfindInfo();
		//GameManager.Instance.GetMap().ResetAllColor();
		TileSystem.Instance.ResetPathfindInfo();
		TileSystem.Instance.ResetAllColor();
		_pathfindingQueue.Clear();

		_targetTileCell = destination;

		sPathCommand command = new sPathCommand();
		command.tileCell = _character.GetCurrentTileCell();
		command.heuristic = 0.0f;

		PushCommand(command);

		FindPath();
		BuildPath();
		InitMove();
	}

	public void MakePathToTarget(MapObject target)
	{
		MakePathToTarget(target.GetCurrentTileCell());
	}

	#region PATHFIND ALGORITHM
	List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();
	TileCell _targetTileCell;
	TileCell _reverseTileCell;

	void PushCommand(sPathCommand command)
	{
		_pathfindingQueue.Add(command);
		_pathfindingQueue.Sort((sPathCommand lhs, sPathCommand rhs) => lhs.heuristic.CompareTo(rhs.heuristic));
	}

	void FindPath()
	{
		TileSystem tileSystem = TileSystem.Instance;
		while (0 != _pathfindingQueue.Count)
		{
			//제일 앞 타일을 하나 꺼낸다.
			sPathCommand command = _pathfindingQueue[0];
			_pathfindingQueue.RemoveAt(0);

			//방문한 타일인가?
			if (false == command.tileCell.IsVisit())
			{
				command.tileCell.Visit();

				if (_targetTileCell.GetTilePosition().Equals(command.tileCell.GetTilePosition()))
				{
					_reverseTileCell = _targetTileCell;
					return;
				}

				for (int direction = 0; direction < 8; direction++)
				{
					sTilePosition nextTilePos = new sTilePosition(command.tileCell.GetTileX(), command.tileCell.GetTileY());
					TileHelper.GetNextTilePosByDirection((eDirection)direction, ref nextTilePos);
					TileCell nextTileCell = tileSystem.GetTileCell(nextTilePos);

					if (((true == tileSystem.CanMoveTileCell(nextTilePos.tileX, nextTilePos.tileY)) && false == nextTileCell.IsVisit()) ||
						(nextTilePos.tileX == _targetTileCell.GetTileX() && nextTilePos.tileY == _targetTileCell.GetTileY()))
					{
						float newDistanceFromStart = command.tileCell.GetDistanceFromStart() + command.tileCell.GetDistanceWeight();
						float newHeuristic = CalcAstarHeuristic(newDistanceFromStart, nextTileCell, _targetTileCell);

						if (null == nextTileCell.GetPrevCell())
						{
							nextTileCell.SetDistanceFromStart(newDistanceFromStart);
							nextTileCell.SetPrevCell(command.tileCell);     //이전 타일 기억

							sPathCommand newCommand = new sPathCommand();
							newCommand.heuristic = newHeuristic;
							newCommand.tileCell = nextTileCell;
							PushCommand(newCommand);

							//nextTileCell.DrawColor(Color.blue);
						}
					}
				}
			}
		}
	}

	float CalcEuclideanDistance(TileCell tileCell, TileCell targetCell)
	{
		int distanceW = targetCell.GetTileX() - tileCell.GetTileX();
		int distanceH = targetCell.GetTileY() - tileCell.GetTileY();

		distanceW *= distanceW;
		distanceH *= distanceH;

		return Mathf.Sqrt(distanceW + distanceH);
	}

	float CalcAstarHeuristic(float distanceFromStart, TileCell tileCell, TileCell targetCell)
	{
		return distanceFromStart + CalcEuclideanDistance(tileCell, targetCell);
	}

	void BuildPath()
	{
		while (null != _reverseTileCell)
		{
			_reverseTileCell.DrawColor(Color.red);

			_character.PushPathTileCell(_reverseTileCell);
			_reverseTileCell = _reverseTileCell.GetPrevCell();
		}

		if(_character.GetPathStack().Count != 0)
			_character.GetPathStack().Pop();        //자기 위치 타일 빼주기
	}

	#endregion

	#region PATH MOVE
	Stack<TileCell> _pathStack;
	TileCell _pathTargetCell;
	Vector2Int _direction;

	void InitMove()
	{
		_pathStack = _character.GetPathStack();
		if(0 != _pathStack.Count)
		{
			_pathTargetCell = _pathStack.Pop();
			_direction = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), _pathTargetCell);
			if (_direction.Equals(Vector2Int.zero))
				Debug.Log("ZERO DIRECTION");
		}
	}

	public bool Move()
	{
		if (0 == _pathStack.Count)
			return true;

		bool bIsArrived = (_character.GetTransform().position.x.EqualApproximately(_pathTargetCell.GetPosition().x, 0.2f) &&
							_character.GetTransform().position.y.EqualApproximately(_pathTargetCell.GetPosition().y, 0.2f));
		if (bIsArrived)
		{
			_pathTargetCell = _pathStack.Pop();
			_direction = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), _pathTargetCell);
		}
			
		Vector2 newPosition = TileHelper.GetSlopeDirection(_direction);
		_character.UpdateDirectionWithAnimation(_direction);
		_character.UpdatePosition(newPosition);

		return false;
	}

	#endregion
}
