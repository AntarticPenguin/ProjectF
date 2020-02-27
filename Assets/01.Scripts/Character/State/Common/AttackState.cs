using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	float _castingTime;

	public override void Update()
	{
		base.Update();

		_castingTime -= Time.deltaTime;

		if (_castingTime <= 0.0f)
		{
			eDirection lookDirection = _character.LookAt();
			sTilePosition nextTilePos = _character.GetTilePosition();
			TileHelper.GetNextTilePosByDirection(lookDirection, ref nextTilePos);

			TileSystem tileSystem = TileSystem.Instance;
			Debug.Log("Attack: " + nextTilePos.tileX+ ", " + nextTilePos.tileX);
			TileCell tileCell = tileSystem.GetTileCell(nextTilePos.tileX, nextTilePos.tileX);
			if (tileCell != null)
			{
				MapObject enemy = tileCell.FindObjectByType(eMapObjectType.ENEMY, eTileLayer.GROUND);
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
//			_nextState = eStateType.IDLE;
		}
	}

	public override void Start()
	{
		base.Start();
		eDirection lookAt = _character.LookAt();
		_character.GetAnimPlayer().Play(GetTriggerName(lookAt), null, null,
		() =>
		{
			//endEvent
			_nextState = eStateType.IDLE;

			//보고 있던 방향으로 다시 애니메이션 재생
			_character.GetAnimator().SetTrigger(lookAt.ToString());
		});

		_character.ResetAttackCoolTimeDuration();
		_castingTime = _character.GetCastingTime();
		//Debug.Log("<color=red>Start Attack: " + castingTime + "</color>");
	}

	public override void Stop()
	{
		base.Stop();
		_character.ResetCastingTime();
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
}
