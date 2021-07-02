using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    public class MapCharacterBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] Image _characterImage;
        [SerializeField] Image _selectFrameImage;

        #endregion


        #region Properties

        public Action<CharacterModel> OnClick_ButtonHandler;

        #endregion


        #region Methods

        public void Initialize(CharacterModel character)
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

        private void OnClick_Button(CharacterModel character)
        {
            OnClick_ButtonHandler?.Invoke(character);
        }

        #endregion
    }
}
