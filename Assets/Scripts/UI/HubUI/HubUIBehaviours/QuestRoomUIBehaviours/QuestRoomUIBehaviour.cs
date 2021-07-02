using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    public class QuestRoomUIBehaviour : MonoBehaviour, IStart
    {
        #region Fields

        [SerializeField] private GameObject _questListPanel;
        [SerializeField] private Text _questDescriptionText;
        [SerializeField] private Button _questApplyButton;

        private HubUIData _data;
        private HubUIContext _context;
        private HuntingQuestModel _selectedQuest;
        private List<HuntingQuestListItemBehaviour> _currentQuestListItems;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _questApplyButton.onClick.AddListener(OnClick_QuestApplyButton);
        }

        private void OnDisable()
        {
            _questApplyButton.onClick.RemoveAllListeners();
        }

        #endregion


        #region IStart

        public void Starting(HubUIContext context)
        {
            if (context.Player.HuntingQuest == null)
            {
                _data = BeastHunter.Data.HubUIData;
                _context = context;
                _currentQuestListItems = new List<HuntingQuestListItemBehaviour>();

                List<HuntingQuestModel> quests = HubUIServices.SharedInstance.HuntingQuestService.GetRandomQuests();

                for (int i = 0; i < quests.Count; i++)
                {
                    GameObject questUI = InstantiateUIObject(_data.QuestRoomData.QuestListItemPrefab, _questListPanel);
                    HuntingQuestListItemBehaviour questBehaviour = questUI.GetComponent<HuntingQuestListItemBehaviour>();
                    questBehaviour.FillInfo(quests[i]);
                    questBehaviour.OnClickButtonHandler += OnClick_QuestListItem;
                    _currentQuestListItems.Add(questBehaviour);
                }

                _questApplyButton.enabled = false;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        #endregion


        #region Methods

        private void OnClick_QuestListItem(HuntingQuestModel quest)
        {
            _questDescriptionText.text = quest.Description;
            _selectedQuest = quest;
            _questApplyButton.enabled = true;
        }

        private void OnClick_QuestApplyButton()
        {
            if (_selectedQuest != null)
            {
                _context.Player.TakeHuntingQuest(_selectedQuest);
                HubUIServices.SharedInstance.GameMessages.Notice($"The quest {_selectedQuest.Title} started. Time expires at: {_selectedQuest.RunningOutTime}");
                CloseAndClear();
            }
        }

        private void CloseAndClear()
        {
            gameObject.SetActive(false);
            _questDescriptionText.text = null;
            for (int i = 0; i < _currentQuestListItems.Count; i++)
            {
                Destroy(_currentQuestListItems[i].gameObject);
            }
            _currentQuestListItems.Clear();
        }

        private GameObject InstantiateUIObject(GameObject prefab, GameObject parent)
        {
            GameObject objectUI = GameObject.Instantiate(prefab);
            objectUI.transform.SetParent(parent.transform, false);
            objectUI.transform.localScale = new Vector3(1, 1, 1);
            return objectUI;
        }

        #endregion
    }
}
