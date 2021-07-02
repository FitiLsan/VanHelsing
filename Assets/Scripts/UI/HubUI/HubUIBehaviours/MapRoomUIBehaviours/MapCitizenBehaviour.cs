using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    class MapCitizenBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Text _citizenNamePanel;
        [SerializeField] private Image _citizenPortrait;
        [SerializeField] private GameObject _exclamationImage;
        [SerializeField] private GameObject _questionImage;

        #endregion


        #region Properties

        public Action<CitizenModel> OnClick_CitizenButtonHandler { get; set; }

        #endregion


        #region Methods

        public void FillCitizenInfo(CitizenModel citizen)
        {
            _citizenNamePanel.text = citizen.Name;
            _citizenPortrait.sprite = citizen.Portrait;
            GetComponent<Button>().onClick.AddListener(() => OnClick_CitizenButton(citizen));
            SetQuestMarker(citizen.QuestMarkerType);
        }

        public void OnClick_CitizenButton(CitizenModel citizen)
        {
            OnClick_CitizenButtonHandler?.Invoke(citizen);
        }

        public void SetQuestMarker(QuestMarkerType questMarkerType)
        {
            switch (questMarkerType)
            {
                case QuestMarkerType.Exclamation: _exclamationImage.SetActive(true); break;
                case QuestMarkerType.Question: _questionImage.SetActive(true); break;
                default: 
                    _exclamationImage.SetActive(false);
                    _questionImage.SetActive(false);
                    break;
            }
        }

        #endregion
    }
}
