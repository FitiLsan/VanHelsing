using System;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "Dialogue/DialogueSystemData", order = 0)]
    public sealed class DialogueSystemData : ScriptableObject
    {
        #region Fields

        public DialogueSystemStruct DialogueSystemStruct;
        public DialogueSystemModel Model;

        #endregion


        #region Metods

        public void DialogueAnswerClear()
        {
            foreach (var answerButton in Model.AnswerButtons)
            {
                answerButton.gameObject.SetActive(false);
            }
        }

        public void DialogueUpdate()
        {
            if (Model.DialogueNode.Count != 0)
            {
                DialogueLoadToGUI.GetDialogueNode(Model.DialogueNode, Model.CurrentNode, Model.DialogueNPCText, Model.AnswerButtons);

                for (var i = Model.DialogueNode[Model.CurrentNode].PlayerAnswers.Count; i < Model.AnswerButtons.Length; i++)
                {
                    Model.AnswerButtons[i].gameObject.SetActive(false);
                }
            }
            else
            {
                DialogueAnswerClear();
            }
        }

        public void SelectAnswerById(EventArgs id)
        {
            SelectAnswer((id as IdArgs).Id);
        }

        public void SelectAnswer(int buttonNumber)
        {
           Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.DialogAnswerSelect, new DialogArgs(Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].AnswerId, Model.NpcID));

            if (Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].IsStartQuest)
            {
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestAccepted, new IdArgs(Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].QuestId));
            }

            if (Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].IsEndQuest)
            {
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.QuestReported, new IdArgs(Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].QuestId));
            }

            if (Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].IsEnd)
            {
                CanvasSwitcher(false);
                return;
            }
            Model.CurrentNode = Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].ToNode;
            DialogueUpdate();
        }

        public void CanvasSwitcher(bool isActive)
        {
            if (isActive)
            {
                DialogueUpdate();
            }
            else
            {
                Model.CurrentNode = 0;
            }

            Model.DialogueCanvas.enabled = isActive;
            LockCharAction.LockAction(isActive);
        }

        public void ButtonClickNumber(string buttonName)
        {
            if (Model.DialogueCanvas.enabled)
            {
                if (Model.DialogueNode.Count != 0 && Model.DialogueNode[Model.CurrentNode].PlayerAnswers.Count != 0)
                {
                    int buttonNum;

                    if (int.TryParse(buttonName, out buttonNum))
                    {
                        if (buttonNum - 1 < Model.DialogueNode[Model.CurrentNode].PlayerAnswers.Count)
                        {
                            SelectAnswer(buttonNum - 1);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
