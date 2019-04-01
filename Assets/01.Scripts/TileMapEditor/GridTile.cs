using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
	public PolygonCollider2D _collider;
	public Material _material;
	List<LineRenderer> _lineRenderers = new List<LineRenderer>();

	private void Awake()
	{
		_collider = gameObject.AddComponent<PolygonCollider2D>();
		_material = new Material(Shader.Find("Mobile/Particles/Additive"));
	}

	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void MakeBoundaryLines(float width, float height)
	{
		for (int i = 0; i < 4; i++)
		{
			GameObject go = new GameObject("line" + i.ToString());
			LineRenderer li = go.AddComponent<LineRenderer>();
			li.transform.SetParent(transform);
			li.material = _material;
			li.sortingLayerID = SortingLayer.NameToID("ON_GROUND");
			li.sortingOrder = 0;
			li.startWidth = 0.02f;
			li.endWidth = 0.02f;

			_lineRenderers.Add(li);
		}
		DrawLine(_lineRenderers[0], new Vector3(transform.position.x, transform.position.y, 0),
			new Vector3(transform.position.x + width / 2, transform.position.y + height / 2));

		DrawLine(_lineRenderers[1], new Vector3(transform.position.x + width / 2, transform.position.y + height / 2),
			new Vector3(transform.position.x, transform.position.y + height, 0));

		DrawLine(_lineRenderers[2], new Vector3(transform.position.x, transform.position.y + height),
			new Vector3(transform.position.x - width / 2, transform.position.y + height / 2, 0));

		DrawLine(_lineRenderers[3], new Vector3(transform.position.x - width / 2, transform.position.y + height / 2, 0),
			new Vector3(transform.position.x, transform.position.y, 0));
	}

	void DrawLine(LineRenderer li, Vector3 start, Vector3 end)
	{
		li.SetPosition(0, start);
		li.SetPosition(1, end);
	}

	public void MakeCollider(float sizeX, float sizeY)
	{
		//반시계로 4개 오프셋 기준
		Vector2[] points = new Vector2[4];
		points[0] = new Vector2(0, sizeY);
		points[1] = new Vector2(-(sizeX / 2.0f), (sizeY / 2.0f));
		points[2] = new Vector2(0, 0);
		points[3] = new Vector2((sizeX / 2.0f), (sizeY / 2.0f));
		_collider.SetPath(0, points);
		//_collider.points = points;
	}

	private void OnMouseOver()
	{
		for(int i = 0; i < _lineRenderers.Count; i++)
		{
			_lineRenderers[i].startWidth = 0.04f;
			_lineRenderers[i].endWidth = 0.04f;
		}
	}

	private void OnMouseExit()
	{
		for (int i = 0; i < _lineRenderers.Count; i++)
		{
			_lineRenderers[i].startWidth = 0.02f;
			_lineRenderers[i].endWidth = 0.02f;
		}
	}

	bool _turnOn = false;
	public void TurnOnOff()
	{
		for (int i = 0; i < _lineRenderers.Count; i++)
		{
			_lineRenderers[i].enabled = _turnOn;
		}
		_turnOn = !_turnOn;
	}
}
