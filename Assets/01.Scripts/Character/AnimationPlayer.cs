using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	System.Action _beginCallback = null;
	System.Action _midCallback = null;
	System.Action _endCallback = null;

	public void Play(string trigger,
		System.Action beginCallback,
		System.Action midCallback,
		System.Action endCallback
		)
	{
		GetComponent<Animator>().SetTrigger(trigger);
		_beginCallback = beginCallback;
		_midCallback = midCallback;
		_endCallback = endCallback;
	}

	//Animation Event
	public void OnBeginEvent()
	{
		//if (null != _beginCallback)
		//	_beginCallback();

		_beginCallback?.Invoke();
	}

	public void OnMidEvent()
	{
		_midCallback?.Invoke();
	}

	public void OnEndEvent()
	{
		Debug.Log("Animation END EVENT!!");
		_endCallback?.Invoke();
	}
}
