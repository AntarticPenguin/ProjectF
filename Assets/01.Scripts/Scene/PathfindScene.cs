using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindScene : MonoBehaviour
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
        
    }

	public int _tileX;
	public int _tileY;
	void Init()
	{
		Character player = MapObjectSpawner.Instance.CreateCharacter(12, 9, "Pathfinder", "Isolet_Test");
		player.Init();
		GameManager.Instance.SetPlayer(player);
		GameManager.Instance.BecomeViewer(player);
		((Pathfinder)player).SetTargetTileCell(_tileX, _tileY);
	}
}
