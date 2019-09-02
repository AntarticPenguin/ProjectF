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

		_bHasDestination = false;
		_bHasTarget = false;
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
	public void UpdateNextPosition(Vector2 position)
	{
		float speed = _status.speed + GetCurrentTileCell().GetProperties(eTileLayer.GROUND).speed;
		Vector2 destination = (Vector2)(_transform.position) + (position.normalized * speed * Time.deltaTime);

		TileMap map = GameManager.Instance.GetMap();
		TileCell curTileCell = map.GetTileCell(_tileX, _tileY);
		eTileDirection boundaryDirection = curTileCell.CheckTileBoundary(destination);
		sTilePosition nextTilePos = new sTilePosition(_tileX, _tileY);
		TileHelper.GetNextTilePosByTileDirection(boundaryDirection, ref nextTilePos);

		if (map.CanMoveTileCell(nextTilePos.tileX, nextTilePos.tileY))
		{
			//타일 오프셋에 따른 캐릭터 y값 보정(des = des + (next.offset - cur.offset))
			if (eTileDirection.IN_TILE != boundaryDirection)
			{
				float curOffset = curTileCell.GetOffset();
				float nextOffset = map.GetTileCell(nextTilePos.tileX, nextTilePos.tileY).GetOffset();
				destination.y = destination.y + (nextOffset - curOffset);
			}

			map.GetTileCell(_tileX, _tileY).RemoveObject(this);
			map.GetTileCell(nextTilePos.tileX, nextTilePos.tileY).AddObject(this, _currentLayer);
			_tileX = nextTilePos.tileX;
			_tileY = nextTilePos.tileY;

			//z값 -1 : 동일한 레이어 + 동일한 order 에서 캐릭터들이 타일보다 뒤에 있으면 안됨
			transform.position = new Vector3(destination.x, destination.y, -1.0f);
		}
	}

	eDirection _lookAt;
	Vector2Int _lookDirection = new Vector2Int();
	public void UpdateDirectionWithAnimation(Vector2Int direction)
	{
		string trigger = "";
		int x = direction.x;
		int y = direction.y;

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
	public SpriteRenderer GetSpriteRenderer() { return GetComponent<SpriteRenderer>(); }

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

		_status.speed = 2.0f;

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

	#region SET DESTINATION OR TARGET, PATHFINDING
	bool _bHasDestination;
	TileCell _destination;
	public bool HasDestination() { return _bHasDestination; }
	public TileCell GetDestination() { return _destination; }
	public void SetDestination(TileCell destination)
	{
		_destination = destination;
		_bHasDestination = true;
	}

	public void ResetDestination()
	{
		_destination = null;
		_bHasDestination = false;
	}

	MapObject _target;
	bool _bHasTarget;
	public bool HasTarget() { return _bHasTarget; }
	public MapObject GetTarget() { return _target; }
	public void SetTarget(MapObject target)
	{
		_target = target;
		_bHasTarget = true;
	}

	public void ResetTarget()
	{
		_target = null;
		_bHasTarget = false;
	}

	Stack<TileCell> _pathStack = new Stack<TileCell>();
	public void PushPathTileCell(TileCell tileCell)
	{
		_pathStack.Push(tileCell);
	}

	public void ResetPath()
	{
		if (0 != _pathStack.Count)
			_pathStack.Clear();
	}

	public Stack<TileCell> GetPathStack() { return _pathStack; }

	#endregion

	public void PickUpItem(ItemObject item)
	{
		bool result = Inventory.Instance.AddItem(item);
		if (result)
		{
			TileMap map = GameManager.Instance.GetMap();
			sTilePosition tilePos = item.GetTilePosition();
			TileCell tileCell = map.GetTileCell(tilePos);
			tileCell.RemoveObject(item);
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