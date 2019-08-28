﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindState : State
{
	struct sPathCommand
	{
		public TileCell tileCell;
		public float heuristic;
	}

	List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();
	TileCell _targetTileCell;
	TileCell _reverseTileCell;

	public override void Update()
	{
		base.Update();

	}

	public override void Start()
	{
		base.Start();
		Debug.Log("Start pathfind");

		_targetTileCell = _character.GetDestination();
		if (null == _targetTileCell)
		{
			Debug.Log("No Target");
			return;
		}
			

		sPathCommand command = new sPathCommand();
		command.tileCell = _character.GetCurrentTileCell();
		command.heuristic = 0.0f;

		PushCommand(command);

		FindPath();
		BulidPath();
	}

	public override void Stop()
	{
		base.Stop();

	}

	void PushCommand(sPathCommand command)
	{
		_pathfindingQueue.Add(command);
		_pathfindingQueue.Sort((sPathCommand lhs, sPathCommand rhs) => lhs.heuristic.CompareTo(rhs.heuristic));
	}

	void FindPath()
	{
		TileMap map = GameManager.Instance.GetMap();
		while(0 != _pathfindingQueue.Count)
		{
			//제일 앞 타일을 하나 꺼낸다.
			sPathCommand command = _pathfindingQueue[0];
			_pathfindingQueue.RemoveAt(0);

			//방문한 타일인가?
			if(false == command.tileCell.IsVisit())
			{
				command.tileCell.Visit();

				if(_targetTileCell.GetTilePosition().Equals(command.tileCell.GetTilePosition()))
				{
					_reverseTileCell = _targetTileCell;
					return;
				}

				for(int direction = 0; direction < 8; direction++)
				{
					sTilePosition nextTilePos = new sTilePosition(command.tileCell.GetTileX(), command.tileCell.GetTileY());
					TileHelper.GetNextTilePosByDirection((eDirection)direction, ref nextTilePos);
					TileCell nextTileCell = map.GetTileCell(nextTilePos);

					if( ((true == map.CanMoveTileCell(nextTilePos.tileX, nextTilePos.tileY)) && false == nextTileCell.IsVisit()) ||
						(nextTilePos.tileX == _targetTileCell.GetTileX() && nextTilePos.tileY == _targetTileCell.GetTileY()))
					{
						float newDistanceFromStart = command.tileCell.GetDistanceFromStart() + command.tileCell.GetDistanceWeight();

						if(null == nextTileCell.GetPrevCell())
						{
							nextTileCell.SetDistanceFromStart(newDistanceFromStart);
							nextTileCell.SetPrevCell(command.tileCell);     //이전 타일 기억

							sPathCommand newCommand = new sPathCommand();
							newCommand.heuristic = 0.0f;
							newCommand.tileCell = nextTileCell;
							PushCommand(newCommand);

							nextTileCell.DrawColor(Color.blue);
						}
						else
						{
							if(newDistanceFromStart < nextTileCell.GetDistanceFromStart())
							{
								nextTileCell.SetDistanceFromStart(newDistanceFromStart);
								nextTileCell.SetPrevCell(command.tileCell);

								sPathCommand newCommand = new sPathCommand();
								newCommand.heuristic = 0.0f;
								newCommand.tileCell = nextTileCell;
								PushCommand(newCommand);
							}
						}
					}
				}
			}
		}
	}

	void BulidPath()
	{
		while(null != _reverseTileCell)
		{
			_reverseTileCell.DrawColor(Color.red);
			_reverseTileCell = _reverseTileCell.GetPrevCell();
		}
	}
}
