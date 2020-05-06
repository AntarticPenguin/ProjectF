using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	public override void Start()
	{
		base.Start();
		_character.DoAttack();

		Character target = _character.GetAttackTarget();
		Vector2Int direction = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), target.GetCurrentTileCell());
		_character.UpdateDirectionWithAnimation(direction);
		eDirection lookAt = TileHelper.ConvertToeDirection(direction);

		_character.GetAnimPlayer().Play(GetTriggerName(lookAt), null, null,
		() =>
		{
			//endEvent
			_nextState = eStateType.IDLE;
		});

		HashSet<MapObject> enemies = FindEnemy();
		if (null != enemies)
		{
			Debug.Log("ENEMY COUNT: " + enemies.Count);
			foreach(var enemy in enemies)
			{
				MessageParam msg = new MessageParam();
				msg.sender = _character;
				msg.receiver = enemy;
				msg.message = "Attack";
				msg.damageInfo.damagePoint = _character.GetStatus().attack;
				msg.damageInfo.attackType = eDamageType.NORMAL;

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
			case eDirection.NORTH_EAST:
				trigger = "ATTACK_NORTH_EAST";
				break;
			case eDirection.NORTH_WEST:
				trigger = "ATTACK_NORTH_WEST";
				break;
			case eDirection.SOUTH_EAST:
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

	HashSet<MapObject> FindEnemy()
	{
		var tileSystem = TileSystem.Instance;
		var attackInfo = _character.GetAttackInfo();

		List<TileCell> tileCells = tileSystem.GetTilecellInAttackRange(_character.GetCurrentTileCell(), _character.LookAt(), attackInfo.attackRangeType, attackInfo.attackRange);

		HashSet<MapObject> enemies = new HashSet<MapObject>();
		for(int i = 0; i < tileCells.Count; i++)
		{
			var findObject = tileCells[i].FindObjectByType(GetHostileType(), eTileLayer.RANGE);
			if (null != findObject)
				enemies.Add(findObject);
		}

		return enemies;
	}
}
