using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
	public override void Update()
	{
		base.Update();

		Vector2 direction = new Vector2();

		if (Input.GetKey(KeyCode.UpArrow))
		{
			direction += new Vector2(0.0f, 1.0f);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			direction += new Vector2(0.0f, -1.0f);
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			direction += new Vector2(-1.0f, 0.0f);
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			direction += new Vector2(1.0f, 0.0f);
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

		_character.UpdateDirectionWithAnimation(direction);

		//TEST tile properties
		TileMap map = GameManager.Instance.GetMap();
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
