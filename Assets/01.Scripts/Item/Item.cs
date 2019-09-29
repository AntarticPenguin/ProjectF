using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ItemDefault", order = 0)]
[System.Serializable]
public class Item : ScriptableObject
{
	//아이템 정보를 가지고 있는 클래스
	public string _itemName = "New Item";
	public Sprite _icon = null;

	public virtual void Use()
	{
		
	}
}
