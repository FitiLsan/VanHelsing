using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public sealed class TimeSkip : MonoBehaviour
{
    #region Constance

    private const int maxHours = 24;
    private const int devider = 2;
    private const int halfDay = 12;
    private const string Rain = "Rain";
    private const string Sun = "Sunny";
    private const string Fog = "Fog";

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
            return weather[0];
        }
        else if (time < secondHour)
        {
            return weather[1];
        }
        else
        {
            return weather[2];
        }
    }

    private void Awake()
    {
        WeatherHours();
        TimeChanged();
    }

    
    //public string GetWeather()
    //{
    //    return;
    //}

    public void WeatherHours()
    {
        firstHour = Random.Range(devider, maxHours - devider);
        Debug.Log(firstHour);

        if (firstHour <= halfDay)
        {
            secondHour = Random.Range(firstHour + devider, maxHours - devider);
            Debug.Log(secondHour);
        }
        else
        {
            tempHour = Random.Range(devider, secondHour - devider);
            Debug.Log(tempHour);
            secondHour = firstHour;
            firstHour = tempHour;
        }
    }

    #endregion
}
