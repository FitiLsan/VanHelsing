using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public class ToTolkNpc : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<int> ToTalkClickEvent;
        public GameObject SelectedNpc;
        public void OnPointerClick(PointerEventData eventData)
        {

            SelectedNpc = eventData.pointerEnter.transform.parent.parent.gameObject;
            ToTalkClickEvent?.Invoke(GetNpcId());
        }

        public int GetNpcId()
        {
            var npcId = SelectedNpc.GetComponent<NpcIdInfo>().NpcID;
            return npcId;
        }
    }

}