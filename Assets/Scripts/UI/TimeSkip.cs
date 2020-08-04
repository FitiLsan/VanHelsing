using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public sealed class TimeSkip : MonoBehaviour
{
    #region Constant

    private const int maxHours = 24;
    private const int devider = 2;
    private const int halfDay = 12;
    private const string RainWeather = "Rain";
    private const string SunWeather = "Sunny";
    private const string FogWeather = "Fog";

    #endregion


    #region Fields

    private int tempHour;
    private int firstHour;
    private int secondHour;
    public Text weatherTxt;
    public Text hoursTxt;
    public Image image;
    public List<Sprite> weather;
    public Slider hoursSlider;

    #endregion


    #region Methods

    public void TimeChanged()
    {
        int time = (int)hoursSlider.value;
        hoursTxt.text = hoursSlider.value.ToString();
        image.sprite = WeatherPlace(firstHour, secondHour, time);
    }

    public Sprite WeatherPlace(int firstHour, int secondHour, int time)
    {
        if (time < firstHour)
        {
            weatherTxt.text = FogWeather;
            return weather[0];
        }
        else if (time < secondHour)
        {
            weatherTxt.text = SunWeather;
            return weather[1];
        }
        else
        {
            weatherTxt.text = RainWeather;
            return weather[2];
        }
    }

    private void Awake()
    {
        WeatherHours();
        TimeChanged();
    }

    public void WeatherHours()
    {
        firstHour = Random.Range(devider, maxHours - devider);
        Debug.Log($"Первый час: {firstHour}");

        if (firstHour <= halfDay)
        {
            secondHour = Random.Range(firstHour + devider, maxHours - devider);
            Debug.Log($"Второй час: {secondHour}");
        }
        else
        {
            tempHour = Random.Range(devider, firstHour - devider);
            Debug.Log(tempHour);
            secondHour = firstHour;
            Debug.Log($"Второй час: {secondHour}");
            firstHour = tempHour;
        }
    }

    #endregion
}
