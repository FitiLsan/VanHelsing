using UnityEngine;


namespace BeastHunter
{
    public sealed class QuestJournalInitializeController : IAwake
    {
        #region Properties

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public QuestJournalInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region Methods

        public void OnAwake()
        {
            var questJournalData = Data.QuestJournalData;
            GameObject instance = GameObject.Instantiate(questJournalData.QuestJournalStruct.Prefab);
            QuestJournalModel questJournal = new QuestJournalModel(instance, questJournalData, _context);
            _context.QuestJournalModel = questJournal;
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestJournalCreated, null);
        }
        #endregion
    }
}