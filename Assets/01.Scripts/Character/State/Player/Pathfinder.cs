using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : Character
{
	private void Awake()
	{
		_objectType = eMapObjectType.CHARACTER;
	}

	public override void InitState()
	{
		base.InitState();
		ReplaceState(eStateType.PATHFIND, new PathFindState());
		ReplaceState(eStateType.MOVE, new PathMoveState());

		_curState = _stateMap[eStateType.IDLE];
	}

	public void SetTargetTileCell(int x, int y)
	{
		TileCell targetCell = TileSystem.Instance.GetTileCell(x, y);
		SetDestination(targetCell);
	}
}
