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
            var list = Services.SharedInstance.PhysicsService.GetObjectsInRadius(CharacterTransform.position, 100f, 11);

            GameObject instance = GameObject.Instantiate(QuestIndicatorData.QuestIndicatorStruct.Prefab);
            QuestIndicatorModel QuestIndicator = new QuestIndicatorModel(instance, QuestIndicatorData, _context);
            _context.QuestIndicatorModel = QuestIndicator;
        }

            #endregion
        }
    }
