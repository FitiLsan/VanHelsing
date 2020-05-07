using UnityEngine;

namespace BeastHunter
{
    public sealed class QuestIndicatorModel
    {
        #region Fields

        public GameContext Context;

        #endregion


        #region Properties

        public Transform QuestIndicatorTransform { get; }
        public Transform NpcTransform { get; }
        public QuestIndicatorData QuestIndicatorData { get; }
        public QuestIndicatorStruct QuestIndicatorStruct { get; }


        #endregion


        #region ClassLifeCycle

        public QuestIndicatorModel(GameObject prefab, QuestIndicatorData questIndicatorData, GameObject npc, GameContext context)
        {
            QuestIndicatorTransform = prefab.transform;
            QuestIndicatorData = questIndicatorData;
            QuestIndicatorStruct = QuestIndicatorData.QuestIndicatorStruct;
            NpcTransform = npc.transform;
            Context = context;
        }

        #endregion


        #region Metods

        
        public void Execute()
        {
           QuestIndicatorData.QuestIndicatorCheck(Context);
        }

        #endregion
    }
}
