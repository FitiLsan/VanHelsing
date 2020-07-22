using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public sealed class ToComeInPlace : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<int> ToComeInPlaceEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            ToComeInPlaceEvent?.Invoke(GetPlaceId());
        }

        public int GetPlaceId()
        {
            var parent = gameObject.transform.parent.transform.parent;
            var placeId = parent.GetComponent<PlaceInteractiveField>().PlaceId;

            return placeId;
        }
    }


}