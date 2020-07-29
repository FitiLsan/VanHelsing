using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeastHunter
{
    public class PlaceButtonClick : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<Place> ClickEvent;
        public static event Action<int> CanvasClickEvent;
        private Place _currentPlace;

        private static GameObject _currentPositon;

        public void OnPointerClick(PointerEventData eventData)
        {
            var parent = eventData.pointerEnter.transform.parent;
            _currentPlace = parent.GetComponent<Place>();

            //_currentPositon = GameObject.Find("currentPosition");

            //_currentPositon.transform.SetPositionAndRotation(parent.position, parent.rotation);



            ClickEvent?.Invoke(GetPlace());
            CanvasClickEvent?.Invoke(0);
        }

        private Place GetPlace()
        {
            return _currentPlace;
        }
    }
}