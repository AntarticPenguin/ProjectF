using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.Tilemaps;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Tile Prefab brush", menuName = "Brushes/Tile Prefab brush")]
[CustomGridBrush(false, true, false, "Tile Prefab Brush")]
public class TilePrefabBrush : GridBrushBase
{
	public GameObject m_Prefabs;
	public Sprite selectedSprite;
	public Vector3 m_Anchor = new Vector3(0.5f, 0.5f, 0.0f);

	private GameObject prev_brushTarget;
	private Vector3Int prev_position = Vector3Int.one * Int32.MaxValue;

	public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
	{
		base.Pick(gridLayout, brushTarget, position, pivot);

		Tilemap palette = brushTarget.GetComponent<Tilemap>();
		if (palette == null)
			return;
		Vector3Int palettePosition = position.position;
		Tile tile = palette.GetTile(palettePosition) as Tile;
		if (tile != null)
		{
			selectedSprite = tile.sprite;
		}
	}

	public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
	{
		if (position == prev_position)
		{
			return;
		}
		prev_position = position;
		if (brushTarget)
		{
			prev_brushTarget = brushTarget;
		}
		brushTarget = prev_brushTarget;

		// Do not allow editing palettes
		if (brushTarget.layer == 31)
			return;

		GameObject prefab = m_Prefabs;
		GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
		if (instance != null)
		{
			Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
			Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");
			instance.GetComponent<SpriteRenderer>().sprite = selectedSprite;
			instance.transform.SetParent(brushTarget.transform);
			instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position + m_Anchor));
		}
	}

	public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
	{
		if (brushTarget)
		{
			prev_brushTarget = brushTarget;
		}
		brushTarget = prev_brushTarget;
		// Do not allow editing palettes
		if (brushTarget.layer == 31)
			return;

		Transform erased = GetObjectInCell(grid, brushTarget.transform, position);
		if (erased != null)
			Undo.DestroyObjectImmediate(erased.gameObject);
	}

	private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
	{
		int childCount = parent.childCount;
		Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
		Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
		Bounds bounds = new Bounds((max + min) * .5f, max - min);

		for (int i = 0; i < childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (bounds.Contains(child.position))
				return child;
		}
		return null;
	}
}

/// <summary>
/// The Brush Editor for a Prefab Brush.
/// </summary>
[CustomEditor(typeof(PrefabBrush))]
public class TilePrefabBrushEditor : GridBrushEditor
{
	private PrefabBrush prefabBrush { get { return target as PrefabBrush; } }

	private SerializedProperty m_Prefabs;
	private SerializedProperty selectedSprite;
	private SerializedProperty m_Anchor;
	private SerializedObject m_SerializedObject;

	protected override void OnEnable()
	{
		base.OnEnable();
		m_SerializedObject = new SerializedObject(target);
		m_Prefabs = m_SerializedObject.FindProperty("m_Prefabs");
		selectedSprite = m_SerializedObject.FindProperty("selectedSprite");
		m_Anchor = m_SerializedObject.FindProperty("m_Anchor");
	}

	/// <summary>
	/// Callback for painting the inspector GUI for the PrefabBrush in the Tile Palette.
	/// The PrefabBrush Editor overrides this to have a custom inspector for this Brush.
	/// </summary>
	public override void OnPaintInspectorGUI()
	{
		m_SerializedObject.UpdateIfRequiredOrScript();
		EditorGUILayout.PropertyField(m_Prefabs);
		EditorGUILayout.PropertyField(selectedSprite);
		EditorGUILayout.PropertyField(m_Anchor);
		m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
	}
}
