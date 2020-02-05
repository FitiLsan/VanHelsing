using System;
using BaseScripts;
using Common;
using Controllers;
using Events;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View
{
    public class DayTimeView : MonoBehaviour
    {
        private bool _lock = true;

        [SerializeField] private Text _info;
        private DayTimeController dayTimeController;

        private void Start()
        {
            dayTimeController = StartScript.GetStartScript.DayTimeController;
        }

        private void Update()
        {
            var (curT, len) = dayTimeController.GetTime;
            _info.text = $"DayTime [{dayTimeController.CurrentDayTime}], Time [{curT:F2}/{len}]";
            if (!Input.GetKeyDown(KeyCode.T)) return;
            _lock = !_lock;
            if (!_lock)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void SetDayTime(string dayTime)
        {
            
            switch (dayTime)
            {
                case "Morning:true":
                    dayTimeController.SetDayTime(DayTimeTypes.Morning, true);
                    break;
                case "Morning:false":
                    dayTimeController.SetDayTime(DayTimeTypes.Morning, false);
                    break;
                case "Day:true":
                    dayTimeController.SetDayTime(DayTimeTypes.Day, true);
                    break;
                case "Day:false":
                    dayTimeController.SetDayTime(DayTimeTypes.Day, false);
                    break;
                case "Evening:true":
                    dayTimeController.SetDayTime(DayTimeTypes.Evening, true);
                    break;
                case "Evening:false":
                    dayTimeController.SetDayTime(DayTimeTypes.Evening, false);
                    break;
                case "Night:true":
                    dayTimeController.SetDayTime(DayTimeTypes.Night, true);
                    break;
                case "Night:false":
                    dayTimeController.SetDayTime(DayTimeTypes.Night, false);
                    break;
            }
        }
    }
}