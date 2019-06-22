using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject
{
	Transform _transform;
	Animator _animator;

	private void Awake()
	{
		_objectType = eMapObjectType.CHARACTER;
	}

	// Start is called before the first frame update
	void Start()
    {
		
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
		_transform = GetComponent<Transform>();

		InitState();
		_animator = GetComponent<Animator>();

		InitStatus();

		_hasDestination = false;
	}
	
	#region STATE
	protected State _curState;
	protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();

	public virtual void InitState()
	{
		ReplaceState(eStateType.IDLE, new IdleState());
		ReplaceState(eStateType.MOVE, new MoveState());
		ReplaceState(eStateType.ATTACK, new AttackState());
		ReplaceState(eStateType.DAMAGE, new DamageState());

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

	#region POSITION AND BOUNDARY
	public void UpdateNextPosition(Vector2 destination)
	{
		TileMap map = GameManager.Instance.GetMap();
		eTileDirection boundaryDirection = map.GetTileCell(_tileX, _tileY).CheckTileBoundary(destination);
		sTilePosition nextTilePos = new sTilePosition(_tileX, _tileY);
		TileHelper.GetNextTilePosByTileDirection(boundaryDirection, ref nextTilePos);

		if (map.CanMoveTileCell(nextTilePos.tileX, nextTilePos.tileY))
		{
			map.GetTileCell(_tileX, _tileY).RemoveObject(this);
			map.GetTileCell(nextTilePos.tileX, nextTilePos.tileY).AddObject(this, _currentLayer);
			_tileX = nextTilePos.tileX;
			_tileY = nextTilePos.tileY;
			transform.position = new Vector3(destination.x, destination.y, 0.0f);
		}
	}

	eDirection _lookAt;
	Vector2Int _lookDirection = new Vector2Int();
	public void UpdateDirectionWithAnimation(Vector2Int direction)
	{
		string trigger = "";
		int x = direction.x;
		int y = direction.y;

		if (x > 1) x = 1;
		if (x < -1) x = -1;
		if (y > 1) y = 1;
		if (y < -1) y = -1;

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

		if (!trigger.Equals(""))
		{
			_lookDirection.x = x;
			_lookDirection.y = y;
			_animator.SetTrigger(trigger);
		}
	}

	public eDirection LookAt() { return _lookAt; }
	public Vector2Int GetLookDirection() { return _lookDirection; }

	#endregion

	public Transform GetTransform() { return _transform; }

	#region Message
	public override void ReceiveMessage(MessageParam msgParam)
	{
		switch(msgParam.message)
		{
			case "Attack":
				Debug.Log(_transform.name + "..Sender: " + msgParam.sender.name);
				_receiveDamage = msgParam.attackPoint;
				ChangeState(eStateType.DAMAGE);
				break;
			default:
				break;
		}
	}
	#endregion

	#region Status Battle etc
	protected sStatus _status;

	float _receiveDamage;

	public virtual void InitStatus()
	{
		_status = new sStatus();
		_status.maxHp = 100;
		_status.hp = 80;
		_status.maxMp = 100;
		_status.mp = 50;
		_status.attack = 10;
		_status.armor = 0.0f;
		_status.avoid = 5;

		_status.speed = 7.0f;

		_receiveDamage = 0;
	}

	public ref sStatus GetStatus()
	{
		return ref _status;
	}

	public float GetReceiveDamage() { return _receiveDamage; }

	public void DecreaseHp(int damage)
	{
		_status.hp -= damage;
		if (_status.hp <= 0)
			_status.hp = 0;
		Debug.Log(_transform.name + "'s hp: " + _status.hp);
	}

	float _attackDelay;
	public void SetAttackDelay(float time) { _attackDelay = time; }
	public float GetAttackDelay() { return _attackDelay; }
	public void ResetAttackDelay() { _attackDelay = 0.0f; }

	#endregion

	bool _hasDestination;
	TileCell _destination;
	public bool HasDestination() { return _hasDestination; }
	public TileCell GetDestination() { return _destination; }
	public void SetDestination(TileCell destination)
	{
		_destination = destination;
		_hasDestination = true;
	}

	public void ResetDestination()
	{
		_destination = null;
		_hasDestination = false;
	}

	public void PickUpItem(ItemObject item)
	{
		bool result = Inventory.Instance.AddItem(item);
		if (result)
		{
			Destroy(item.gameObject);
		}
			
	}
}

public struct sStatus
{
	public int maxHp;
	public int maxMp;
	public int hp;
	public int mp;

	public int attack;
	public float armor;
	public int avoid;

	public float speed;
}