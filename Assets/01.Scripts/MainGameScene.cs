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
		Character character = CreateCharacter();

		Camera.main.transform.SetParent(character.transform);
		Camera.main.transform.localPosition = new Vector3(0.0f, 0.0f, Camera.main.transform.localPosition.z);
		Camera.main.transform.localScale = Vector3.one;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	Character CreateCharacter()
	{
		TileMap map = GameManager.Instance.GetMap();

		//string filePath = "Prefabs/CharacterSample";
		string filePath = "Prefabs/CharacterSample2";
		//string filePath = "Prefabs/CharacterSample3";

		GameObject charPrefabs = Resources.Load<GameObject>(filePath);
		GameObject characterObject = Instantiate(charPrefabs);
		characterObject.InitTransformAsChild(map.transform);
		characterObject.transform.localScale = new Vector2(2.0f, 2.0f);

		Character character = characterObject.AddComponent<Character>();
		map.GetTileCell(0, 0).AddObject(character, eTileLayer.MIDDLE_GROUND, 0);

		return character;
	}
}
