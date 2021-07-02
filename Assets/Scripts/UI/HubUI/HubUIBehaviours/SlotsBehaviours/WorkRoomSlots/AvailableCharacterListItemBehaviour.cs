using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    class AvailableCharacterListItemBehaviour : BaseCharacterSlotBehaviour
    {
        #region Fields

        [SerializeField] private Text _nameText;
        [SerializeField] private Text _rankText;
        [SerializeField] private Text _skillNameText;
        [SerializeField] private Text _skillLevelText;
        [SerializeField] private Image _skillLevelImage;
        [SerializeField] private GameObject _skillPanel;


        private RectTransform _rectTransform;
        private Vector2 _size;
        private SkillType _displayedSkill;

        #endregion


        #region Properties

        public Func<int, bool> IsPointerEnterOnFunc { get; set; }

        protected override int _storageSlotIndex
        {
            get
            {
                return gameObject.transform.GetSiblingIndex();
            }
            set
            {
                gameObject.transform.SetSiblingIndex(value);
            }
        }

        #endregion


        #region BaseCharacterSlotBehaviour

        public void SetDisplayedSkill(SkillType skillType, CharacterModel character)
        {
            _displayedSkill = skillType;
            UpdateDisplayedSkill(character);
        }

        private void UpdateDisplayedSkill(CharacterModel character)
        {
            if (_displayedSkill != SkillType.None)
            {
                _skillNameText.text = _displayedSkill.ToString();
                _skillLevelText.text = character.Skills[_displayedSkill].ToString() + "%";
                _skillLevelImage.fillAmount = (float)character.Skills[_displayedSkill] / 100;
                _skillPanel.SetActive(true);
            }
            else
            {
                _skillPanel.SetActive(false);
            }
        }

        public override void Initialize(CharacterStorageType storageType, int storageSlotIndex)
        {
            base.Initialize(CharacterStorageType.AvailableCharacters, storageSlotIndex);
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _size = _rectTransform.sizeDelta;
        }

        protected override void FillSlot(CharacterModel character)
        {
            base.FillSlot(character);
            _nameText.text = character.Name;
            _rankText.text = character.Rank.ToString();
            UpdateDisplayedSkill(character);
        }

        protected override void ClearSlot()
        {
            base.ClearSlot();
            _nameText.text = "";
            _rankText.text = "";
            _skillNameText.text = "";
            _skillLevelText.text = "";
            _skillLevelImage.fillAmount = 0;
        }

        protected override bool IsPointerEnterOn()
        {
            return base.IsPointerEnterOn()
                && IsPointerEnterOnFunc.Invoke(_storageSlotIndex);
        }

        protected override bool IsPointerExitOn()
        {
            return base.IsPointerExitOn()
                && IsPointerEnterOnFunc.Invoke(_storageSlotIndex);
        }

        protected override void OnBeginDragLogic()
        {
            base.OnBeginDragLogic();
            _objectForDrag.SetActive(false);
            Vector2 newSize = _size;
            newSize.y = 0;
            _rectTransform.sizeDelta = newSize;
        }

        protected override void OnEndDragLogic()
        {
            base.OnEndDragLogic();
            _rectTransform.sizeDelta = _size;
            _objectForDrag.SetActive(true);
        }

        protected override void OnDropLogic()
        {
            base.OnDropLogic();
            _rectTransform.sizeDelta = _size;
        }

        protected override void OnPointerEnterLogic()
        {
            base.OnPointerEnterLogic();
            Vector2 newSize = _size;
            newSize.y = newSize.y * 1.5f;
            _rectTransform.sizeDelta = newSize;
        }

        protected override void OnPointerExitLogic()
        {
            base.OnPointerExitLogic();
            _rectTransform.sizeDelta = _size;
        }

        #endregion
    }
}
