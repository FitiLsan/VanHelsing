using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public sealed class AnswerButtonBehaviour : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            int buttonNumber = int.Parse(eventData.pointerPress.name.Replace("AnswerButton", "")) - 1;
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.AnswerButtonClicked, new IdArgs(buttonNumber));
        }
    }
}