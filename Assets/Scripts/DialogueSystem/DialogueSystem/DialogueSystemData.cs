using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/DialogueSystem", order = 0)]
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

        public void SelectAnswer(int buttonNumber)
        {
            EventManager.TriggerEvent(GameEventTypes.DialogAnswerSelect, new DialogArgs(Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].AnswerId, Model.NpcID));

           // Debug.Log($"id answer {Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].AnswerId}, id npc  {Model.NpcID}");

            if (Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].IsStartQuest)
            {
                EventManager.TriggerEvent(GameEventTypes.QuestAccepted, new IdArgs(Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].QuestId));
            }

            if (Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].IsEndQuest)
            {
                EventManager.TriggerEvent(GameEventTypes.QuestReported, new IdArgs(Model.DialogueNode[Model.CurrentNode].PlayerAnswers[buttonNumber].QuestId));
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
                            switch (buttonNum)
                            {
                                case 1:
                                    SelectAnswer(0);
                                    break;

                                case 2:
                                    SelectAnswer(1);
                                    break;

                                case 3:
                                    SelectAnswer(2);
                                    break;

                                case 4:
                                    SelectAnswer(3);
                                    break;

                                default:
                                    return;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
