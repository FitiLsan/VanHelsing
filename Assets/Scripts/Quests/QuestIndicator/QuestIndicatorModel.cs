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

        //public QuestIndicatorModel(GameObject prefab, QuestIndicatorData questIndicatorData, GameObject npc, GameContext context) //For 3D mode
        public QuestIndicatorModel(QuestIndicatorData questIndicatorData, GameContext context)
        {
            //For 3D mode  QuestIndicatorTransform = prefab.transform;
            PlaceButtonClick.ClickEvent += questIndicatorData.OnClickPlace;
            QuestIndicatorData = questIndicatorData;
            QuestIndicatorStruct = QuestIndicatorData.QuestIndicatorStruct;
            //For 3D mode   NpcTransform = npc.transform;
            Context = context;
            QuestIndicatorData.Context = context;
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.QuestUpdated, QuestIndicatorData.QuestIndicatorCheck);
        }

        #endregion


        #region Metods

        public void QuestIndicatorLookingAtCamera()
        {
            QuestIndicatorTransform.LookAt(Services.SharedInstance.CameraService.CharacterCamera.transform);
        }

        public void Execute()
        {
            QuestIndicatorLookingAtCamera();
        }

        #endregion
    }
}
