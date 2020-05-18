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

		if (_character._playerController.bIsPC)
		{
			PCMove(out newPosition, out lookDirection);
		}
		else
		{
			var joystick = _character._playerController._joystick;
			if (!joystick.IsNeutral())
			{
				newPosition += TileHelper.GetSlopeDirection(joystick.Direction);
				lookDirection += joystick.Direction.ToVector2Int();
			}
		}

		if (newPosition.Equals(Vector2.zero))
		{
			_nextState = eStateType.IDLE;
			return;
		}

		_character.UpdateDirection(lookDirection);
		_character.UpdatePosition(newPosition);
	}

	void PCMove(out Vector2 newPosition, out Vector2Int lookDirection)
	{
		TileSystem tileSystem = TileSystem.Instance;
		//Vector2Int lookDirection = new Vector2Int();
		//Vector2 newPosition = Vector2.zero

		lookDirection = Vector2Int.zero;
		newPosition = Vector2.zero;

		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (Input.GetKey(KeyCode.RightArrow))
			{
				newPosition += new Vector2(1.0f, tileSystem.GetSlope());
				lookDirection += new Vector2Int(1, 1);
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				newPosition += new Vector2(-1.0f, tileSystem.GetSlope());
				lookDirection += new Vector2Int(-1, 1);
			}
			//else
			//{
			//	newPosition += new Vector2(0.0f, 1.0f);
			//	lookDirection += new Vector2Int(0, 1);
			//}
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (Input.GetKey(KeyCode.RightArrow))
			{
				newPosition += new Vector2(1.0f, -tileSystem.GetSlope());
				lookDirection += new Vector2Int(1, -1);
			}
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				newPosition += new Vector2(-1.0f, -tileSystem.GetSlope());
				lookDirection += new Vector2Int(-1, -1);
			}
			//else
			//{
			//	newPosition += new Vector2(0.0f, -1.0f);
			//	lookDirection += new Vector2Int(0, -1);
			//}
		}

		//if (Input.GetKey(KeyCode.LeftArrow))
		//{
		//	if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
		//	{
		//		newPosition += new Vector2(-1.0f, 0.0f);
		//		lookDirection += new Vector2Int(-1, 0);
		//	}
		//}
		//else if (Input.GetKey(KeyCode.RightArrow))
		//{
		//	if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
		//	{
		//		newPosition += new Vector2(1.0f, 0.0f);
		//		lookDirection += new Vector2Int(1, 0);
		//	}
		//}
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
