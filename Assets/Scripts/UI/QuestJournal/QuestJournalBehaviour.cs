using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public sealed class QuestJournalBehaviour : MonoBehaviour, IPointerClickHandler
    {
        #region Fields

        public event Action<int> QuestButtonClickEvent;

        #endregion


        #region Methods

        public void OnPointerClick(PointerEventData eventData)
        {
            var name = eventData.pointerPress.name.Split('/');
            var questId = Convert.ToInt32(name[1]);

            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestButtonClickEvent, new IdArgs(questId)) ;
        }

        #endregion

    }
}