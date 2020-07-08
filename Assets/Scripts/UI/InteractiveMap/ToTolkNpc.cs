using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public class ToTolkNpc : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<int> ToTalkClickEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            ToTalkClickEvent?.Invoke(GetNpcId());
        }

        public int GetNpcId()
        {
            var parent = gameObject.transform.parent.transform.parent;
            var npcId = parent.GetComponent<PlaceInteractiveField>().SelectedNpcId;
            return npcId;
        }
    }

}