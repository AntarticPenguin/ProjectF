using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	public override void Start()
	{
		base.Start();

		Character target = _character.GetAttackTarget();
		Vector2Int direction = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), target.GetCurrentTileCell());
		_character.UpdateDirectionWithAnimation(direction);
		eDirection lookAt = TileHelper.ConvertToeDirection(direction);

		_character.GetAnimPlayer().Play(GetTriggerName(lookAt), null, null,
		() =>
		{
			//endEvent
			_nextState = eStateType.IDLE;
			_character.ResetAttackCoolTimeDuration();
		});

		sTilePosition nextTilePos = _character.GetTilePosition();
		TileHelper.GetNextTilePosByDirection(lookAt, ref nextTilePos);

		TileSystem tileSystem = TileSystem.Instance;
		TileCell tileCell = tileSystem.GetTileCell(nextTilePos.tileX, nextTilePos.tileX);
		if (tileCell != null)
		{
			MapObject enemy = tileCell.FindObjectByType(GetHostileType(), eTileLayer.GROUND);
			if (null != enemy)
			{
				MessageParam msg = new MessageParam();
				msg.sender = _character;
				msg.receiver = enemy;
				msg.message = "Attack";
				msg.attackInfo.attackPoint = _character.GetStatus().attack;
				msg.attackInfo.attackType = eAttackType.NORMAL;

				MessageSystem.Instance.Send(msg);
			}
		}
	}

	public override void Stop()
	{
		base.Stop();
		_character.ResetAttackTarget();
	}

	string GetTriggerName(eDirection lookAt)
	{
		string trigger = "";

		switch (lookAt)
		{
			case eDirection.NORTH:
			case eDirection.NORTH_EAST:
			case eDirection.EAST:
				trigger = "ATTACK_NORTH_EAST";
				break;
			case eDirection.NORTH_WEST:
			case eDirection.WEST:
				trigger = "ATTACK_NORTH_WEST";
				break;
			case eDirection.SOUTH_EAST:
			case eDirection.SOUTH:
				trigger = "ATTACK_SOUTH_EAST";
				break;
			case eDirection.SOUTH_WEST:
				trigger = "ATTACK_SOUTH_WEST";
				break;
			default:
				trigger = "ATTACK_NORTH_EAST";
				break;
		}
		return trigger;
	}

	eMapObjectType GetHostileType()
	{
		switch (_character.GetMapObjectType())
		{
			case eMapObjectType.PLAYER:
				return eMapObjectType.ENEMY;
			case eMapObjectType.ENEMY:
				return eMapObjectType.PLAYER;
			default:
				return eMapObjectType.NONE;
		}
	}
}
