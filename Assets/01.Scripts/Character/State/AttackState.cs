using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	float _duration;

	public override void Update()
	{
		base.Update();
		_duration -= Time.deltaTime;

		if(_duration < 0.0f)
		{
			eDirection lookDirection = _character.LookAt();
			sTilePosition curTilePos = _character.GetTilePosition();
			int attackTileX = curTilePos.tileX;
			int attackTileY = curTilePos.tileY;
			switch (lookDirection)
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
					msg.attackPoint = _character.GetStatus().attack;
					MessageSystem.Instance.Send(msg);
				}
			}
			_character.ResetAttackDelay();
			_nextState = eStateType.IDLE;
		}
	
	}

	public override void Start()
	{
		base.Start();
		_duration = _character.GetAttackDelay();
		Debug.Log("<color=red>Start Attack: " + _duration + "</color>");
	}

	public override void Stop()
	{
		base.Stop();
	}
}
