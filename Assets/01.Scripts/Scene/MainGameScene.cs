using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{
	public TileMap _tileMap;

	[Header("Player Spawn Position")]
	public int _playerSpawnX;
	public int _playerSpawnY;

	[Header("Enemy Spawn Position")]
	public int _enemySpawnX;
	public int _enemySpawnY;

	private void Awake()
	{
		GameManager.Instance.Init();
		TileSystem.Instance.Init();
		Init();
	}

	// Start is called before the first frame update
	void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
		MessageSystem.Instance.ProcessMessage();
    }

	void Init()
	{
		//Character player = MapObjectSpawner.Instance.CreateCharacter(_playerSpawnX, _playerSpawnY, "Player", "Isolet_Test Collider");
		Character player = MapObjectSpawner.Instance.CreateCharacter(_playerSpawnX, _playerSpawnY, "Player", "Player_Preset01");
		player.Init();
		GameManager.Instance.SetPlayer(player);
		GameManager.Instance.BecomeViewer(player);

		Character enemy = MapObjectSpawner.Instance.CreateCharacter(_enemySpawnX, _enemySpawnY, "Enemy", "Enemy_Test");
		enemy.Init();

		//MapObjectSpawner.Instance.CreateMapObject(3, 3, eMapObjectType.ITEM, "HpPotion");
		//MapObjectSpawner.Instance.CreateMapObject(2, 1, eMapObjectType.ITEM, "HpPotion");
		//MapObjectSpawner.Instance.CreateMapObject(4, 4, eMapObjectType.ITEM, "HpPotionHalf");
		//MapObjectSpawner.Instance.CreateMapObject(5, 4, eMapObjectType.ITEM, "HpPotionHalf");
	}
}
