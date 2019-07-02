using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.EventSystems;

public class TileMapEditor : MonoBehaviour
{
	enum eMouseOn
	{
		WORKSPACE,
		MENU,
	}
	eMouseOn _eMouseOn;

	public GameObject _gridPrefab;
	public Dropdown _dropDownMenu;
	public GameObject _dropDownContainer;
	public Text _canMoveText;

	public Grid _grid;
	public int _width;
	public int _height;
	public float _dragSpeed = 4.0f;

	float _gridSizeX;
	float _gridSizeY;
	Vector3 _cameraOldPos;
	bool _bDrag;

	GameObject _selectedMenuTile = null;
	GridTile _selectedTileObject = null;
	bool _bTransformMode;

	bool _bCanMove;

	//List<GridTile> _gridTiles = new List<GridTile>();
	List<List<GameObject>> _gridTiles = new List<List<GameObject>>();

	private void Awake()
	{
		_bDrag = false;
		_bTransformMode = false;
		_bCanMove = true;
		_eMouseOn = eMouseOn.WORKSPACE;
		_gridSizeX = _grid.cellSize.x;
		_gridSizeY = _grid.cellSize.y;
	}

	// Start is called before the first frame update
	void Start()
    {
		InitDropdownMenu();
		InitGridTiles(_width, _height);
	}

	// Update is called once per frame
	void Update()
	{
		//Camera Drag & Zoom
		{
			if (Input.GetMouseButtonDown(1))
			{
				_cameraOldPos = Camera.main.transform.position;
			}

			if (Input.GetMouseButton(1))
			{
				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				if (Mathf.Abs(_cameraOldPos.x - pos.x) > 0 ||
					Mathf.Abs(_cameraOldPos.y - pos.y) > 0)
				{
					if (false == _bDrag)
					{
						_cameraOldPos += pos * _dragSpeed;
						_bDrag = true;
					}
					Camera.main.transform.position = _cameraOldPos + -pos * _dragSpeed;
				}
			}

			if (Input.GetMouseButtonUp(1))
				_bDrag = false;

			float wheelInput = Input.GetAxis("Mouse ScrollWheel");
			if (eMouseOn.WORKSPACE == _eMouseOn && (wheelInput > 0 || wheelInput < 0))
			{
				Camera.main.orthographicSize += -wheelInput;
			}
		}

		//Tile Transform
		if (Input.GetKeyDown(KeyCode.T))
		{
			_bTransformMode = !_bTransformMode;
			_selectedMenuTile = null;
			_selectedTileObject = null;

			if (_bTransformMode)
				Debug.Log("Transform mode On");
			else
				Debug.Log("Transform mode Off");
		}

		//Raycast on MenuObject or TileObject
		if (Input.GetMouseButton(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
				return;

			if (null != _selectedMenuTile || _bTransformMode)
			{
				Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
				if (null != hit.collider)
				{
					GridTile tile = hit.collider.GetComponent<GridTile>();

					if (false == _bTransformMode)
					{
						Sprite sprite = _selectedMenuTile.GetComponent<Image>().sprite;
						tile.SetTileObjectBySprite(sprite);
						tile.SetCanMove(_bCanMove);
					}
					else
					{
						_selectedTileObject = tile;
					}
				}
			}
		}

		//Tile Transform
		if (null != _selectedTileObject)
		{
			if(null != _selectedTileObject.GetTileObject())
			{
				if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					_selectedTileObject.IncreasePosition();
				}
				else if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					_selectedTileObject.DecreasePosition();
				}
			}
		}
	}

	void InitGridTiles(int width, int height)
	{
		int sortingCount = width * height;

		for (int y = 0; y < height; y++)
		{
			List<GameObject> tempList = new List<GameObject>();
			for (int x = 0; x < width; x++)
			{
				GameObject go = Instantiate(_gridPrefab);
				go.InitTransformAsChild(transform);
				go.name = "GridTile(" + x + ", " + y + ")";

				GridTile tile = go.GetComponent<GridTile>();
				Vector3 pos = _grid.CellToWorld(new Vector3Int(x, y, 0));
				tile.transform.localPosition = pos;
				tile.MakeCollider(_gridSizeX, _gridSizeY);
				tile.MakeBoundaryLines(_gridSizeX, _gridSizeY);
				tile.SetSortingOrder(sortingCount--);

				tempList.Add(go);
			}
			_gridTiles.Add(tempList);
		}

		_width = width;
		_height = height;
	}

	void ResetGridTiles()
	{
		for(int i = 0; i < _gridTiles.Count; i++)
		{
			List<GameObject> list = _gridTiles[i];
			for(int j = 0; j < list.Count; j++)
			{
				Destroy(list[j]);
			}
		}
		_gridTiles.Clear();
	}

	Dictionary<eTileAsset, List<GameObject>> _dropDownList = new Dictionary<eTileAsset, List<GameObject>>();
	Dictionary<string, Sprite> _spriteMap = new Dictionary<string, Sprite>();
	void InitDropdownMenu()
	{
		for(int i = 0; i < (int)eTileAsset.MAX; i++)
		{
			Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tile/" + ((eTileAsset)i).ToString());
			for(int j = 0; j < sprites.Length; j++)
				_spriteMap[sprites[j].name] = sprites[j];

			List<GameObject> dropDownElements = new List<GameObject>();

			for (int index = 0; index < sprites.Length; index++)
			{
				GameObject go = Resources.Load<GameObject>("Prefabs/Editor/UI/ListElementBtn");
				GameObject btnPrefab = Instantiate(go);
				btnPrefab.transform.SetParent(_dropDownContainer.transform);
				btnPrefab.name = sprites[index].name;

				Image img = btnPrefab.GetComponent<Image>();
				img.sprite = sprites[index];

				Button btn = btnPrefab.GetComponent<Button>();
				btn.onClick.AddListener(delegate {
					OnButtonClick(btnPrefab);
				});

				dropDownElements.Add(btnPrefab);
			}

			_dropDownList.Add((eTileAsset)i, dropDownElements);
		}

		_dropDownMenu.onValueChanged.AddListener(delegate {
			DropdownValueChanged(_dropDownMenu);
		});

		_dropDownMenu.onValueChanged.Invoke(0);
	}

	#region UI EVENT
	public void OpenFile()
	{
		string assetPath = Application.dataPath;    //Get path "/Assets"
		string path = EditorUtility.OpenFilePanel("OpenFile", assetPath + "/Resources/", "csv");
		if (0 != path.Length)
		{
			ResetGridTiles();
			CSVParser csvParser = new CSVParser();
			csvParser.ReadCSV(path);
			int[] mapSize = csvParser.GetMapSize();

			InitGridTiles(mapSize[0], mapSize[1]);

			List<List<string>> mapData = csvParser.GetMapData();
			for(int row = 0; row < mapData.Count; row++)
			{
				List<string> rowData = mapData[row];
				for(int index = 0; index < rowData.Count; index++)
				{
					GridTile tile = _gridTiles[row][index].GetComponent<GridTile>();
					string[] info = rowData[index].Split('@');
					string spriteName = info[0];
					if (spriteName.Equals("none"))
						continue;

					Sprite sprite = _spriteMap[spriteName];
					if(3 == info.Length)
					{
						string offset = info[1];
						tile.SetTileObjectBySprite(sprite, float.Parse(offset));
						tile.SetCanMove(info[2]);
					}
					else
					{
						tile.SetTileObjectBySprite(sprite);
					}
				}
			}
		}
	}

	public void SaveFile()
	{
		string assetPath = Application.dataPath;    //Get path "/Assets"
		string path = EditorUtility.SaveFilePanel("SaveFile", assetPath + "/Resources/MapData", "", "csv");
		if(0 != path.Length)
		{
			CSVParser csvParser = new CSVParser();
			csvParser.SaveCSV(_width, _height, _gridTiles, path);
			AssetDatabase.Refresh();
		}
	}

	public void TurnOnOffGrid()
	{
		for (int y = 0; y < _height; y++)
		{
			for(int x = 0; x < _width; x++)
			{
				_gridTiles[y][x].GetComponent<GridTile>().TurnOnOff();
			}
		}
	}

	void DropdownValueChanged(Dropdown change)
	{
		eTileAsset selected = (eTileAsset)System.Enum.Parse(typeof(eTileAsset), _dropDownMenu.options[change.value].text);
		for (int i = 0; i < (int)eTileAsset.MAX; i++)
		{
			eTileAsset tileGroup = (eTileAsset)i;
			List<GameObject> list = _dropDownList[tileGroup];
			if(tileGroup == selected)
			{
				for(int j = 0; j < list.Count; j++)
					list[j].SetActive(true);
			}
			else
			{
				for (int j = 0; j < list.Count; j++)
					list[j].SetActive(false);
			}
		}
	}

	void OnButtonClick(GameObject tileObject)
	{
		_selectedMenuTile = tileObject;
	}

	public void MouseOnMenu()
	{
		_eMouseOn = eMouseOn.MENU;
	}

	public void MouseExitMenu()
	{
		_eMouseOn = eMouseOn.WORKSPACE;
	}

	public void CanMoveSwitch()
	{
		if (true == _bCanMove)
		{
			_bCanMove = false;
			_canMoveText.text = "Switch CanMove To True";
		}
		else if (false == _bCanMove)
		{
			_bCanMove = true;
			_canMoveText.text = "Switch CanMove To False";
		}
		Debug.Log("CanMove: " + _bCanMove.ToString());
	}

	public void ShowCanMove()
	{
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				_gridTiles[y][x].GetComponent<GridTile>().ShowCanMove();
			}
		}
	}

	#endregion
}
