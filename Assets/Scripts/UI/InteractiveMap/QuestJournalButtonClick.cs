using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeastHunter
{
    public class QuestJournalButtonClick : MonoBehaviour, IPointerClickHandler
    {
        public static event Action<bool> ClickQuestJournalButton;

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickQuestJournalButton?.Invoke(true);
        }
    }
}