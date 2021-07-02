using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    class WorkRoomButtonBehaviour : MonoBehaviour
    {
        [SerializeField] Text _roomNameText;
        [SerializeField] Text _roomTimeText;


        public Action<WorkRoomModel> OnClickButtonHandler { get; set; }


        public void Initialize(WorkRoomModel room)
        {
            _roomNameText.text = room.Name;
            UpdateOrderTime(room.MinOrderCompleteTime);
            GetComponent<Button>().onClick.AddListener(() => OnClickButton(room));
        }

        public void UpdateOrderTime(int time)
        {
            _roomTimeText.text = $"({time} {HubUIServices.SharedInstance.TimeService.GetHoursWord(time)})";
        }

        private void OnClickButton(WorkRoomModel room)
        {
            OnClickButtonHandler?.Invoke(room);
        }
    }
}
