using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/HealthItem", order = 1)]
[System.Serializable]
public class HealthItem : Item
{
	public float _value;

	public override void Use()
	{
		base.Use();
		Debug.Log("Use Item: " + _itemName + ", value: " + _value);
	}
}
