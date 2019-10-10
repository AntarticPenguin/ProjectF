using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.LWRP;

public class DayNightCycle : MonoBehaviour
{
	#region SINGLETON
	static DayNightCycle _instance;
	public static DayNightCycle Instance
	{
		get
		{
			if (null == _instance)
			{
				_instance = FindObjectOfType<DayNightCycle>();
				if (null == _instance)
				{
					GameObject go = new GameObject();
					go.name = "TimeSystem";
					_instance = go.AddComponent<DayNightCycle>();
					DontDestroyOnLoad(go);
				}
			}
			return _instance;
		}
	} 
	#endregion

	// Start is called before the first frame update
	void Start()
    {
		Init();
    }

    // Update is called once per frame
    void Update()
    {
		UpdateTime();
		UpdateLight();

		Vector2 newPosition = GameManager.Instance.GetPlayer().transform.position;
		newPosition.y += 0.5f;
		_pointLight.transform.position = newPosition;
	}

	public Text _hourText;
	public Text _minText;
	public Text _meridiem;
	public Light2D _globalLight;
	public Light2D _pointLight;
	public delegate void OnHourChanged();
	public OnHourChanged onHourChangedCallback;

	public int _hour;
	public int _minute;
	int _second;
	float _updateDuration;

	float _globalMinLight;
	float _globalMaxLight;

	void Init()
	{
		_hourText.text = _hour.ToString("D2") + " : ";
		_minText.text = _minute.ToString("D2");
		if (_hour >= 12)
			_meridiem.text = "PM";
		else
			_meridiem.text = "AM";

		_updateDuration = 0.0f;

		_globalMinLight = 0.5f;
		_globalMaxLight = 1.5f;

		Vector2 newPosition = GameManager.Instance.GetPlayer().transform.position;
		newPosition.y += 0.5f;
		_pointLight.transform.position = newPosition;

		onHourChangedCallback += UpdateLight;
	}

	void UpdateTime()
	{
		_updateDuration += Time.deltaTime;
		if (_updateDuration >= 1.0f)
		{
			_second += 30;
			_updateDuration = 0.0f;
		}

		if (_second >= 60)
		{
			_minute++;
			_minText.text = _minute.ToString("D2");
			_second = 0;
		}

		if (_minute >= 60)
		{
			_hour++;
			_minute = 0;
			if (onHourChangedCallback != null)
				onHourChangedCallback.Invoke();

			if (_hour == 24)
				_hour = 0;

			if (_hour >= 12)
			{
				_meridiem.text = "PM";
				_hourText.text = (_hour - 12).ToString("D2") + " : ";
			}
			else
			{
				_meridiem.text = "AM";
				_hourText.text = _hour.ToString("D2") + " : ";
			}
		}
	}

	void UpdateLight()
	{
		if (_hour <= 12)
		{
			_globalLight.intensity = _globalMinLight + (_hour / 12.0f);
		}
		else
		{
			_globalLight.intensity = _globalMaxLight - ((_hour - 12) / 12.0f);
		}

		if (_globalLight.intensity >= _globalMaxLight)
			_globalLight.intensity = _globalMaxLight;

		if (_globalLight.intensity <= _globalMinLight)
			_globalLight.intensity = _globalMinLight;

		if (10 <= _hour && _hour <= 16)
			_pointLight.enabled = false;
		else
			_pointLight.enabled = true;
	}
}
