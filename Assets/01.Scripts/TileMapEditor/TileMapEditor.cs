using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Text;

public enum eTileAsset
{
	Grass,
	Snow,
	MAX,
}

public class TileMapEditor : MonoBehaviour
{
	public GameObject _sampleTileObject;
	public GameObject _gridPrefab;
	public Dropdown _dropDownMenu;
	public GameObject _dropDownContainer;

	public Grid _grid;
	public int _width;
	public int _height;
	public float _dragSpeed = 4.0f;

	float _gridSizeX;
	float _gridSizeY;
	Vector3 _cameraOldPos;
	bool _bDrag;

	//List<GridTile> _gridTiles = new List<GridTile>();
	List<List<GameObject>> _gridTiles = new List<List<GameObject>>();

	private void Awake()
	{
		_bDrag = false;
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
		if (wheelInput > 0 || wheelInput < 0)
		{
			Camera.main.orthographicSize += -wheelInput;
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (null != _selectedObject)
			{
				Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
				if (null != hit.collider)
				{
					GridTile tile = hit.collider.GetComponent<GridTile>();

					Sprite sprite = _selectedObject.GetComponent<Image>().sprite;
					tile.SetTileObjectBySprite(sprite);
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
			Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/" + ((eTileAsset)i).ToString());
			for(int j = 0; j < sprites.Length; j++)
				_spriteMap[sprites[j].name] = sprites[j];

			List<GameObject> dropDownElements = new List<GameObject>();

			for (int index = 0; index < sprites.Length; index++)
			{
				GameObject go = Resources.Load<GameObject>("Prefabs/UI/ListElementBtn");
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
		string path = EditorUtility.OpenFilePanel("OpenFile", "", "csv");
		if (0 != path.Length)
		{
			ResetGridTiles();
			StreamReader sr = new StreamReader(path);
			{
				//parsing map size
				string records = sr.ReadLine();
				string[] tokens = records.Split(',');
				int width = int.Parse(tokens[1]);
				int height = int.Parse(tokens[2]);
				InitGridTiles(width, height);
			}
			{
				//parsing map data
				sr.ReadLine();			//skip first line
				int row = 0;
				while (!sr.EndOfStream)
				{
					string records = sr.ReadLine();
					string[] tokens = records.Split(',');
					for(int i = 0; i < tokens.Length; i++)
					{
						GridTile tile = _gridTiles[row][i].GetComponent<GridTile>();
						string spriteName = tokens[i];
						if (spriteName.Equals("none"))
							continue;
						Sprite sprite = _spriteMap[spriteName];
						tile.SetTileObjectBySprite(sprite);
					}
					row++;
				}
			}
		}
	}

	public void SaveFile()
	{
		string path = EditorUtility.SaveFilePanel("SaveFile", "", "", "csv");
		if(0 != path.Length)
		{
			List<List<string>> rowData = new List<List<string>>();
			{
				//ex)mapSize width, height
				List<string> rowTemp = new List<string>();
				rowTemp.Add("mapSize");
				rowTemp.Add(_width.ToString());
				rowTemp.Add(_height.ToString());
				rowData.Add(rowTemp);
			}
			{
				//ex)mapData
				List<string> rowTemp = new List<string>();
				rowTemp.Add("mapData");
				rowData.Add(rowTemp);
			}
			{
				for(int y = 0; y < _height; y++)
				{
					List<string> rowTemp = new List<string>();
					for (int x = 0; x < _width; x++)
					{
						string tileName = _gridTiles[y][x].GetComponent<GridTile>()._spriteName;
						rowTemp.Add(tileName);
					}
					rowData.Add(rowTemp);
				}
			}
			{
				//output with delimeter ","
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < rowData.Count; i++)
				{
					for(int j = 0; j < rowData[i].Count; j++)
					{
						if(j != rowData[i].Count - 1)
						{
							sb.Append(rowData[i][j] + ",");
						}
						else
						{
							sb.AppendLine(rowData[i][j]);
						}
					}
				}
				
				StreamWriter outStream = File.CreateText(path);
				outStream.Write(sb);
				outStream.Close();

				Debug.Log("Save completed: " + path);
			}
		}
	}

	public void TurnOnOffGrid()
	{
		for(int y = 0; y < _height; y++)
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

	GameObject _selectedObject = null;
	void OnButtonClick(GameObject btn)
	{
		_selectedObject = btn;
	}
	#endregion
}
