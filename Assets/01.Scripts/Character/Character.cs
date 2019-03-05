using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject
{
	float _speed = 8.0f;
	int _hp = 100;
	Transform _transform;
	Animator _animator;

	private void Awake()
	{
		_transform = GetComponent<Transform>();
	}

	// Start is called before the first frame update
	void Start()
    {
		_objectType = eMapObjectType.CHARACTER;
	}

    // Update is called once per frame
    void Update()
    {
		if (eStateType.NONE != _curState.GetNextState())
			ChangeState(_curState.GetNextState());
		_curState.Update();
	}

	public void Init()
	{
		InitState();
		_animator = GetComponent<Animator>();
	}
	
	#region STATE
	protected State _curState;
	protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();

	public virtual void InitState()
	{
		ReplaceState(eStateType.IDLE, new IdleState());
		ReplaceState(eStateType.MOVE, new MoveState());
		ReplaceState(eStateType.ATTACK, new AttackState());

		_curState = _stateMap[eStateType.IDLE];
	}

	public void ReplaceState(eStateType changeType, State replaceState)
	{
		if(_stateMap.ContainsKey(changeType))
		{
			_stateMap.Remove(changeType);
		}

		State state = replaceState;
		state.Init(this);
		_stateMap[changeType] = state;
	}

	void ChangeState(eStateType nextState)
	{
		if (null != _curState)
			_curState.Stop();

		_curState = _stateMap[nextState];
		_curState.Start();
	}

	#endregion

	public void UpdateNextPosition(Vector2 destination)
	{
		TileMap map = GameManager.Instance.GetMap();
		eTileDirection nextDirection = map.GetTileCell(_tileX, _tileY).CheckTileDirection(destination);
		int nextTileX = _tileX;
		int nextTileY = _tileY;
		switch (nextDirection)
		{
			case eTileDirection.NORTH_WEST:
				nextTileY++;
				break;
			case eTileDirection.NORTH_EAST:
				nextTileX++;
				break;
			case eTileDirection.SOUTH_EAST:
				nextTileY--;
				break;
			case eTileDirection.SOUTH_WEST:
				nextTileX--;
				break;
			case eTileDirection.NONE:
				break;
			default:
				break;
		}

		if (map.CanMoveTileCell(nextTileX, nextTileY))
		{
			map.GetTileCell(_tileX, _tileY).RemoveObject(this);
			map.GetTileCell(nextTileX, nextTileY).AddObject(this, _currentLayer);
			_tileX = nextTileX;
			_tileY = nextTileY;
			transform.position = new Vector3(destination.x, destination.y, 0.0f);
		}
	}

	public Transform GetTransform() { return _transform; }
	public float GetSpeed() { return _speed; }

	#region Status
	void DecreaseHp(int damage)
	{
		_hp -= damage;
		if (_hp <= 0)
			_hp = 0;
	}
	#endregion

	#region Message
	public override void ReceiveMessage(MessageParam msgParam)
	{
		switch(msgParam.message)
		{
			case "Attack":
				Debug.Log("Sender: " + msgParam.sender.name);
				//DecreaseHp(msgParam.attackPoint);
				break;
			default:
				break;
		}
	}
	#endregion

	eDirection _lookAt;
	public void UpdateDirectionWithAnimation(Vector2 direction)
	{
		string trigger = "";
		int x = (int)direction.x;
		int y = (int)direction.y;

		if ((x == 1) && (y == -1))
		{
			trigger = "SOUTH_EAST";
			_lookAt = eDirection.SOUTH_EAST;
		}
		else if ((x == -1) && (y == -1))
		{
			trigger = "SOUTH_WEST";
			_lookAt = eDirection.SOUTH_WEST;
		}
		else if ((x == 1) && (y == 1))
		{
			trigger = "NORTH_EAST";
			_lookAt = eDirection.NORTH_EAST;
		}
		else if ((x == -1) && (y == 1))
		{
			trigger = "NORTH_WEST";
			_lookAt = eDirection.NORTH_WEST;
		}
		else if ((x == 0) && (y == -1))
		{
			trigger = "SOUTH";
			_lookAt = eDirection.SOUTH;
		}
		else if ((x == 0) && (y == 1))
		{
			trigger = "NORTH";
			_lookAt = eDirection.NORTH;
		}
		else if ((x == 1) && (y == 0))
		{
			trigger = "EAST";
			_lookAt = eDirection.EAST;
		}
		else if ((x == -1) && (y == 0))
		{
			trigger = "WEST";
			_lookAt = eDirection.WEST;
		}

		_animator.SetTrigger(trigger);
	}

	public eDirection LookAt() { return _lookAt; }
}
