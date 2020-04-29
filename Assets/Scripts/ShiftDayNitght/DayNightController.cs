using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class DayNightController : MonoBehaviour, IAwake, IUpdate
    {


        #region Variable
        public Light Sun;

        public DayNightShiftStruct _dayNightShiftStruct;

        private static DayNigthShiftData _dayNightShiftData;


        public Color _colorDay;

        public Color _colorNight;
        #endregion


        #region Start
        public void Start()
        {
            _dayNightShiftData = new DayNigthShiftData(_dayNightShiftStruct, Sun, _colorDay, _colorNight);
        }

        #endregion


        #region ClassLifeCycle

        public DayNightController(Services services)
        {
            _dayNightShiftData = new DayNigthShiftData(_dayNightShiftStruct, Sun, _colorDay, _colorNight);
        }

        public void Updating()
        {
            _dayNightShiftData.RotationSunDayNigth();
        }


        public void OnAwake()
        {

        }

        public void Update()
        {
            _dayNightShiftData.RotationSunDayNigth();
        }

        #endregion


    }
}