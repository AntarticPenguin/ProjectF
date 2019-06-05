using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	#region SINGLETON
	static ResourceManager _instance;
	public static ResourceManager Instance
	{
		get
		{
			if(null == _instance)
			{
				_instance = FindObjectOfType<ResourceManager>();
				if(null == _instance)
				{
					GameObject go = new GameObject();
					go.name = "ResourceManager";
					_instance = go.AddComponent<ResourceManager>();
					_instance.Init();
					DontDestroyOnLoad(go);
				}
			}
			return _instance;
		}
	}
	#endregion
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	Dictionary<string, Sprite> _spriteMap = new Dictionary<string, Sprite>();
	void Init()
	{
		for(int i = 0; i < (int)eTileAsset.MAX; i++)
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
