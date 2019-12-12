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
		if (Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.LeftArrow) ||
			Input.GetKey(KeyCode.RightArrow)
			)
		{
			_player.ChangeState(eStateType.MOVE);
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_player.ChangeState(eStateType.ATTACK);
			return;
		}

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

		//TEST: Print tile position
		if (Input.GetKeyDown(KeyCode.T))
		{
			sTilePosition tilePos = _player.GetTilePosition();
			Debug.Log(tilePos.ToString());
		}

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
	}

	public void Init(Character player)
	{
		_player = player;
	}
}
