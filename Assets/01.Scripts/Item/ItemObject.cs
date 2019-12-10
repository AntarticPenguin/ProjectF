using UnityEngine;

public class ItemObject : MapObject
{
	//맵상에 표시 혹은 올려지는 오브젝트를 위한 클래스
	Item _item;

	private void Awake()
	{
		SetMapObjectType(eMapObjectType.ITEM);
	}

	public void Init(Item item)
	{
		_item = item;
		gameObject.name = _item.name;

		GetComponent<SpriteRenderer>().sprite = _item._icon;
		var width = GetComponent<SpriteRenderer>().sprite.texture.width;
		var height = GetComponent<SpriteRenderer>().sprite.texture.height;
		//auto resize to 48 x 48
		transform.localScale = new Vector3(48.0f / width, 48.0f / height, 1.0f);
	}

	public Item GetItem() { return _item; }
}
