using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
	//아이템 정보를 가지고 있는 클래스
	public new string name = "New Item";
	public Sprite icon = null;
}
