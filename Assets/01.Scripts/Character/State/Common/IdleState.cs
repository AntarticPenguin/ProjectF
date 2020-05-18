using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	public override void Update()
	{
		base.Update();
	}

	public override void Start()
	{
		base.Start();

		_character.GetAnimPlayer().Play(GetTriggerName(_character.LookAt()));
	}

	public override void Stop()
	{
		base.Stop();
	}

	string GetTriggerName(eDirection lookAt)
	{
		string trigger = "";

		switch (lookAt)
		{
			case eDirection.NORTH_EAST:
				trigger = "NORTH_EAST";
				break;
			case eDirection.NORTH_WEST:
				trigger = "NORTH_WEST";
				break;
			case eDirection.SOUTH_EAST:
				trigger = "SOUTH_EAST";
				break;
			case eDirection.SOUTH_WEST:
				trigger = "SOUTH_WEST";
				break;
			default:
				trigger = "NORTH_EAST";
				break;
		}
		return trigger;
	}
}
