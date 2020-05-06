using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/QuestIndicator", order = 0)]
    public sealed class QuestIndicatorData : ScriptableObject
    {
        #region Fields

        public const float CANVAS_OFFSET = 2.3f;

        public QuestIndicatorStruct QuestIndicatorStruct;
        public Vector3 NpcPos;
        public int NpcID;

        #endregion


        #region Methods

        public void QuestionMarkShow()
        {
            QuestIndicatorStruct.Prefab.GetComponent<Image>().enabled = true;
        }

        public void ExclamationMarkShow()
        {
            QuestIndicatorStruct.Prefab.GetComponent<Image>().enabled = true;
        }

        public void SetPosition(Vector3 npcPosition)
        {

        }
        #endregion

    }
}
