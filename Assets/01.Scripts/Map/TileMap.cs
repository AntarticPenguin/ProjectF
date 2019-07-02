using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		csvParser.ReadCSVByName(_mapCSV.name);
		int[] mapSize = csvParser.GetMapSize();
		_width = mapSize[0];
		_height = mapSize[1];

		InitTiles();
		CreateTiles(csvParser.GetMapData(), eTileLayer.GROUND);
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

				Vector3 pos = _grid.CellToWorld(new Vector3Int(x, y, 0));
				pos.y += (_grid.cellSize.y / 2) + offset;		//fit on grid for debug
				GetTileCell(x, y).Init(sortingOrder);
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
}
