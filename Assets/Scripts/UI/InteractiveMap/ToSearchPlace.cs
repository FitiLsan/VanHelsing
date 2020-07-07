using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public class ToSearchPlace : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<int> ToSearchPlaceEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            ToSearchPlaceEvent?.Invoke(GetPlaceId());
        }

        public int GetPlaceId()
        {
            var parent = gameObject.transform.parent.transform.parent;
            var placeId = parent.GetComponent<PlaceInteractiveField>().PlaceId;
            return placeId;
        }
    }
}