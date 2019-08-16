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
			sTilePosition nextTilePos = _character.GetTilePosition();
			TileHelper.GetNextTilePosByDirection(lookDirection, ref nextTilePos);

			TileMap map = GameManager.Instance.GetMap();
			Debug.Log("Attack: " + nextTilePos.tileX+ ", " + nextTilePos.tileX);
			TileCell tileCell = map.GetTileCell(nextTilePos.tileX, nextTilePos.tileX);
			if (tileCell != null)
			{
				MapObject enemy = tileCell.FindObjectByType(eMapObjectType.ENEMY, eTileLayer.GROUND);
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
