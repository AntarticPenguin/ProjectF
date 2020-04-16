using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : SingletonMonobehavior<DayNightCycle>
{
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

	public float _interval;

	float _globalMinLight;
	float _globalMaxLight;

	public override void Init()
	{
		gameObject.name = "TimeSystem";
		DontDestroyOnLoad(gameObject);

		_hourText.text = _hour.ToString("D2") + " : ";
		_minText.text = _minute.ToString("D2");
		if (_hour >= 12)
			_meridiem.text = "PM";
		else
			_meridiem.text = "AM";

		_globalMinLight = 0.5f;
		_globalMaxLight = 1.1f;

		Vector2 newPosition = GameManager.Instance.GetPlayer().transform.position;
		newPosition.y += 0.5f;
		_pointLight.transform.position = newPosition;

		onHourChangedCallback += UpdateLight;

		StartCoroutine(UpdateTime(_interval));
	}

    // Update is called once per frame
    void Update()
    {
		Vector2 newPosition = GameManager.Instance.GetPlayer().transform.position;
		newPosition.y += 0.5f;
		_pointLight.transform.position = newPosition;
	}

	IEnumerator UpdateTime(float interval)
	{
		while(true)
		{
			yield return new WaitForSeconds(interval);
			_second += 30;

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
