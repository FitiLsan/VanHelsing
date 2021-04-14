using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class HubMapUIMapObjectBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button _button;
        [SerializeField] private Image _selectFrame;

        #endregion


        #region Properties

        public Action<HubMapUIMapObjectModel> OnClick_ButtonHandler;

        #endregion


        #region Methods

        public void FillInfo(HubMapUIMapObjectModel mapObjectModel)
        {
            _selectFrame.enabled = false;
            _button.onClick.AddListener(() => OnClick_Button(mapObjectModel));

            if (mapObjectModel.IsBlocked)
            {
                _button.GetComponentInChildren<Text>().text = "Заблокировано";
                _button.interactable = false;
            }
            else
            {
                _button.GetComponentInChildren<Text>().text = mapObjectModel.Name;
                _button.interactable = true;
            }
        }

        public void SelectFrameSwitch(bool flag)
        {
            _selectFrame.enabled = flag;
        }

        private void OnClick_Button(HubMapUIMapObjectModel mapObjectModel)
        {
            OnClick_ButtonHandler?.Invoke(mapObjectModel);
        }

        #endregion
    }
}
