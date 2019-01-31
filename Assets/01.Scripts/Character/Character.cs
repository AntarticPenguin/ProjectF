using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject
{
	public float _speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		Vector2 direction= new Vector2();

		if (Input.GetKey(KeyCode.UpArrow))
		{
			direction += new Vector2(0.0f, 1.0f);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			direction += new Vector2(0.0f, -1.0f);
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			direction += new Vector2(-1.0f, 0.0f);
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			direction += new Vector2(1.0f, 0.0f);
		}

		TileMap map = GameManager.Instance.GetMap();
		if (Input.GetKeyDown(KeyCode.T))
		{
			Debug.Log("TileX: " + _tileX + ", TileY: " + _tileY + ", " + map.GetTileCell(_tileX, _tileY).GetPosition() +
				", " + "Char: " + transform.position);
		}

		Vector2 position = _speed * direction.normalized * Time.deltaTime;
		Vector2 nextPosition = (Vector2)transform.position + position;
		if (map.GetTileCell(_tileX, _tileY).CheckOnTile(nextPosition))
		{
			//transform.Translate(position);
			//transform.position += position;
			transform.position += new Vector3(position.x, position.y, 0.0f);
		}
		else
		{
			Debug.Log("Can't Move!!");
		}
		//transform.position += new Vector3(position.x, position.y, 0.0f);
	}

	public void Init()
	{

	}
}
