using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStateType
{
	NONE,
	IDLE,
	MOVE,
}

public class State
{
	protected eStateType _nextState = eStateType.NONE;
	protected Character _character;

	public virtual void Init(Character character)
	{
		_character = character;
	}

	public virtual void Update()
	{
		
	}

	public virtual void Start()
	{
		_nextState = eStateType.NONE;
	}

	public virtual void Stop()
	{
		
	}

	public eStateType GetNextState()
	{
		return _nextState;
	}
}
