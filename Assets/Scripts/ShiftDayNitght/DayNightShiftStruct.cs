using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewModel", menuName = "DayNightShiftData")]

public class DayNightShiftStruct : ScriptableObject

{


    #region Fields

    public Light Sun;

    [Range (0,5000)]
    [SerializeField]
    public int secondsInMorning; // seconds in the morning

    [Range(0, 5000)]
    [SerializeField]
    public int secondsInDay;  //  seconds per day

    [Range(0, 5000)]
    [SerializeField]
    public int secondsInEvening;  //  seconds in the evening

    [Range(0, 5000)]
    [SerializeField]
    public int secondsInNight; //  seconds per night

    public string nightfallMessage;  //  The message that is displayed when night approaches

    #endregion


    #region Metod

    public int SecondInMorning
    {
        get
        {
            return secondsInMorning;
        }
    }

    public int SecondInDay
    {
        get
        {
            return secondsInDay;
        }
    }

    public int SecondInEvening
    {
        get
        {
            return secondsInEvening;
        }
    }

    public int SecondInNight
    {
        get
        {
            return secondsInNight;
        }
    }

    #endregion

}