using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class MapTimer : MonoBehaviour
    {
        public int CurrentHour;
        public int CurrentMinutes;
        public Text Hour;
        public Text Minute;
        private void Awake()
        {
            ToComeInPlace.ToComeInPlaceObjEvent += OnPlaceTransfer;

            Hour = gameObject.transform.Find("Hours").GetComponent<Text>();
            Minute = gameObject.transform.Find("Minuts").GetComponent<Text>();

            CurrentHour = Convert.ToInt16( gameObject.transform.Find("Hours").GetComponent<Text>().text);
            CurrentMinutes = Convert.ToInt16(gameObject.transform.Find("Minuts").GetComponent<Text>().text);
        }

        private void OnPlaceTransfer()
        {
            CurrentMinutes +=30 ;
            if(CurrentMinutes>=60)
            {
                CurrentHour++;
                CurrentMinutes = 0;
            }

            Minute.text = string.Format($"{CurrentMinutes:d2}");
            Hour.text = string.Format($"{CurrentHour:d2}");
            
        }
    }
}