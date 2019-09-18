using UnityEngine;

public static class MyExtensions
{
    public static void InitTransformAsChild(this GameObject gameObject, Transform parent)
	{
		gameObject.transform.SetParent(parent);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
	}

	public static bool EqualApproximately(this float lhs, float rhs, float tolerance)
	{
		return (Mathf.Abs(lhs - rhs) < tolerance);
	}
}
