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
		_boundary = GetComponent<SpriteRenderer>().bounds.size.x;
		//Debug.Log("boundary size: " + _boundary.ToString("F4"));

		InitState();
		_animator = GetComponent<Animator>();

		//up, down, left, right
		_boundaryPos.Add(new Vector2(0.0f, _boundary));
		_boundaryPos.Add(new Vector2(0.0f, -_boundary));
		_boundaryPos.Add(new Vector2(-_boundary, 0.0f));
		_boundaryPos.Add(new Vector2(_boundary, 0.0f));

		for(int i = 0; i < (int)eBoundary.MAX; i++)
		{
			sTilePosition tilePosition = new sTilePosition(-1, -1);
			_boundaryMap[(eBoundary)i] = tilePosition;
		}
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

	#region POSITION AND BOUNDARY
	float _boundary;
	Dictionary<eBoundary, sTilePosition> _boundaryMap = new Dictionary<eBoundary, sTilePosition>();
	List<Vector2> _boundaryPos = new List<Vector2>();		//캐릭터 중심좌표를 기준으로 경계

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

		//UpdateBoundary();
	}

	void UpdateBoundary()
	{
		/*
		 *		밟고 있는 타일셀을 일단 검사
		 *		검사를 마친 후, 타일셀들에 AddObject 호출
		 */
		TileMap map = GameManager.Instance.GetMap();
		for(int direction = 0; direction < (int)eBoundary.MAX; direction++)
		{
			Vector2 pos = (Vector2)transform.position + _boundaryPos[direction];
			eTileDirection boundaryDirection = map.GetTileCell(_tileX, _tileY).CheckTileBoundary(pos);
			if(eTileDirection.IN_TILE == boundaryDirection)		//안에서 -> 안으로
			{
				sTilePosition tilePos = new sTilePosition(-1, -1);
				_boundaryMap[(eBoundary)direction] = tilePos;
			}

			Debug.Log(((eBoundary)direction).ToString() + ": OK");
		}
	}
	#endregion

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
