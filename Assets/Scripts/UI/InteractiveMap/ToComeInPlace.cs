using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public sealed class ToComeInPlace : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<int> ToComeInPlaceEvent;
        public static event Action ToComeInPlaceObjEvent;
        private static GameObject _currentPositon;

        public void OnPointerClick(PointerEventData eventData)
        {
            ToComeInPlaceEvent?.Invoke(GetPlaceId());
            ToComeInPlaceObjEvent?.Invoke();

            //_currentPositon = GameObject.Find("currentPosition");
            //_currentPositon.transform.SetPositionAndRotation(parent.position, parent.rotation);
        }

        public int GetPlaceId()
        {
            var parent = gameObject.transform.parent.transform.parent.parent;
            var placeId = parent.GetComponent<PlaceInteractiveField>().PlaceId;

            return placeId;
        }
    }


}