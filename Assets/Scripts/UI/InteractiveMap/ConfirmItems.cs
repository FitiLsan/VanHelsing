using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public sealed class ConfirmItems : MonoBehaviour, IPointerClickHandler
    {
        public static event Action ConfirmItemsEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            ConfirmItemsEvent?.Invoke();
        }
    }
}