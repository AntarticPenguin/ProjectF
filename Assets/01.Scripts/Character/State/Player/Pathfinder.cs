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
		ReplaceState(eStateType.IDLE, new PlayerIdleState());
		ReplaceState(eStateType.PATHFIND, new PathfindState());

		_curState = _stateMap[eStateType.IDLE];
	}

	public void SetTargetTileCell(int x, int y)
	{
		TileCell targetCell = GameManager.Instance.GetMap().GetTileCell(x, y);
		SetDestination(targetCell);
	}
}
