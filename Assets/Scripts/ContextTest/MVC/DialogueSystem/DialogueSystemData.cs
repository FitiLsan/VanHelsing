using DialogueSystem;
using Events;
using Events.Args;
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
            foreach (var b in Model.answerButtons)
            {
                b.gameObject.SetActive(false);
            }
        }

        public void DialogueUpdate()
        {
            if (Model.dialogueNode.Count != 0)
            {
                DialogueLoadToGUI.GetDialogueNode(Model.dialogueNode, Model.currentNode, Model.dialogueNPCText, Model.answerButtons);

                for (var i = Model.dialogueNode[Model.currentNode].PlayerAnswers.Count; i < Model.answerButtons.Length; i++)
                {
                    Model.answerButtons[i].gameObject.SetActive(false);
                }
            }
            else
            {
                DialogueAnswerClear();
            }
        }

        public void SelectAnswer(int buttonNumber)
        {
            EventManager.TriggerEvent(GameEventTypes.DialogAnswerSelect, new DialogArgs(Model.dialogueNode[Model.currentNode].PlayerAnswers[buttonNumber].AnswerId, Model.npcID));

            Debug.Log($"id answer {Model.dialogueNode[Model.currentNode].PlayerAnswers[buttonNumber].AnswerId}, id npc  {Model.npcID}");

            if (Model.dialogueNode[Model.currentNode].PlayerAnswers[buttonNumber].IsStartQuest)
            {
                EventManager.TriggerEvent(GameEventTypes.QuestAccepted, new IdArgs(Model.dialogueNode[Model.currentNode].PlayerAnswers[buttonNumber].QuestId));
            }

            if (Model.dialogueNode[Model.currentNode].PlayerAnswers[buttonNumber].IsEndQuest)
            {
                EventManager.TriggerEvent(GameEventTypes.QuestReported, new IdArgs(Model.dialogueNode[Model.currentNode].PlayerAnswers[buttonNumber].QuestId));
            }

            if (Model.dialogueNode[Model.currentNode].PlayerAnswers[buttonNumber].IsEnd)
            {
                CanvasSwitcher(false);
                return;
            }
            Model.currentNode = Model.dialogueNode[Model.currentNode].PlayerAnswers[buttonNumber].ToNode;
            DialogueUpdate();
        }

        public void CanvasSwitcher(bool isActive)
        {
            if (isActive)
            {
                Model.dialogueCanvas.enabled = true;
                DialogueUpdate();
                LockCharAction.LockAction(true);
            }
            else
            {
                Model.dialogueCanvas.enabled = false;
                Model.currentNode = 0;
                LockCharAction.LockAction(false);

            }
        }

        public void ButtonClickNumber(string buttonName)
        {
            if (Model.dialogueCanvas.enabled)
            {
                for (var i = 1; i < Model.answerButtons.Length + 1; i++)
                {
                    if (buttonName == i.ToString())
                    {
                        if (Model.dialogueNode.Count != 0 && Model.dialogueNode[Model.currentNode].PlayerAnswers.Count != 0)
                        {
                            if (i - 1 < Model.dialogueNode[Model.currentNode].PlayerAnswers.Count)
                            {
                                SelectAnswer(i - 1);
                            }
                            else
                            {
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
