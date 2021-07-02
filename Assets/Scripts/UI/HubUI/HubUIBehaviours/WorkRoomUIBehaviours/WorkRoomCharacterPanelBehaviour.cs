using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunterHubUI
{
    class WorkRoomCharacterPanelBehaviour : MonoBehaviour, IDropHandler
    {
        public Action OnDropHandler { get; set; }

        public void OnDrop(PointerEventData eventData)
        {
            OnDropHandler?.Invoke();
        }
    }
}
