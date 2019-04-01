﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{
	public TileMap _tileMap;

	private void Awake()
	{
		GameManager.Instance.SetMap(_tileMap);
		_tileMap.Init();
	}

	// Start is called before the first frame update
	void Start()
    {
		Init();
	}

    // Update is called once per frame
    void Update()
    {
		MessageSystem.Instance.ProcessMessage();
    }

	void Init()
	{
		Character player = CreateCharacter("Player", "Isolet_Test");
		player.name = "Player";

		player.Init();
		player.BecomeViewer();

		//Character enemy = CreateCharacter("Enemy", "EnemySample");
		//enemy.name = "Enemy";
		//enemy.Init();
	}

	Character CreateCharacter(string scriptName, string resourceName)
	{
		TileMap map = GameManager.Instance.GetMap();

		string filePath = "Prefabs/Character/" + resourceName;

		GameObject charPrefabs = Resources.Load<GameObject>(filePath);
		GameObject characterObject = Instantiate(charPrefabs);
		characterObject.InitTransformAsChild(map.transform);
		characterObject.transform.localScale = new Vector2(2.0f, 2.0f);


		Character character = null;
		switch(scriptName)
		{
			case "Player":
				character = characterObject.AddComponent<Player>();
				break;
			case "Enemy":
				character = characterObject.AddComponent<Enemy>();
				break;
			default:
				break;
		}
		map.GetTileCell(0, 0).SetObject(character, eTileLayer.ON_GROUND, 0);
		

		return character;
	}
}
