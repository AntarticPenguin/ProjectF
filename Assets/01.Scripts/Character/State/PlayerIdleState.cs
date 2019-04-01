using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			

		//TEST: Print tile position
		TileMap map = GameManager.Instance.GetMap();
		if (Input.GetKeyDown(KeyCode.T))
		{
			sTilePosition tilePos = _character.GetTilePosition();
			Debug.Log(tilePos.ToString());

			//map.GetTileCell(0, 0).PrintObjectList();
		}

		if(Input.GetKeyDown(KeyCode.F1))
		{
			for(int y = 0; y < map._height; y++)
			{
				for (int x = 0; x < map._width; x++)
				{
					TileCell tileCell = map.GetTileCell(x, y);
					Debug.Log("tileX: " + x + ", tileY: " + y + "=> " + tileCell.PrintObjectList());
				}
			}
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
