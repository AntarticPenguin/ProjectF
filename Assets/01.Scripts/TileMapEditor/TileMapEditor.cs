using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileMapEditor : MonoBehaviour
{
	public GameObject _sampleTileObject;
	public GameObject _gridPrefab;

	public Grid _grid;
	public int _width;
	public int _height;
	public float _dragSpeed = 4.0f;

	Vector3 _cameraOldPos;
	bool _bDrag;

	List<GridTile> _gridTiles = new List<GridTile>();

	private void Awake()
	{
		_bDrag = false;
	}

	// Start is called before the first frame update
	void Start()
    {
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				GameObject go = Instantiate(_gridPrefab);
				go.InitTransformAsChild(transform);
				GridTile tile = go.GetComponent<GridTile>();
				Vector3 pos = _grid.CellToWorld(new Vector3Int(x, y, 0));
				tile.transform.localPosition = pos;
				tile.MakeCollider(1.1f, 0.5f);
				tile.MakeBoundaryLines(1.1f, 0.5f);

				_gridTiles.Add(tile);
			}
		}

		int sortingCount = _width * _height;
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				GameObject gameObject = Instantiate(_sampleTileObject);
				gameObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("GROUND");
				gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingCount--;
				Vector3 pos = _grid.CellToLocal(new Vector3Int(x, y, 1));
				pos += new Vector3(0.0f, 0.27f, 0.0f);
				gameObject.transform.SetParent(transform);
				gameObject.transform.position = pos;
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
		if(Input.GetMouseButtonDown(1))
		{
			_cameraOldPos = Camera.main.transform.position;
		}

		if(Input.GetMouseButton(1))
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
	}

	public void OpenFile()
	{
		string path = EditorUtility.OpenFilePanel("OpenFile", "", "*");
		if (path.Length != 0)
			Debug.Log(path);
	}

	public void TurnOnOffGrid()
	{
		for(int i = 0; i < _gridTiles.Count; i++)
		{
			_gridTiles[i].TurnOnOff();
		}
	}
}
