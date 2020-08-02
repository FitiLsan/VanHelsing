using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace BeastHunter
{
    public sealed class TimeSkip : MonoBehaviour
    {
        #region Constants

        private const int MAX_HOURS = 24;
        private const int HOURS_DEVIDER = 2;
        private const int HALF_PERIOD_HOURS = 12;

        private const string WEATHER_RAIN = "Rain";
        private const string WEATHER_SUN = "Sunny";
        private const string WEATHER_FOG = "Fog";

        #endregion


        #region Fields

        public Text WeatherText;
        public Text HoursText;
        public Image Image;
        public Sprite[] Weather;
        public Slider HoursSlider;

        private WeatherType _chosenWeather;

        private int _tempHour;
        private int _firstHour;
        private int _secondHour;
        private int _chosenTime;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            WeatherHours();
            TimeChanged();
        }

        #endregion


        #region Methods

        public void TimeChanged()
        {
            _chosenTime = (int)HoursSlider.value;
            HoursText.text = HoursSlider.value.ToString();
            Image.sprite = WeatherPlace(_firstHour, _secondHour, _chosenTime);
        }

        public Sprite WeatherPlace(int firstHour, int secondHour, int time)
        {
            if (time < firstHour)
            {
                WeatherText.text = WEATHER_FOG;
                _chosenWeather = WeatherType.Fog;
                return Weather[0];
            }
            else if (time < secondHour)
            {
                WeatherText.text = WEATHER_SUN;
                _chosenWeather = WeatherType.Sun;
                return Weather[1];
            }
            else
            {
                WeatherText.text = WEATHER_RAIN;
                _chosenWeather = WeatherType.Rain;
                return Weather[2];
            }
        }

        public void WeatherHours()
        {
            _firstHour = Random.Range(HOURS_DEVIDER, MAX_HOURS - HOURS_DEVIDER);

            if (_firstHour <= HALF_PERIOD_HOURS)
            {
                _secondHour = Random.Range(_firstHour + HOURS_DEVIDER, MAX_HOURS - HOURS_DEVIDER);
            }
            else
            {
                _tempHour = Random.Range(HOURS_DEVIDER, _firstHour - HOURS_DEVIDER);
                _secondHour = _firstHour;
                _firstHour = _tempHour;
            }
        }

        public void LoadChosenConditions()
        {
            switch (_chosenWeather)
            {
                case WeatherType.None:
                    break;
                case WeatherType.Sun:
                    LoadChosenTime(_chosenTime);
                    // LOAD WEATHER SUN
                    break;
                case WeatherType.Rain:
                    LoadChosenTime(_chosenTime);
                    // LOAD WEATHER RAIN
                    break;
                case WeatherType.Fog:
                    LoadChosenTime(_chosenTime);
                    // LOAD WEATHER FOG
                    break;
                default:
                    break;
            }
            
        }

        public void LoadChosenTime(float time)
        {
            // SET TIME
            Services.SharedInstance.TrapService.SaveTraps();
            SceneManager.LoadScene(0);
        }

        public void CloseWindow()
        {
            Services.SharedInstance.TimeSkipService.CloseTimeSkipMenu();
        }

        #endregion
    }
}

