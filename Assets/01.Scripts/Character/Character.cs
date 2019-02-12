using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject
{
	public float _speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		Vector2 direction= new Vector2();

		if (Input.GetKey(KeyCode.UpArrow))
		{
			direction += new Vector2(0.0f, 1.0f);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			direction += new Vector2(0.0f, -1.0f);
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			direction += new Vector2(-1.0f, 0.0f);
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			direction += new Vector2(1.0f, 0.0f);
		}

		TileMap map = GameManager.Instance.GetMap();
		if (Input.GetKeyDown(KeyCode.T))
		{
			Debug.Log("TileX: " + _tileX + ", TileY: " + _tileY + ", " + map.GetTileCell(_tileX, _tileY).GetPosition() +
				", " + "Char: " + transform.position);
		}

		Vector2 position = _speed * direction.normalized * Time.deltaTime;
		Vector2 nextPosition = (Vector2)transform.position + position;

		//1. 다음 포지션 체크
		//	- 타일을 나갈 경우 어느방향으로 나갔는지 확인
		//	- 해당 타일에 갈 수 있는 지 체크(타일 범위 밖 or 물체가 있어서 block)

		//2. 1에서 갈 수 있다면, 위치 세팅 및 타일 위치 업데이트
		UpdateNextPosition(nextPosition);
	}

	public void Init()
	{

	}

	void UpdateNextPosition(Vector2 nextPosition)
	{
		TileMap map = GameManager.Instance.GetMap();
		eTileDirection nextDirection = map.GetTileCell(_tileX, _tileY).CheckTileDirection(nextPosition);
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
			transform.position = new Vector3(nextPosition.x, nextPosition.y, 0.0f);
		}
	}
}
