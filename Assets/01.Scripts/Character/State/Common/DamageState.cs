using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : State
{
	public override void Update()
	{
		base.Update();
	}

	public override void Start()
	{
		base.Start();

		sStatus status = _character.GetStatus();
		float random = Random.Range(0, 10);
		if(random < status.avoid)
		{
			Debug.Log("Avoid attack");
		}
		else
		{
			int receiveDamage = (int)(_character.GetReceiveDamage() - status.armor);
			if (receiveDamage < 0)
				Debug.Log("Defense");
			else
			{
				_character.DecreaseHp(receiveDamage);
			}
		}
		_nextState = eStateType.IDLE;
	}

	public override void Stop()
	{
		base.Stop();
	}
}
