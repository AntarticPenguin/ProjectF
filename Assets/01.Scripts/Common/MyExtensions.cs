using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyExtensions
{
    public static void InitTransformAsChild(this GameObject gameObject, Transform parent)
	{
		gameObject.transform.SetParent(parent);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
	}
}
