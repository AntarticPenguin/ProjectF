using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell
{
	Vector2 _position;

	public void AddObject(MapObject mapObject, eTileLayer layer, int sortingOrder)
	{
		mapObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
		int sortingLayerID = SortingLayer.NameToID(layer.ToString());
		mapObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;

		mapObject.SetPosition(_position);
	}

	public void SetPosition(Vector2 position)
	{
		_position = position;
	}
}
