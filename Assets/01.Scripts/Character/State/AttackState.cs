using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	public override void Update()
	{
		base.Update();
	}

	public override void Start()
	{
		base.Start();

		eDirection lookDirection = _character.LookAt();
		sTilePosition curTilePos = _character.GetTilePosition();
		int attackTileX = curTilePos._tileX;
		int attackTileY = curTilePos._tileY;
		switch(lookDirection)
		{
			case eDirection.NORTH:
				attackTileX++;
				attackTileY++;
				break;
			case eDirection.NORTH_EAST:
				attackTileX++;
				break;
			case eDirection.EAST:
				attackTileX++;
				attackTileY--;
				break;
			case eDirection.SOUTH_EAST:
				attackTileY--;
				break;
			case eDirection.SOUTH:
				attackTileX--;
				attackTileY--;
				break;
			case eDirection.SOUTH_WEST:
				attackTileX--;
				break;
			case eDirection.WEST:
				attackTileX--;
				attackTileY++;
				break;
			case eDirection.NORTH_WEST:
				attackTileY++;
				break;
			default:
				break;
		}
		TileMap map = GameManager.Instance.GetMap();
		Debug.Log("Attack: " + attackTileX + ", " + attackTileY);
		TileCell tileCell = map.GetTileCell(attackTileX, attackTileY);
		if (tileCell != null)
		{
			MapObject enemy = tileCell.FindObjectByType(eMapObjectType.ENEMY);
			if (null != enemy)
			{
				MessageParam msg = new MessageParam();
				msg.sender = _character;
				msg.receiver = enemy;
				msg.message = "Attack";
				MessageSystem.Instance.Send(msg);
			}
		}
		else
			Debug.Log("Out of Range");

		_nextState = eStateType.IDLE;
	}

	public override void Stop()
	{
		base.Stop();
	}
}
