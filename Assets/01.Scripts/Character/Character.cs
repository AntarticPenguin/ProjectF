using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.position = new Vector2(0.0f, 0.0f);
		}
    }

	public void Init()
	{
		TileMap map = GameManager.Instance.GetMap();

		//test create character
		string filePath = "Prefabs/CharacterSample";
		GameObject charPrefabs = Resources.Load<GameObject>(filePath);
		charPrefabs.transform.SetParent(map.transform);
		charPrefabs.transform.localPosition = Vector3.zero;
		charPrefabs.transform.localScale = new Vector2(2.0f, 2.0f);

		MapObject character = charPrefabs.AddComponent<MapObject>();
		map.GetTileCell(0, 0).AddObject(character, eTileLayer.GROUND, 255);
	}
}
