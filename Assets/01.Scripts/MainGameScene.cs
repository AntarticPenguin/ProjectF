using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{
	public TileMap _tileMap;

	void Awake()
	{
		GameManager.Instance.SetMap(_tileMap);
	}

	// Start is called before the first frame update
	void Start()
    {
		Character player = CreateCharacter("Player", "Isolet_Test");
		//Character enemy = CreateCharacter("Enemy", "EnemySample");

		Camera.main.transform.SetParent(player.transform);
		Camera.main.transform.localPosition = new Vector3(0.0f, 0.0f, Camera.main.transform.localPosition.z);
		Camera.main.transform.localScale = Vector3.one;

		player.Init();
		//enemy.Init();
	}

    // Update is called once per frame
    void Update()
    {
		MessageSystem.Instance.ProcessMessage();
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
