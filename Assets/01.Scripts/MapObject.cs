using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	Vector2 _position;

	public void SetPosition(Vector2 position)
	{
		_position = position;
		transform.localPosition = _position;
	}
}
