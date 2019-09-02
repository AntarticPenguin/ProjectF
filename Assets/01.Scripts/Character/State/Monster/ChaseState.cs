using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
	public override void Update()
	{
		base.Update();

		if(_character.HasTarget())
		{
			TileCell targetCell = _character.GetTarget().GetCurrentTileCell();
			Vector2Int lookDirection = TileHelper.GetDirectionVector(_character.GetCurrentTileCell(), targetCell);
			Vector2 newPosition = Vector2.zero;
			newPosition.x += lookDirection.x;
			newPosition.y += lookDirection.y;

			_character.UpdateNextPosition(newPosition);
			_character.UpdateDirectionWithAnimation(lookDirection);
		}
	}

	public override void Start()
	{
		base.Start();

		_character.GetSpriteRenderer().color = Color.red;
	}

	public override void Stop()
	{
		base.Stop();

		_character.ResetTarget();
		_character.GetSpriteRenderer().color = Color.white;
	}
}
