using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		_objectType = eMapObjectType.NONE;
		_currentLayer = eTileLayer.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	Vector2 _position;
	protected int _tileX;
	protected int _tileY;
	protected eMapObjectType _objectType;

	public void SetPosition(Vector2 position)
	{
		_position = position;
		transform.localPosition = _position;
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
		sTilePosition tilePos;
		tilePos._tileX = _tileX;
		tilePos._tileY = _tileY;
		return tilePos;
	}

	public eMapObjectType GetMapObjectType() { return _objectType; }

	protected eTileLayer _currentLayer;
	public eTileLayer GetCurrentLayer() { return _currentLayer; }
	public void SetCurrentLayer(eTileLayer layer)
	{
		_currentLayer = layer;
	}

	#region Message

	public virtual void ReceiveMessage(MessageParam msgParam)
	{
		//TODO: 메세지 처리
	}

	#endregion
}
