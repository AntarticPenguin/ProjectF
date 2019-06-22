using UnityEngine;
using UnityEngine.UI;

public enum eStatusUIType
{
	TEXT,
	SLIDER,
}

public class StatusUI : MonoBehaviour
{
	eStatusUIType _uiType;

	public void SetUIType(eStatusUIType type)
	{
		_uiType = type;
	}

	public void UpdateInfo(string menu, sStatus status)
	{
		if(eStatusUIType.TEXT == _uiType)
		{
			if(menu.Equals("PlayerName"))
			{
				GetComponent<Text>().text = GameManager.Instance.GetPlayer().name;
			}
			else if (menu.Equals("Attack"))
			{
				GetComponent<Text>().text = "Attack: " + status.attack;
			}
			else if(menu.Equals("Armor"))
			{
				GetComponent<Text>().text = "Armor: " + status.armor;
			}
			else if(menu.Equals("Avoid"))
			{
				GetComponent<Text>().text = "Avoid: " + status.avoid;
			}
			else if(menu.Equals("Speed"))
			{
				GetComponent<Text>().text = "Speed: " + status.speed;
			}
		}
		else if(eStatusUIType.SLIDER == _uiType)
		{
			if(menu.Equals("HpSlider"))
			{
				GetComponentInChildren<Text>().text = status.hp + "/" + status.maxHp;
				GetComponent<Slider>().value = ((float)status.hp / status.maxHp);
			}
			else if(menu.Equals("MpSlider"))
			{
				GetComponentInChildren<Text>().text = status.mp + "/" + status.maxMp;
				GetComponent<Slider>().value = ((float)status.mp / status.maxMp);
			}
		}
	}
}
