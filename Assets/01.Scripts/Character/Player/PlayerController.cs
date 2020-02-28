using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Character _player;

	void Awake()
	{
		_player = GetComponent<Character>();
		if (_player)
			Debug.Log($"PlayerController::Awake(): {_player.GetType()}");
	}

    // Update is called once per frame
    void Update()
    {
		Move();
		Attack();
		Loot();
		UsePortal();
		TestFunc();
	}

	public void Init(Character player)
	{
		_player = player;
	}

	void Move()
	{
		if (Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.LeftArrow) ||
			Input.GetKey(KeyCode.RightArrow)
			)
		{
			if(eStateType.MOVE == _player.GetCurStateType() || eStateType.IDLE == _player.GetCurStateType())
				_player.ChangeState(eStateType.MOVE);
		}
	}

	void Attack()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_player.IsAttackReady())
				_player.ChangeState(eStateType.ATTACK);
			else
				Debug.Log("Attack is not ready");
		}
	}

	void Loot()
	{
		//Get the Item
		if (Input.GetKeyDown(KeyCode.Z))
		{
			TileCell tileCell = _player.GetCurrentTileCell();
			MapObject item = tileCell.FindObjectByType(eMapObjectType.ITEM, eTileLayer.ITEM);
			if (null != item)
			{
				Debug.Log("Item name: " + item.name);
				_player.PickUpItem((ItemObject)item);
			}
		}
	}

	void UsePortal()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			TileCell tileCell = _player.GetCurrentTileCell();
			MapObject portal = tileCell.FindObjectByType(eMapObjectType.PORTAL, eTileLayer.TRIGGER);
			if (null != portal)
			{
				MessageParam msgParm = new MessageParam();
				msgParm.sender = _player;
				msgParm.receiver = portal;
				msgParm.message = "Interact";

				MessageSystem.Instance.Send(msgParm);
			}
		}
	}

	void TestFunc()
	{
		//TEST: Print tile position
		if (Input.GetKeyDown(KeyCode.T))
		{
			sTilePosition tilePos = _player.GetTilePosition();
			Debug.Log(tilePos.ToString());
		}

		//TEST: Print tiles that player stepped on
		if (Input.GetKeyDown(KeyCode.Y))
		{
			sTilePosition tilePos = _player.GetTilePosition();
			int tileX = tilePos.tileX;
			int tileY = tilePos.tileY;
			for (int y = tileY + 1; y >= tileY - 1; y--)
			{
				string str = "";
				for (int x = tileX - 1; x <= tileX + 1; x++)
				{
					if (tileX == x && tileY == y)
						str += "● ";
					else
					{
						if (TileSystem.Instance.GetTileCell(x, y).InCharacter())
							str += "O ";
						else
							str += "X ";
					}
				}
				Debug.Log(str);
			}
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			MessageParam msg = new MessageParam();
			msg.sender = null;
			msg.receiver = _player;
			msg.message = "Attack";
			msg.attackInfo.attackPoint = 10;
			msg.attackInfo.attackType = eAttackType.STUN;

			MessageSystem.Instance.Send(msg);
		}
	}
}
