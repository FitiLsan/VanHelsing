using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public class ToSearchPlace : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<List<ItemInfo>> ToSearchPlaceEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            ToSearchPlaceEvent?.Invoke(GetPlaceItemList());
        }

        public List<ItemInfo> GetPlaceItemList()
        {
            var parent = gameObject.transform.parent.transform.parent;
            var items = parent.GetComponent<PlaceInteractiveField>().ItemList;
            
            return items;
        }
    }
}