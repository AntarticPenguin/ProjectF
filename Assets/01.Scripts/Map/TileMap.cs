using System.Collections.Generic;
using UnityEngine;
using System;

public class TileMap : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

	}

	int _width;
	int _height;

	TileCell[,] _tileCellList;

	[SerializeField]
	private Grid _grid;

	public GameObject _tileObjectPrefab;
	public TextAsset _mapCSV;

	public void Init()
	{
		CSVParser csvParser = new CSVParser();
		csvParser.ReadMapCSV(_mapCSV.name);
		int[] mapSize = csvParser.GetMapSize();
		_width = mapSize[0];
		_height = mapSize[1];

		InitTiles();
		CreateTiles(csvParser.GetMapData(), eTileLayer.GROUND);
		CreatePortal();
	}

	void InitTiles()
	{
		_tileCellList = new TileCell[_height, _width];
		for(int y = 0; y < _height; ++y)
		{
			for(int x = 0; x < _width; ++x)
			{
				_tileCellList[y, x] = new TileCell();
			}
		}
	}

	void CreateTiles(List<List<string>> mapData, eTileLayer layer)
	{
		int sortingOrder = _width * _height;
		for(int y = 0; y < _height; ++y)
		{
			for(int x = 0; x < _width; ++x)
			{
				GameObject tileObjectPrefab = Instantiate(_tileObjectPrefab);
				tileObjectPrefab.InitTransformAsChild(transform);

				TileObject tileObject = tileObjectPrefab.GetComponent<TileObject>();
				tileObject.name = "(" + x + ", " + y + ")";
				tileObject.SetTilePosition(x, y);

				string[] info = mapData[y][x].Split('@');
				string tileName = info[0];
				float offset = 0.0f;
				bool canMove = true;
				if (3 == info.Length)
				{
					offset = float.Parse(info[1]);
					if (info[2].Equals("True"))
						canMove = true;
					else if (info[2].Equals("False"))
						canMove = false;
				}
					
				SpriteRenderer spriteRenderer = tileObjectPrefab.GetComponent<SpriteRenderer>();
				spriteRenderer.sprite = ResourceManager.Instance.FindSpriteByName(tileName);
				GetTileCell(x, y).SetSpriteRenderer(spriteRenderer);

				Vector3 pos = _grid.CellToWorld(new Vector3Int(x, y, 0));
				pos.y += (_grid.cellSize.y / 2) + offset;		//fit on grid for debug
				GetTileCell(x, y).Init(sortingOrder);
				GetTileCell(x, y).SetOffset(offset);
				GetTileCell(x, y).SetTilePosition(x, y);
				GetTileCell(x, y).SetPosition(pos);
				GetTileCell(x, y).SetObject(tileObject, layer);
				GetTileCell(x, y).SetCanMove(canMove);
				if (null == spriteRenderer.sprite)
					GetTileCell(x, y).SetCanMove(false);

				--sortingOrder;
			}
		}
	}

	Dictionary<string, sPortalInfo> _portalInfo = new Dictionary<string, sPortalInfo>();
	void CreatePortal()
	{
		CSVParser parser = new CSVParser();
		List<sPortalInfo> info = parser.ReadMapInfo(_mapCSV.name);

		for(int i = 0; i < info.Count; i++)
		{
			MapObjectSpawner.Instance.CreatePortal(info[i]);
			_portalInfo.Add(info[i].portalName, info[i]);
		}
	}

	//TODO: 맵로딩 delegate
	public delegate void OnLoadMapFinished();
	public OnLoadMapFinished onLoadMapFinished;

	public void LoadMap(ref sPortalInfo info)
	{
		string[] tokens = info.nextMap.Split('-');
		string mapName = tokens[0];
		string portalName = info.nextMap;

		string path = "MapData/" + mapName;
		TextAsset asset = Resources.Load<TextAsset>(path);
		if (asset != null)
		{
			Debug.Log("LoadMap: " + asset.name);
			_mapCSV = asset;
		}
		else
		{
			Debug.Log("Failed load map");
		}

		Init();

		sPortalInfo spawnPortal = _portalInfo[portalName];
		Character player = MapObjectSpawner.Instance.CreateCharacter(spawnPortal.tileX, spawnPortal.tileY, "Player", "Isolet_Test");
		player.Init();
		GameManager.Instance.SetPlayer(player);
		GameManager.Instance.BecomeViewer(player);
	}

	public int GetWidth() { return _width; }
	public int GetHeight() { return _height; }

	public TileCell GetTileCell(int x, int y)
	{
		if (_width <= x || x < 0)
			return null;
		if (_height <= y || y < 0)
			return null;
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

	public void ClearMap()
	{
		for(int y = 0; y < _height; y++)
		{
			for(int x = 0; x < _width; x++)
			{
				GetTileCell(x, y).Clear();
			}
		}
		Array.Clear(_tileCellList, 0, _height * _width);
		_portalInfo.Clear();
	}

	public List<MapObject> FindObjectsByRange(eMapObjectType type, eTileLayer layer, TileCell center, int range = 1)
	{
		int minX = center.GetTileX() - range;
		int minY = center.GetTileY() - range;
		int maxX = center.GetTileX() + range;
		int maxY = center.GetTileY() + range;

		if (minX < 0) minX = 0;
		if (minY < 0) minY = 0;
		if (_width <= maxX) maxX = _width;
		if (_height <= maxY) maxY = _height;

		List<MapObject> mapObjects = new List<MapObject>();
		for (int y = minY; y < maxY; y++)
		{
			for(int x = minX; x < maxX; x++)
			{
				var mapObject = GetTileCell(x, y).FindObjectByType(type, layer);
				if (null != mapObject)
					mapObjects.Add(mapObject);
			}
		}

		if (0 == mapObjects.Count)
			return null;
		return mapObjects;
	}
}
