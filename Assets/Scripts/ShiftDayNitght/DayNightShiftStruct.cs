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

    public int secondsInMorning;
    [Range(0, 5000)]
    [SerializeField]

    public int secondsInDay;
    [Range(0, 5000)]
    [SerializeField]

    public int secondsInEvening;
    [Range(0, 5000)]
    [SerializeField]

    public int secondsInNight;

    public string nightfallMessage;

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