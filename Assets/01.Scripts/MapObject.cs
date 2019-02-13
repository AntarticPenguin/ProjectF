using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	Vector2 _position;
	protected int _tileX;
	protected int _tileY;

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
}
