using UnityEngine;


public class DayNigthShiftData
{
    #region Constant

    private const int _constFullday = 2400;  //  The number of seconds for one day, constant

    private const int _secondInTimes = 600;  //  The number of seconds per stage of the day, constant

    #endregion

    #region Fields

    public static DayNightShiftStruct _dayNightShiftStruct;

    public int _secondsInFullDay;

    public float _rotationLimit = 0;

    public Light _sun;

    public int _secondsInMorning;

    public int _secondsInDay;

    public int _secondsInEvening;

    public int _secondsInNight;

    private float _x;

    private float _z;

    private float _flagGradient;

    private bool _flagMessege = false;

    private string _nightfallMessage;

    Gradient _gradient = new Gradient();

    GradientColorKey[] _gradientNigthColorKeys = new GradientColorKey[2];

    GradientAlphaKey[] _gradientNightAlphaKeys = new GradientAlphaKey[2];

    #endregion

    #region Construction

    public DayNigthShiftData(DayNightShiftStruct contex, Light sun, Color sunColorDay, Color sunColorNight)  //  class constructor for processing data received from outside
    {

        if (contex.SecondInDay != 0)
            _secondsInMorning = contex.SecondInMorning;
        else
        {
            _secondsInMorning = _secondInTimes;
            Debug.Log("The set value is 0, the default was 30 seconds.");
        }

        if (contex.SecondInDay != 0)
            _secondsInDay = contex.SecondInDay;
        else
        {
            _secondsInDay = _secondInTimes;
            Debug.Log("The set value is 0, the default was 30 seconds.");
        }

        if (contex.SecondInEvening != 0)
            _secondsInEvening = contex.SecondInEvening;
        else
        {
            _secondsInEvening = _secondInTimes;
            Debug.Log("The set value is 0, the default was 30 seconds.");
        }

        if (contex.SecondInNight != 0)
            _secondsInNight = contex.SecondInNight;
        else
        {
            _secondsInNight = _secondInTimes;
            Debug.Log("The set value is 0, the default was 30 seconds.");
        }

        _sun = sun;

        _sun.color = sunColorDay;

        CalculationSecondInFullDay();

        _x = _sun.transform.eulerAngles.x;
    
        _z = _sun.transform.eulerAngles.z;

        Debug.Log("1");

        _gradientNigthColorKeys[0].color = sunColorDay;
        _gradientNigthColorKeys[0].time = 0.0f;
        _gradientNigthColorKeys[1].color = sunColorNight;
        _gradientNigthColorKeys[1].time = 1.0f;
        _gradientNightAlphaKeys[0].alpha = sunColorDay.a;
        _gradientNightAlphaKeys[0].time = 0.0f;
        _gradientNightAlphaKeys[1].alpha = sunColorNight.a;
        _gradientNightAlphaKeys[1].time = 1.0f;

        _gradient.SetKeys(_gradientNigthColorKeys, _gradientNightAlphaKeys);

        _flagGradient = 0.5f;

        _nightfallMessage = contex.nightfallMessage;
    }

    #endregion

    #region Metods

    public float RotationLimit
    {
        get
        {
            return _rotationLimit;
        }
    }

    public int SecondInFullDay
    {
        get
        {
            return _secondsInFullDay;
        }
    }





    public void RotationSunDayNigth() // the method rotates the sun a fixed amount around the y - axis
    {
        if (_rotationLimit < 90f && _rotationLimit >= 0f)
        {
            _rotationLimit = _rotationLimit + (2 * 3.14f) / _secondsInMorning;
            if (_rotationLimit < 45)
                _flagGradient = 0.5f - (_rotationLimit / 90);
            else
                _flagGradient = 0;
        }
        if (_rotationLimit < 180f && _rotationLimit > 90f)
        {
            _flagMessege = true;
            _rotationLimit = _rotationLimit + (2 * 3.14f) / _secondsInDay;         
        }
        if (_rotationLimit < 270f && _rotationLimit > 180f)
        {           
            _rotationLimit = _rotationLimit + (2 * 3.14f) / _secondsInEvening;
            if (_rotationLimit > 225)
            {
                MassageLog();
                _flagGradient = (_rotationLimit - 225) / 90;
            }
        }
        if (_rotationLimit < 360f && _rotationLimit > 270f)
        {
            _rotationLimit = _rotationLimit + (2 * 3.14f) / _secondsInNight;
            if (_rotationLimit < 285)_flagGradient = 0.5f + (_rotationLimit - 270) / 30;
            if (_rotationLimit > 345) _flagGradient = 1.0f - (_rotationLimit - 345) / 30;
        }

        if (_rotationLimit >= 360f) _rotationLimit = 0;

        _sun.color = _gradient.Evaluate(_flagGradient);

        _sun.transform.localRotation = Quaternion.Euler(_x, _rotationLimit, _z);
        
   }

    public int CalculationSecondInFullDay ()  // calculating the maximum number of seconds
    {

        _secondsInFullDay = _secondsInMorning + _secondsInDay + _secondsInEvening + _secondsInNight;  //  summation of all time periods
        if (_secondsInFullDay == 0)  //  Foolproof
        {
            _secondsInFullDay = _constFullday;
            Debug.Log("All variable values ​​are 0, the default value is " + _constFullday + " seconds.");
        }
        return _secondsInFullDay;
    }

    public void MassageLog()  //  Method for displaying an evening message, obtained from SO
    {
        if (_flagMessege)
        {
            Debug.Log(_nightfallMessage);
            _flagMessege = false;
        }
    }

    #endregion
}
