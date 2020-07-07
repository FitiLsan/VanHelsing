using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace BeastHunter
{
    public class SelectNpcButtonClick : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<GameObject> SelectNpcClickEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            SelectNpcClickEvent?.Invoke(GetNpc());
        }

        public GameObject GetNpc()
        {
            return gameObject;
        }
    }
}