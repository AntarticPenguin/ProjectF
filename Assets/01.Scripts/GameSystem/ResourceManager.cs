using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonMonobehavior<ResourceManager>
{
	Dictionary<string, Sprite> _spriteMap = new Dictionary<string, Sprite>();

	public override void InitStart()
	{
		gameObject.name = "ResourceManager";
		DontDestroyOnLoad(gameObject);

		for (int i = 0; i < (int)eTileAsset.MAX; i++)
		{
			Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tile/" + ((eTileAsset)i).ToString());
			for (int j = 0; j < sprites.Length; j++)
				_spriteMap[sprites[j].name] = sprites[j];
		}
	}

	public Sprite FindSpriteByName(string name)
	{
		if (name.Equals("none"))
			return null;
		return _spriteMap[name];
	}
}
