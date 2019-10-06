using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/HealthItem", order = 1)]
[System.Serializable]
public class HealthItem : Item
{
	public int _value;

	public override void Use(Character character)
	{
		base.Use(character);
		character.IncreaseHp(_value);
	}
}
