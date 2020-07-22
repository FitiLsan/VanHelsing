using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public sealed class PlaceExit : MonoBehaviour, IPointerClickHandler
    {
        public static event Action PlaceExitEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            PlaceExitEvent?.Invoke();
        }
    }
}