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

        public void QuestionMarkShow(bool isOn, QuestIndicatorModel model)
        {
            model.QuestIndicatorTransform.GetChild(1).gameObject.SetActive(isOn);
        }

        public void ExclamationMarkShow(bool isOn, QuestIndicatorModel model)
        {
            model.QuestIndicatorTransform.GetChild(0).gameObject.SetActive(isOn);
        }

        public void SetPosition(Transform npcTransform, Transform questIndicatorTransform)
        {
            questIndicatorTransform.position = new Vector3(npcTransform.position.x, npcTransform.position.y + CANVAS_OFFSET, npcTransform.position.z);
            questIndicatorTransform.parent = npcTransform;
        }

        public void QuestIndicatorCheck(GameContext context)
        {
            foreach (QuestIndicatorModel questIndicatorModel in context.QuestIndicatorModelList)
            {
                questIndicatorModel.QuestIndicatorTransform.LookAt(context.CharacterModel.CharacterCamera.transform);

                questIndicatorModel.QuestIndicatorData.GetQuestInfo(questIndicatorModel.NpcTransform.GetComponent<IGetNpcInfo>().GetInfo().Item1, questIndicatorModel);
            }
        }

        public void GetQuestInfo(int npcID, QuestIndicatorModel model)
        {
            var questStartInfo = DatabaseWrapper.GetTable($"select * from 'dialogue_answers' where Npc_id={npcID} and Start_quest=1;");
            if (questStartInfo.Rows.Count != 0)
            {
                ExclamationMarkShow(true, model);
            }
            var questEndInfo = DatabaseWrapper.GetTable($"select * from 'dialogue_answers' where Npc_id={npcID} and End_quest=1;");

            if (questEndInfo.Rows.Count != 0)
            {
                QuestionMarkShow(true, model);
            }

            var questTaskInfo = DatabaseWrapper.GetTable($"select * from 'dialogue_answers' where Npc_id={npcID} and Task_quest=1;");

            if (questTaskInfo.Rows.Count != 0)
            {
                QuestionMarkShow(true, model);
            }

        }
        #endregion

    }
}
