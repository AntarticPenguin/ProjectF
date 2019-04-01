using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	private void Awake()
	{
		_objectType = eMapObjectType.ENEMY;
	}

	void Start()
	{
		
	}

	public override void InitState()
	{
		base.InitState();
	}							
}
