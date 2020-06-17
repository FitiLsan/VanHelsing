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
        private static List<Image> _lineList = new List<Image>();
        private Place _currentPlace;
        private Image _currentline;
        private static GameObject _curretPositon;

        public void OnPointerClick(PointerEventData eventData)
        {
            var parent = eventData.pointerEnter.transform.parent;
            _curretPositon = GameObject.Find("currentPosition");
            _currentline = parent.Find("Line").GetComponent<Image>();
            _lineList.Add(_currentline);
            _curretPositon.transform.SetPositionAndRotation(parent.position, parent.rotation);
            foreach(var line in _lineList)
            {
                line.enabled = false;
            }
            _currentline.enabled = true;
            _currentPlace = parent.GetComponent<Place>();
            
            ClickEvent?.Invoke(GetPlace());
        }
        
        private Place GetPlace()
        {
            return _currentPlace;
        }
    }
}