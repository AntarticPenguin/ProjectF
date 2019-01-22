using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		InitTiles();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public int _width = 5;
	public int _height = 5;
	float _xInterval = 0.545f;
	float _yInterval = 0.28f;
	float _xStartPos = 0.0f;

	TileCell[,] _tileCellList;

	public GameObject _tileObjectPrefab;

	void InitTiles()
	{
		_tileCellList = new TileCell[_width, _height];

		for(int y = 0; y < _height; ++y)
		{
			for(int x = 0; x < _width; ++x)
			{
				_tileCellList[y, x] = new TileCell();
			}
		}

		CreateTiles(eTileLayer.GROUND);
		//CreateTiles(eTileLayer.MIDDLE_GROUND);

		TileMap map = GameManager.Instance.GetMap();

		//test create character
		string filePath = "Prefabs/CharacterSample";
		GameObject charPrefabs = Resources.Load<GameObject>(filePath);
		GameObject characterObject = Instantiate(charPrefabs);
		characterObject.transform.SetParent(map.transform);
		characterObject.transform.localPosition = Vector3.zero;
		characterObject.transform.localScale = new Vector2(2.0f, 2.0f);

		MapObject character = characterObject.AddComponent<MapObject>();
		map.GetTileCell(0, 0).AddObject(character, eTileLayer.GROUND, 255);

		Camera.main.transform.SetParent(character.transform);
		Camera.main.transform.localPosition = new Vector3(0.0f, 0.0f, Camera.main.transform.localPosition.z);
		Camera.main.transform.localScale = Vector3.one;
	}

	void CreateTiles(eTileLayer layer)
	{
		int sortingOrder = _width * _height;

		for (int y = 0; y < _height; ++y)
		{
			_xStartPos = _xInterval * -y;
			for (int x = 0; x < _width; ++x)
			{
				GameObject tileGameObject = Instantiate(_tileObjectPrefab);
				tileGameObject.transform.SetParent(transform);
				tileGameObject.transform.localPosition = Vector3.zero;
				tileGameObject.transform.localScale = Vector3.one;

				TileObject tileObject = tileGameObject.GetComponent<TileObject>();
				tileObject.SetTilePosition(x, y);

				// x행, y열
				// y = 0.514x + (열 + 층수)
				float xPos = _xInterval * x + _xStartPos;
				float yPos = 0.514f * (xPos + _xInterval * (y + (int)layer)) + (_yInterval * (y + (int)layer));
				GetTileCell(x, y).SetPosition(new Vector2(xPos, yPos));
				GetTileCell(x, y).AddObject(tileObject, layer, sortingOrder);
				--sortingOrder;
			}
		}
	}

	public TileCell GetTileCell(int x, int y)
	{
		return _tileCellList[y, x];
	}
}
