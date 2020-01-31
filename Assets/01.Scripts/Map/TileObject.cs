using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject
{
	private void Awake()
	{
		_objectType = eMapObjectType.TILEOBJECT;
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

	}

	sTileProperties _tileProperties;
	public sTileProperties GetProperties() { return _tileProperties; }
	public void SetTileProperties(float speed)
	{
		_tileProperties.speed = speed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//TODO: 캐릭터와 일정 높이(offset)를 벗어나면 밟고 있지 않다는걸로 표기
		var type = collision.tag;
		if (type.Equals(eMapObjectType.PLAYER.ToString()))
		{
			var mapObject = collision.gameObject.GetComponent<Player>();
			if (mapObject == null)
			{
				mapObject = collision.gameObject.GetComponentInParent<Player>();
			}

			TileSystem.Instance.GetTileCell(_tileX, _tileY).AddObject(mapObject, mapObject.GetCurrentLayer(), false);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		var type = collision.tag;
		if (type.Equals(eMapObjectType.PLAYER.ToString()))
		{
			var mapObject = collision.gameObject.GetComponent<Player>();
			if (mapObject == null)
			{
				mapObject = collision.gameObject.GetComponentInParent<Player>();
			}

			TileSystem.Instance.GetTileCell(_tileX, _tileY).RemoveObject(mapObject);
		}
	}
}
