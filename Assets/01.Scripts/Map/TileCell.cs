using System.Collections.Generic;
using UnityEngine;

public class TileCell
{
	//static int pixelPerUnit = 128;

	Vector2 _position;
	int _tileX;
	int _tileY;
	bool _bCanMove;

	//Grid쓰니까 이제 필요없음
	//float _width = 103.0f / pixelPerUnit;
	//float _height = 49.6f / pixelPerUnit;   //타일 윗면의 아래꼭지점부터 위까지의 높이

	List<List<MapObject>> _mapObjectListByLayer = new List<List<MapObject>>();
	int _groundLayerOrder;				//타일셀이 가지고 있는 고유값
	int _itemLayerOrder;

	public void Init(int groundOrder)
	{
		for(int i = 0; i < (int)eTileLayer.MAXCOUNT; i++)
		{
			List<MapObject> mapObjects = new List<MapObject>();
			_mapObjectListByLayer.Add(mapObjects);
		}
		_bCanMove = true;
		_bIsVisit = false;

		_groundLayerOrder = groundOrder;
		_itemLayerOrder = 0;

		_distanceFromStart = 0.0f;
		_weight = 1.0f;
		_prevCell = null;
	}

	//타일셀 정보에 추가
	public void AddObject(MapObject mapObject, eTileLayer layer, bool setTilePos = true)
	{
		mapObject.SetCurrentLayer(layer);

		if (eTileLayer.ITEM == layer)
		{
			//아이템: 최신이 제일 앞에 오게
			mapObject.GetComponent<SpriteRenderer>().sortingOrder = _itemLayerOrder;
			_itemLayerOrder++;
		}
		else
		{
			if(eMapObjectType.CHARACTER != mapObject.GetMapObjectType() &&
				eMapObjectType.PLAYER != mapObject.GetMapObjectType() &&
				eMapObjectType.ENEMY != mapObject.GetMapObjectType() )
			{
				mapObject.GetComponent<SpriteRenderer>().sortingOrder = _groundLayerOrder;
			}
		}

		List<MapObject> mapObjectList = _mapObjectListByLayer[(int)layer];
		if (!mapObjectList.Contains(mapObject)) //캐릭터 중복삽입 방지
			mapObjectList.Add(mapObject);

		int sortingLayerID = SortingLayer.NameToID(layer.ToString());
		mapObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		if(setTilePos)
			mapObject.SetTilePosition(_tileX, _tileY);
	}

	//타일셀 정보에서 제거
	public void RemoveObject(MapObject mapObject)
	{
		eTileLayer currentLayer = mapObject.GetCurrentLayer();
		if(eTileLayer.NONE != currentLayer)
		{
			if(eTileLayer.ITEM == currentLayer)
			{
				_itemLayerOrder--;
			}

			List<MapObject> mapObjectList = _mapObjectListByLayer[(int)currentLayer];
			mapObjectList.Remove(mapObject);
		}
		else
		{
			Debug.Log("TileLayer is NONE");
		}
	}

	//타일셀 정보 추가 및 포지션까지 세팅
	public void SetObject(MapObject mapObject, eTileLayer layer)
	{
		AddObject(mapObject, layer);

		mapObject.SetPosition(_position);
	}

	public MapObject FindObjectByType(eMapObjectType mapObjectType, eTileLayer layer)
	{
		List<MapObject> mapObjects = _mapObjectListByLayer[(int)layer];
		for(int i = 0; i < mapObjects.Count; i++)
		{
			if(mapObjectType == mapObjects[i].GetMapObjectType())
			{
				return mapObjects[i];
			}
		}
		return null;
	}

	public void SetPosition(Vector2 position)
	{
		_position = position;
	}

	public Vector2 GetPosition()
	{
		return _position;
	}

	public void SetTilePosition(int tileX, int tileY)
	{
		_tileX = tileX;
		_tileY = tileY;
	}
	
	public int GetTileX() { return _tileX; }
	public int GetTileY() { return _tileY; }
	public sTilePosition GetTilePosition()
	{
		return new sTilePosition(_tileX, _tileY);
	}

	public void SetCanMove(bool canMove)
	{
		_bCanMove = canMove;
	}

	public bool CanMove()
	{
		return _bCanMove;
	}

	public int GetGroundLayerOrder() { return _groundLayerOrder; }

	//깔린 바닥 타일 오브젝트 정보
	TileObject _baseTileObject;
	public void SetTileObject(TileObject tileObject)
	{
		_baseTileObject = tileObject;
	}

	public TileObject GetBaseTileObject() { return _baseTileObject; }

	public sTileProperties GetProperties(eTileLayer layer)
	{
		sTileProperties tileProperties = new sTileProperties();
		tileProperties.speed = 0.0f;

		List<MapObject> mapObjectList = _mapObjectListByLayer[(int)layer];
		for(int i = 0; i < mapObjectList.Count; i++)
		{
			if (eMapObjectType.TILEOBJECT == mapObjectList[i].GetMapObjectType())
			{
				TileObject tileObject = mapObjectList[i].GetComponent<TileObject>();
				tileProperties = tileObject.GetProperties();
				return tileProperties;
			}
		}
		return tileProperties;
	}

	float _offset = 0.0f;
	public void SetOffset(float offset) { _offset = offset; }
	public float GetOffset() { return _offset; }

	public void Clear()
	{
		for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
		{
			List<MapObject> mapObjectList = _mapObjectListByLayer[(int)layer];
			for(int i = 0; i < mapObjectList.Count; i++)
			{
				Object.Destroy(mapObjectList[i].gameObject);
			}
		}
	}

	#region Check Tile Boundary

	public eTileDirection CheckTileBoundary(Vector2 destination)
	{
		//Grid grid = GameManager.Instance.GetMap().GetGrid();
		Grid grid = TileSystem.Instance.GetGrid();
		float width = grid.cellSize.x;
		float height = grid.cellSize.y;

		Vector2 leftPoint = new Vector2(_position.x - width / 2, _position.y);
		Vector2 downPoint = new Vector2(_position.x, _position.y - height / 2);
		Vector3 upPoint = new Vector2(_position.x, _position.y + height / 2);

		float downTan = (downPoint.y - leftPoint.y) / (downPoint.x - leftPoint.x);  //타일 아래변 두개 기울기
		float upTan = (upPoint.y - leftPoint.y) / (upPoint.x - leftPoint.x);        //타일 윗변 두개 기울기

		if (CheckUpsideSlope(downTan, downPoint, destination) == false)
			return eTileDirection.SOUTH_WEST;
		if (CheckUpsideSlope(-downTan, downPoint, destination) == false)
			return eTileDirection.SOUTH_EAST;
		if (CheckUpsideSlope(upTan, upPoint, destination) == true)
			return eTileDirection.NORTH_WEST;
		if (CheckUpsideSlope(-upTan, upPoint, destination) == true)
			return eTileDirection.NORTH_EAST;
		return eTileDirection.IN_TILE;
	}

	bool CheckUpsideSlope(float slope, Vector2 onSlopePosition, Vector2 destination)
	{
		//직선식에 대입해서
		//0보다 작으면 위(true)
		//0보다 크면 아래(false)
		bool upSide = slope * (destination.x - onSlopePosition.x) - (destination.y - onSlopePosition.y) < 0;
		return upSide;
	}

	#endregion

	#region PATHFINDING
	bool _bIsVisit;
	public void Visit() { _bIsVisit = true; }
	public bool IsVisit() { return _bIsVisit; }
	public void ResetPathfindInfo()
	{
		_bIsVisit = false;
		_prevCell = null;
		_distanceFromStart = 0.0f;
	}

	float _distanceFromStart;
	float _weight;
	public float GetDistanceFromStart() { return _distanceFromStart; }
	public void SetDistanceFromStart(float distance) { _distanceFromStart = distance; }
	public float GetDistanceWeight() { return _weight; }

	TileCell _prevCell;
	public TileCell GetPrevCell() { return _prevCell; }
	public void SetPrevCell(TileCell tileCell) { _prevCell = tileCell; }

	//TEST
	SpriteRenderer _spriteRenderer;
	public void SetSpriteRenderer(SpriteRenderer spriteRenderer) { _spriteRenderer = spriteRenderer; }
	public void DrawColor(Color color) { _spriteRenderer.color = color;	}
	public void ResetColor() { _spriteRenderer.color = Color.white; }

	#endregion

	//TEST
	public bool InCharacter()
	{
		if (FindObjectByType(eMapObjectType.CHARACTER, eTileLayer.GROUND) != null ||
			FindObjectByType(eMapObjectType.PLAYER, eTileLayer.GROUND) != null)
			return true;
		return false;
	}
}
