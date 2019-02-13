using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject
{
	public float _speed = 10.0f;
	Transform _transform;

	private void Awake()
	{
		_transform = GetComponent<Transform>();
	}

	// Start is called before the first frame update
	void Start()
    {
		Init();
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
	}

	#region STATE
	State _curState;
	Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();

	void InitState()
	{
		ReplaceState(eStateType.IDLE, new IdleState());
		ReplaceState(eStateType.MOVE, new MoveState());

		_curState = _stateMap[eStateType.IDLE];
	}

	void ReplaceState(eStateType changeType, State replaceState)
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
			_tileX = nextTileX;
			_tileY = nextTileY;
			transform.position = new Vector3(destination.x, destination.y, 0.0f);
		}
	}

	public Transform GetTransform() { return _transform; }
	public float GetSpeed() { return _speed; }
}
