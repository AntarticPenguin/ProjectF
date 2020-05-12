using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject
{
	public PlayerController _playerController { get; set; }
	Transform _transform;
	Animator _animator;
	AnimationPlayer _animPlayer;

	private void Awake()
	{
		SetMapObjectType(eMapObjectType.CHARACTER);
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
		_animator = GetComponentInChildren<Animator>();
		_animPlayer = GetComponent<AnimationPlayer>();

		InitState();
		_curState.Start();

		InitStatus();

		_bHasDestination = false;

		InitCoolTime();
	}
	
	#region STATE
	protected State _curState;
	protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();
	eStateType _curStateType;
	
	public virtual void InitState()
	{
		ReplaceState(eStateType.IDLE, new IdleState());
		ReplaceState(eStateType.MOVE, new MoveState());
		ReplaceState(eStateType.ATTACK, new AttackState());
		ReplaceState(eStateType.DAMAGE, new DamageState());

		_curState = _stateMap[eStateType.IDLE];
		_curStateType = eStateType.IDLE;
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

	public void ChangeState(eStateType nextState)
	{
		if (null != _curState)
			_curState.Stop();

		_curState = _stateMap[nextState];
		_curState.Start();
		_curStateType = nextState;
	}

	public eStateType GetCurStateType() { return _curStateType; }

	#endregion

	#region POSITION AND BOUNDARY
	public void UpdatePosition(Vector2 position)
	{
		float speed = _status.speed + GetCurrentTileCell().GetProperties(eTileLayer.GROUND).speed;
		Vector2 destination = (Vector2)(_transform.position) + (position.normalized * speed * Time.deltaTime);

		TileSystem tileSystem = TileSystem.Instance;
		TileCell curTileCell = tileSystem.GetTileCell(_tileX, _tileY);
		eTileDirection boundaryDirection = curTileCell.CheckTileBoundary(destination);
		sTilePosition nextTilePos = new sTilePosition(_tileX, _tileY);
		TileHelper.GetNextTilePosByTileDirection(boundaryDirection, ref nextTilePos);

		if (tileSystem.CanMoveTileCell(nextTilePos.tileX, nextTilePos.tileY))
		{
			//타일 오프셋에 따른 캐릭터 y값 보정(des = des + (next.offset - cur.offset))
			if (eTileDirection.IN_TILE != boundaryDirection)
			{
				int layerOrder = tileSystem.GetTileCell(nextTilePos.tileX, nextTilePos.tileY).GetGroundLayerOrder();
				var sprites = GetComponentsInChildren<SpriteRenderer>(true);
				foreach(var spriteRenderer in sprites)
				{
					spriteRenderer.sortingOrder = layerOrder;
				}

				float curOffset = curTileCell.GetOffset();
				float nextOffset = tileSystem.GetTileCell(nextTilePos.tileX, nextTilePos.tileY).GetOffset();
				destination.y = destination.y + (nextOffset - curOffset);

				var layer = GetCurrentLayer();
				tileSystem.GetTileCell(_tileX, _tileY).RemoveObject(this, layer);
				tileSystem.GetTileCell(nextTilePos).AddObject(this, layer);
			}

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
		//else if ((x == 0) && (y == -1))
		//{
		//	trigger = "SOUTH";
		//	_lookAt = eDirection.SOUTH;
		//}
		//else if ((x == 0) && (y == 1))
		//{
		//	trigger = "NORTH";
		//	_lookAt = eDirection.NORTH;
		//}
		//else if ((x == 1) && (y == 0))
		//{
		//	trigger = "EAST";
		//	_lookAt = eDirection.EAST;
		//}
		//else if ((x == -1) && (y == 0))
		//{
		//	trigger = "WEST";
		//	_lookAt = eDirection.WEST;
		//}

		if (!trigger.Equals(""))
		{
			_lookDirection.x = x;
			_lookDirection.y = y;
			if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(trigger))
			{
				_animator.SetTrigger(trigger);
			}
		}
	}

	public eDirection LookAt() { return _lookAt; }
	public Vector2Int GetLookDirection() { return _lookDirection; }
	public Animator GetAnimator() { return _animator; }
	public AnimationPlayer GetAnimPlayer() { return _animPlayer; }

	#endregion

	public Transform GetTransform() { return _transform; }
	public SpriteRenderer GetSpriteRenderer() { return GetComponentInChildren<SpriteRenderer>(); }

	#region Message
	public override void ReceiveMessage(MessageParam msgParam)
	{
		switch(msgParam.message)
		{
			case "Attack":
				//Debug.Log(_transform.name + "..Sender: " + msgParam.sender.name);
				_receiveDamagedInfo = msgParam.damageInfo;
				ChangeState(eStateType.DAMAGE);
				break;
			default:
				break;
		}
	}
	#endregion

	#region Status, Battle etc
	protected sStatus _status;
	public delegate void OnHpChagned();
	public OnHpChagned onHpChangedCallback;

	Character _attackTarget = null;
	sDamageInfo _receiveDamagedInfo;
	sAttackInfo _attackInfo;

	public virtual void InitStatus()
	{
		_status = new sStatus();
		_status.maxHp = 100;
		_status.hp = 80;
		_status.maxMp = 100;
		_status.mp = 50;
		_status.attack = 10;
		_status.attackRange = 1;
		_status.armor = 0.0f;
		_status.avoid = 2;
		_status.speed = 2.0f;

		_attackCoolTime = 3.0f;

		_receiveDamagedInfo.damagePoint = 0;
		_receiveDamagedInfo.attackType = eDamageType.NORMAL;
	}

	public ref sStatus GetStatus()
	{
		return ref _status;
	}

	public Character GetAttackTarget() { return _attackTarget; }
	public void SetAttackTarget(Character target) { _attackTarget = target; }
	public void ResetAttackTarget() { _attackTarget = null; }

	public sDamageInfo GetReceiveDamagedInfo() { return _receiveDamagedInfo; }
	public void ResetDamagedInfo()
	{
		_receiveDamagedInfo.damagePoint = 0;
		_receiveDamagedInfo.attackType = eDamageType.NORMAL;
	}

	//다음에 공격할 공격정보를 세팅한다
	public void SetAttackInfo(sAttackInfo info) { _attackInfo = info; }
	public ref sAttackInfo GetAttackInfo() { return ref _attackInfo; }
	

	public void IncreaseHp(int value)
	{
		_status.hp += value;
		if (_status.hp >= _status.maxHp)
			_status.hp = _status.maxHp;

		if (onHpChangedCallback != null)
			onHpChangedCallback.Invoke();
	}

	public void DecreaseHp(int damage)
	{
		Debug.Log("<color=red> Damaged: " + damage + "</color>");
		_status.hp -= damage;
		if (_status.hp <= 0)
			_status.hp = 0;

		if (onHpChangedCallback != null)
			onHpChangedCallback.Invoke();
	}
	#endregion

	#region Cooltime, CastingTime

	void InitCoolTime()
	{
		_bAttackReady = true;
	}

	protected float _attackCoolTime = 3.0f;
	bool _bAttackReady;

	public bool IsAttackReady() { return _bAttackReady; }

	public void DoAttack()
	{
		_bAttackReady = false;
		StartCoroutine(UpdateAttackCoolTime(_attackCoolTime));
	}

	IEnumerator UpdateAttackCoolTime(float time)
	{
		yield return new WaitForSeconds(time);
		_bAttackReady = true;
	}

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
	public MapObject GetPathTarget() { return _target; }
	public void SetPathTarget(MapObject target) { _target = target;	}
	public void ResetPathTarget() {	_target = null;	}

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
			sTilePosition tilePos = item.GetTilePosition();
			TileCell tileCell = TileSystem.Instance.GetTileCell(tilePos);
			tileCell.RemoveObject(item, item.GetCurrentLayer());
			Destroy(item.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		
	}
}

public struct sStatus
{
	public int maxHp;
	public int maxMp;
	public int hp;
	public int mp;

	public int attack;
	public int attackRange;
	public float armor;
	public int avoid;

	public float speed;
}