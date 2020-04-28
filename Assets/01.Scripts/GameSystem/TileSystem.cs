using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSystem : Singleton<TileSystem>
{
	TileCell[,] _tileCellList;
	int _width;
	int _height;

	Grid _grid;
	Dictionary<eTilemapType, Tilemap> _tilemaps = new Dictionary<eTilemapType, Tilemap>();

	public void Init()
	{
		//TEST
		_width = 16;
		_height = 16;
		_tileCellList = new TileCell[_height, _width];
		for(int y = 0; y < _height; y++)
		{
			for(int x = 0; x < _width; x++)
			{
				_tileCellList[y, x] = new TileCell();
			}
		}
		GameObject rootScene = GameObject.Find("MainGameScene");
		Transform gridObject = rootScene.transform.Find("Grid");
		_grid = gridObject.GetComponent<Grid>();
		Tilemap[] maps = gridObject.GetComponentsInChildren<Tilemap>();
		for (int i = 0; i < maps.Length; i++)
		{
			eTilemapType type = (eTilemapType)i;
			_tilemaps.Add(type, maps[i]);
		}

		InitTileCell();
	}

	//GROUND 타일맵에 깔린 타일 오브젝트들 정보
	//타일 정렬되어 있어야함!!
	void InitTileCell()
	{
		eTileLayer layer = eTileLayer.GROUND;
		for(int y = 0; y < _height; y++)
		{
			for(int x = 0; x < _width; x++)
			{
				int index = y * _width + x;
				GameObject tile = _tilemaps[eTilemapType.GROUND].transform.GetChild(index).gameObject;
				TileObject tileObject = tile.GetComponent<TileObject>();

				GetTileCell(x, y).Init(tile.GetComponent<SpriteRenderer>().sortingOrder);

				Vector3 pos = _grid.CellToWorld(new Vector3Int(x, y, 0));
				float gridCenterY = (pos.y + _grid.cellSize.y / 2);
				float offset = 0.0f;
				if (tile.transform.position.y.EqualApproximately(gridCenterY, 0.01f))
				{
					offset = 0.0f;
				}
				else
				{
					if (tile.transform.position.y < gridCenterY)
						offset = gridCenterY - tile.transform.position.y;
					else
						offset = tile.transform.position.y - gridCenterY;
				}
				//test pathfinding 
				SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
				GetTileCell(x, y).SetSpriteRenderer(spriteRenderer);

				GetTileCell(x, y).SetTileObject(tileObject);
				GetTileCell(x, y).SetOffset(offset);
				GetTileCell(x, y).SetPosition(tile.transform.position);				//객체 월드 포지션
				GetTileCell(x, y).SetTilePosition(x, y);		//타일 좌표
				GetTileCell(x, y).AddObject(tileObject, layer);
				Tile baseTile = _tilemaps[eTilemapType.BLOCK].GetTile(new Vector3Int(x, y, 0)) as Tile;
				if(baseTile != null && baseTile.sprite != null)
				{
					GetTileCell(x, y).SetCanMove(false);
				}
				else
				{
					GetTileCell(x, y).SetCanMove(true);
				}
			}
		}
	}

	public Tilemap GetTilemap(eTilemapType type)
	{
		return _tilemaps[type];
	}

	public TileCell GetTileCell(int x, int y)
	{
		if (_width <= x || x < 0) return null;
		if (_height <= y || y < 0) return null;
		return _tileCellList[y, x];
	}

	public TileCell GetTileCell(sTilePosition tilePosition)
	{
		return GetTileCell(tilePosition.tileX, tilePosition.tileY);
	}

	public bool CanMoveTileCell(int x, int y)
	{
		if (x < 0)
			return false;
		if (_width <= x)
			return false;
		if (y < 0)
			return false;
		if (_height <= y)
			return false;

		if (false == GetTileCell(x, y).CanMove())
			return false;

		return true;
	}

	public Grid GetGrid() { return _grid; }
	public float GetSlope()
	{
		Vector3 pos1 = _grid.CellToWorld(Vector3Int.zero);
		Vector3 pos2 = _grid.CellToWorld(new Vector3Int(1, 0, 0));
		float slope = (pos2.y - pos1.y) / (pos2.x - pos1.x);
		return slope;
	}

	public HashSet<MapObject> FindObjectsByRange(eMapObjectType type, eTileLayer layer, TileCell center, int range = 1)
	{
		int minX = center.GetTileX() - range;
		int minY = center.GetTileY() - range;
		int maxX = center.GetTileX() + range;
		int maxY = center.GetTileY() + range;

		if (minX < 0) minX = 0;
		if (minY < 0) minY = 0;
		if (_width <= maxX) maxX = _width;
		if (_height <= maxY) maxY = _height;

		HashSet<MapObject> mapObjects = new HashSet<MapObject>();
		for (int y = minY; y <= maxY; y++)
		{
			for (int x = minX; x <= maxX; x++)
			{
				var mapObject = GetTileCell(x, y)?.FindObjectByType(type, layer);
				if (null != mapObject)
				{
					mapObjects.Add(mapObject);
				}
					
			}
		}

		if (0 == mapObjects.Count)
			return null;
		return mapObjects;
	}

	#region PATHFINDING

	public void ResetPathfindInfo()
	{
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				GetTileCell(x, y).ResetPathfindInfo();
			}
		}
	}

	//TEST
	public void ResetAllColor()
	{
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				GetTileCell(x, y).ResetColor();
			}
		}
	}

	#endregion
}
