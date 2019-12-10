using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
	public override void Update()
	{
		base.Update();

		TileSystem tileSystem = TileSystem.Instance;
		Vector2Int lookDirection = new Vector2Int();
		Vector2 newPosition = new Vector2();

		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (Input.GetKey(KeyCode.RightArrow))
			{
				newPosition += new Vector2(1.0f, tileSystem.GetSlope());
				lookDirection += new Vector2Int(1, 1);
			}
			else if(Input.GetKey(KeyCode.LeftArrow))
			{
				newPosition += new Vector2(-1.0f, tileSystem.GetSlope());
				lookDirection += new Vector2Int(-1, 1);
			}
			else
			{
				newPosition += new Vector2(0.0f, 1.0f);
				lookDirection += new Vector2Int(0, 1);
			}
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if(Input.GetKey(KeyCode.RightArrow))
			{
				newPosition += new Vector2(1.0f, -tileSystem.GetSlope());
				lookDirection += new Vector2Int(1, -1);
			}
			else if(Input.GetKey(KeyCode.LeftArrow))
			{
				newPosition += new Vector2(-1.0f, -tileSystem.GetSlope());
				lookDirection += new Vector2Int(-1, -1);
			}
			else
			{
				newPosition += new Vector2(0.0f, -1.0f);
				lookDirection += new Vector2Int(0, -1);
			}
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
			{
				newPosition += new Vector2(-1.0f, 0.0f);
				lookDirection += new Vector2Int(-1, 0);
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
			{
				newPosition += new Vector2(1.0f, 0.0f);
				lookDirection += new Vector2Int(1, 0);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_nextState = eStateType.ATTACK;
			return;
		}

		if(newPosition.Equals(Vector2.zero))
		{
			_nextState = eStateType.IDLE;
			return;
		}

		_character.UpdateDirectionWithAnimation(lookDirection);
		_character.UpdateNextPosition(newPosition);
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
