using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
	public override void Update()
	{
		base.Update();

		TileMap map = GameManager.Instance.GetMap();
		Vector2Int lookDirection = new Vector2Int();
		Vector2 direction = new Vector2();

		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (Input.GetKey(KeyCode.RightArrow))
			{
				direction += new Vector2(1.0f, map.GetSlope());
				lookDirection += new Vector2Int(1, 1);
			}
			else if(Input.GetKey(KeyCode.LeftArrow))
			{
				direction += new Vector2(-1.0f, map.GetSlope());
				lookDirection += new Vector2Int(-1, 1);
			}
			else
			{
				direction += new Vector2(0.0f, 1.0f);
				lookDirection += new Vector2Int(0, 1);
			}
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if(Input.GetKey(KeyCode.RightArrow))
			{
				direction += new Vector2(1.0f, -map.GetSlope());
				lookDirection += new Vector2Int(1, -1);
			}
			else if(Input.GetKey(KeyCode.LeftArrow))
			{
				direction += new Vector2(-1.0f, -map.GetSlope());
				lookDirection += new Vector2Int(-1, -1);
			}
			else
			{
				direction += new Vector2(0.0f, -1.0f);
				lookDirection += new Vector2Int(0, -1);
			}
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
			{
				direction += new Vector2(-1.0f, 0.0f);
				lookDirection += new Vector2Int(-1, 0);
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
			{
				direction += new Vector2(1.0f, 0.0f);
				lookDirection += new Vector2Int(1, 0);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_nextState = eStateType.ATTACK;
			return;
		}

		if(direction.Equals(Vector2.zero))
		{
			_nextState = eStateType.IDLE;
			return;
		}

		_character.UpdateDirectionWithAnimation(lookDirection);

		//TEST tile properties
		TileCell tileCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());
		var tileProperties = tileCell.GetProperties(eTileLayer.GROUND);
		float speed = _character.GetStatus().speed + tileProperties.speed;

		Vector2 position = speed * direction.normalized * Time.deltaTime;
		Vector2 destination = (Vector2)(_character.GetTransform().position) + position;

		_character.UpdateNextPosition(destination);
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
