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
		_character.GetAnimator().SetTrigger("IDLE");
	}

	public override void Stop()
	{
		base.Stop();
	}
}
