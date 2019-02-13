using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	public override void Update()
	{
		base.Update();

		if( Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.LeftArrow) ||
			Input.GetKey(KeyCode.RightArrow)
			)
		{
			_nextState = eStateType.MOVE;
			return;
		}

		//TEST: Print tile position
		TileMap map = GameManager.Instance.GetMap();
		if (Input.GetKeyDown(KeyCode.T))
		{
			int tileX = _character.GetTileX();
			int tileY = _character.GetTileY();
			Debug.Log("TileX: " + tileX + ", TileY: " + tileY + ", " + map.GetTileCell(tileX, tileY).GetPosition() +
				", " + "Char: " + _character.GetTransform().position);
		}
	}

	public override void Start()
	{
		base.Start();
		Debug.Log("State: " + eStateType.IDLE.ToString());
	}

	public override void Stop()
	{
		base.Stop();
	}
}
