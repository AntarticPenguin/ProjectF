using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Cinemachine;
using UnityEngine.Experimental.Rendering.LWRP;

public class PrepareScene : MonoBehaviour
{
	public Transform rootScene;
	public int width;
	public int height;

	public void Prepare()
	{
		InitTilemap();
		InitCamera();
		InitLight();
		InitUICanvas();
		InitSystem();
	}

	void InitTilemap()
	{
		GameObject gridGo = new GameObject();
		gridGo.name = "Grid";
		Grid grid = gridGo.AddComponent<Grid>();
		grid.cellSize = new Vector3(1.02f, 0.51f, 0.0f);
		grid.cellLayout = GridLayout.CellLayout.Isometric;
		if (rootScene)
		{
			gridGo.InitTransformAsChild(rootScene);
		}

		{
			GameObject tilemapGo = new GameObject();
			tilemapGo.name = "Tilemap - Ground";
			tilemapGo.AddComponent<Tilemap>();

			var renderer = tilemapGo.AddComponent<TilemapRenderer>();
			renderer.sortOrder = TilemapRenderer.SortOrder.TopRight;
			renderer.mode = TilemapRenderer.Mode.Individual;
			renderer.sortingLayerID = SortingLayer.NameToID("GROUND");

			tilemapGo.InitTransformAsChild(gridGo.transform);
		}

		{
			GameObject tilemapGo = new GameObject();
			tilemapGo.name = "Tilemap - Blocking";
			var tilemap = tilemapGo.AddComponent<Tilemap>();

			var renderer = tilemapGo.AddComponent<TilemapRenderer>();
			renderer.sortOrder = TilemapRenderer.SortOrder.TopRight;
			renderer.sortingLayerID = SortingLayer.NameToID("BLOCK");
			var collider = tilemapGo.AddComponent<TilemapCollider2D>();
			collider.usedByComposite = true;

			Rigidbody2D rigid = tilemapGo.AddComponent<Rigidbody2D>();
			tilemapGo.AddComponent<CompositeCollider2D>();
			rigid.bodyType = RigidbodyType2D.Kinematic;
			rigid.simulated = true;

			Tile tile = Resources.Load("TilePalette/BlockTile") as Tile;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (0 < y && y < height - 1)
					{
						if (x == 0 || x == width - 1)
						{
							Vector3Int position = new Vector3Int(x, y, 0);
							tilemap.SetTile(position, tile);
						}
					}
					else
					{
						Vector3Int position = new Vector3Int(x, y, 0);
						tilemap.SetTile(position, tile);
					}
				}
			}
			tilemapGo.InitTransformAsChild(gridGo.transform);
		}
	}

	void InitCamera()
	{
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 4;
		Camera.main.depth = -1;
		Camera.main.gameObject.AddComponent<AxisDistanceSortCameraHelper>();

		GameObject camGo = new GameObject();
		camGo.name = "CM vcam1";
		var vcam = camGo.AddComponent<CinemachineVirtualCamera>();
		vcam.m_Priority = 4;
		vcam.m_Lens.OrthographicSize = 4;

		var transposer = vcam.AddCinemachineComponent<CinemachineFramingTransposer>();
		transposer.m_XDamping = 1.0f;
		transposer.m_YDamping = 1.5f;
		transposer.m_ZDamping = 1.0f;
		transposer.m_ScreenX = 0.5f;
		transposer.m_ScreenY = 0.5f;
		transposer.m_CameraDistance = 10.0f;
		transposer.m_DeadZoneWidth = 0.08f;
		transposer.m_DeadZoneHeight = 0.08f;
		transposer.m_DeadZoneDepth = 0.0f;
		transposer.m_SoftZoneWidth = 0.08f;
		transposer.m_SoftZoneHeight = 0.08f;
		transposer.m_CenterOnActivate = false;

		var cineBrain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
		cineBrain.m_ShowCameraFrustum = true;

		if (rootScene)
		{
			camGo.InitTransformAsChild(rootScene);
			Camera.main.gameObject.InitTransformAsChild(rootScene);
		}
	}

	void InitLight()
	{
		GameObject lightPrefab = Resources.Load("Prefabs/LightGroup") as GameObject;
		GameObject instance = PrefabUtility.InstantiatePrefab(lightPrefab) as GameObject;
		if (rootScene)
		{
			instance.InitTransformAsChild(rootScene);
		}
	}

	void InitUICanvas()
	{
		GameObject canvasPrefab = Resources.Load("Prefabs/UI/UICanvas") as GameObject;
		var go = PrefabUtility.InstantiatePrefab(canvasPrefab) as GameObject;
		var timeSystem = go.GetComponentInChildren<DayNightCycle>();
		if(rootScene)
		{
			var litGroup = rootScene.Find("LightGroup");
			var globalLit = litGroup.Find("Global Light 2D").GetComponent<Light2D>();
			var pointLit = litGroup.Find("Point Light 2D").GetComponent<Light2D>();
			timeSystem._globalLight = globalLit;
			timeSystem._pointLight = pointLit;
		}
	}

	void InitSystem()
	{
		
	}
}

[CustomEditor(typeof(PrepareScene))]
public class PrepareSceneEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PrepareScene myScript = (PrepareScene)target;
		if (GUILayout.Button("Prepare Default Tilemap"))
		{
			myScript.Prepare();
		}
	}
}