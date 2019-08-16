using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{
	public TileMap _tileMap;

	[Header("Player Spawn Position")]
	public int _playerSpawnX;
	public int _playerSpawnY;

	[Header("Portal Spawn Position")]
	public int _portalSpawnX;
	public int _portalSpawnY;

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
		Character player = MapObjectSpawner.Instance.CreateCharacter(_playerSpawnX, _playerSpawnY, "Player", "Isolet_Test");
		player.Init();
		GameManager.Instance.SetPlayer(player);
		GameManager.Instance.BecomeViewer(player);

		//Character enemy = CreateCharacter("Enemy", "Enemy_Test");
		//enemy.name = "Enemy";
		//enemy.Init();

		MapObjectSpawner.Instance.CreateMapObject(3, 3, eMapObjectType.ITEM, "Sword");
		MapObjectSpawner.Instance.CreateMapObject(2, 1, eMapObjectType.ITEM, "Shield");
	}
}
