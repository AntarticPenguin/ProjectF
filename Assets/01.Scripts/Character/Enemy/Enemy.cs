using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	void Start()
	{
		_objectType = eMapObjectType.ENEMY;
	}

	public override void InitState()
	{
		base.InitState();
	}
}
