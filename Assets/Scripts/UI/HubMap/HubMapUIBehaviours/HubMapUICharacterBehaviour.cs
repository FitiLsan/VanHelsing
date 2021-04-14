using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class HubMapUICharacterBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] Image _characterImage;
        [SerializeField] Image _selectFrameImage;

        #endregion


        #region Properties

        public Action<HubMapUICharacterModel> OnClick_ButtonHandler;

        #endregion


        #region Methods

        public void Initialize(HubMapUICharacterModel character)
        {
            character.Behaviour = this;
            _selectFrameImage.enabled = false;
            _characterImage.sprite = character.Portrait;
            GetComponent<Button>().onClick.AddListener(() => OnClick_Button(character));
        }

        public void SelectFrameSwitch(bool flag)
        {
            _selectFrameImage.enabled = flag;
        }

        private void OnClick_Button(HubMapUICharacterModel character)
        {
            OnClick_ButtonHandler?.Invoke(character);
        }

        #endregion
    }
}
