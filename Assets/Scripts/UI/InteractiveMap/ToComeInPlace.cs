using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public sealed class ToComeInPlace : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<int> ToComeInPlaceEvent;
        public static event Action<Place> ToComeInPlaceObjEvent;
        private Place _currentPlace;
        private static GameObject _currentPositon;

        public void OnPointerClick(PointerEventData eventData)
        {
            ToComeInPlaceEvent?.Invoke(GetPlaceId());
            var parent = eventData.pointerEnter.transform.parent;
            _currentPlace = parent.GetComponent<Place>();
            ToComeInPlaceObjEvent?.Invoke(GetPlace());

            _currentPositon = GameObject.Find("currentPosition");

            _currentPositon.transform.SetPositionAndRotation(parent.position, parent.rotation);
        }

        public int GetPlaceId()
        {
            var parent = gameObject.transform.parent.transform.parent.parent;
            var placeId = parent.GetComponent<PlaceInteractiveField>().PlaceId;

            return placeId;
        }

        private Place GetPlace()
        {
            return _currentPlace;
        }
    }


}