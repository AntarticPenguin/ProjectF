using System.Collections.Generic;
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

	public Tilemap groundTilemap;

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
			tilemapGo.name = "Tilemap - Trigger";
			var tilemap = tilemapGo.AddComponent<Tilemap>();

			var renderer = tilemapGo.AddComponent<TilemapRenderer>();
			renderer.sortOrder = TilemapRenderer.SortOrder.TopRight;
			renderer.enabled = false;
			renderer.sortingLayerID = SortingLayer.NameToID("TRIGGER");

			tilemapGo.InitTransformAsChild(gridGo.transform);
		}

		{
			GameObject tilemapGo = new GameObject();
			tilemapGo.name = "Tilemap - Ground";
			groundTilemap = tilemapGo.AddComponent<Tilemap>();

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
			collider.usedByComposite = false;

			Rigidbody2D rigid = tilemapGo.AddComponent<Rigidbody2D>();
			tilemapGo.AddComponent<CompositeCollider2D>();
			rigid.bodyType = RigidbodyType2D.Kinematic;
			rigid.simulated = true;

			Tile tile = Resources.Load("TilePalette/BlockTile") as Tile;
			int blockHeight = height + 1;
			int blockWidth = width + 1;
			for (int y = -1; y < blockHeight; y++)
			{
				for (int x = -1; x < blockWidth; x++)
				{
					if (-1 < y && y < blockHeight - 1)
					{
						if (x == -1 || x == blockWidth - 1)
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
		vcam.tag = "FollowingCamera";

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
			vcam.transform.position = new Vector3(0, 0, -1);
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

	public struct sSortObject
	{
		public GameObject gameObject;
		public int siblingIndex;
	}

	List<sSortObject> sortlist = new List<sSortObject>();
	public void SortOrder()
	{
		for(int index = 0; index < groundTilemap.transform.childCount; index++)
		{
			//Tile(heightIndex | widhtIndex)... Tile(2 | 9)
			GameObject go = groundTilemap.transform.GetChild(index).gameObject;
			string objectName = go.name;
			string str = objectName.Replace("Tile", string.Empty);
			str = str.Replace("(", string.Empty);
			str = str.Replace(")", string.Empty);
			string[] info = str.Split('|');
			int widthIndex = int.Parse(info[0]);
			int heightIndex = int.Parse(info[1]);

			int siblingIndex = heightIndex * width + widthIndex;
			sSortObject item = new sSortObject();
			item.gameObject = go;
			item.siblingIndex = siblingIndex;
			sortlist.Add(item);
			//Debug.Log(go.name + ":: width: " + widthIndex + ", height: " + heightIndex);
		}

		sortlist.Sort(delegate (sSortObject lhs, sSortObject rhs)
		{
			if (lhs.siblingIndex == rhs.siblingIndex) return 0;
			return lhs.siblingIndex.CompareTo(rhs.siblingIndex);
		});

		for(int i = 0; i < sortlist.Count; i++)
		{
			GameObject go = sortlist[i].gameObject;
			int siblingIndex = sortlist[i].siblingIndex;
			go.transform.SetSiblingIndex(siblingIndex);
		}
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

		if (GUILayout.Button("sort with Ground tiles' order"))
		{
			myScript.SortOrder();
		}
	}
}