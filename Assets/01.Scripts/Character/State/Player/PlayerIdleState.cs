﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIdleState : State
{
	public override void Update()
	{
		base.Update();

		if (Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.LeftArrow) ||
			Input.GetKey(KeyCode.RightArrow)
			)
		{
			_nextState = eStateType.MOVE;
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_nextState = eStateType.ATTACK;
			return;
		}

		//Get the Item
		if (Input.GetKeyDown(KeyCode.Z))
		{
			TileCell tileCell = _character.GetCurrentTileCell();
			MapObject item = tileCell.FindObjectByType(eMapObjectType.ITEM, eTileLayer.ITEM);
			if (null != item)
			{
				Debug.Log("Item name: " + item.name);
				_character.PickUpItem((ItemObject)item);
			}
		}

		if(Input.GetKeyDown(KeyCode.F))
		{
			TileCell tileCell = _character.GetCurrentTileCell();
			MapObject portal = tileCell.FindObjectByType(eMapObjectType.PORTAL, eTileLayer.ON_GROUND);
			if(null != portal)
			{
				MessageParam msgParm = new MessageParam();
				msgParm.sender = _character;
				msgParm.receiver = portal;
				msgParm.message = "Interact";
				
				MessageSystem.Instance.Send(msgParm);
			}
		}

		//TEST: Print tile position
		if (Input.GetKeyDown(KeyCode.T))
		{
			sTilePosition tilePos = _character.GetTilePosition();
			Debug.Log(tilePos.ToString());
		}

		if(Input.GetKeyDown(KeyCode.Y))
		{
			sTilePosition tilePos = _character.GetTilePosition();
			int tileX = tilePos.tileX;
			int tileY = tilePos.tileY;
			for(int y = tileY + 1; y >= tileY - 1; y--)
			{
				string str = "";
				for(int x = tileX - 1; x <= tileX + 1; x++)
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

		//TEST: Pathfinding
		if(Input.GetKeyDown(KeyCode.P))
		{
			_nextState = eStateType.PATHFIND;
		}
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Stop()
	{
		base.Stop();
	}
}
