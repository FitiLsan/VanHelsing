using UnityEngine;


namespace BeastHunter
{
    public sealed class QuestIndicatorInitializeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public QuestIndicatorInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var QuestIndicatorData = Data.QuestIndicatorData;
            var CharacterTransform = _context.CharacterModel.CharacterTransform.transform;
            var list = Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(CharacterTransform.position, 100f, "NPC");

            for (int i = 0; i < list.Count; i++)
            {
                GameObject instance = GameObject.Instantiate(QuestIndicatorData.QuestIndicatorStruct.Prefab);
                QuestIndicatorModel QuestIndicator = new QuestIndicatorModel(instance, QuestIndicatorData, list[i].gameObject, _context);
                _context.QuestIndicatorModelList.Add(QuestIndicator);
                QuestIndicatorData.SetPosition(list[i].transform, instance.transform);
            }
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestUpdated, null);
        }
        #endregion
    }
}
