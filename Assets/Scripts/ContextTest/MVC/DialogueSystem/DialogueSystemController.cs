using DialogueSystem;
using Events;
using Events.Args;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class DialogueSystemController : IUpdate
    {
        #region Fields
        private readonly GameContext _context;
        #endregion
        #region Properties
        public DialogueSystemModel Model { get; private set; }
        public DialogueSystemView View { get; private set; }
        #endregion
        #region ClassLifeCycle
        public DialogueSystemController(GameContext context, Services services)
        {
            _context = context;
       //     Model = new DialogueSystemModel();

            ButtonClick.MouseClickEvent += SelectAnswer;
            ButtonClick.KeyBoardButtonDownEvent += ButtonClickNumber;
           // StartDialogueData.ShowCanvasEvent += CanvasSwitcher;
        }
        #endregion
        #region Updating
        public void Updating()
        {
        }
        #endregion
        public void DialogueAnswerClear()
            {
                foreach (var b in View.answerButtons)
                {
                    b.gameObject.SetActive(false);
                }
            }

            public void DialogueUpdate()
            {
                if (Model.dialogueNode.Count != 0)
                {
                    DialogueLoadToGUI.GetDialogueNode(Model.dialogueNode, Model.currentNode, View.dialogueNPCText, View.answerButtons);

                    for (var i = Model.dialogueNode[Model.currentNode].PlayerAnswers.Count; i < View.answerButtons.Length; i++)
                    {
                        View.answerButtons[i].gameObject.SetActive(false);
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
                    View.dialogueCanvas.enabled = true;
                    DialogueUpdate();
                    LockCharAction.LockAction(true);
                }
                else
                {
                    View.dialogueCanvas.enabled = false;
                    Model.currentNode = 0;
                    LockCharAction.LockAction(false);

                }
            }

            public void ButtonClickNumber(string buttonName)
            {
            View = GameObject.FindObjectOfType<DialogueSystemView>();
            Model = _context._dialogueSystemModel;
            if (View.dialogueCanvas.enabled)
                {
                    for (var i = 1; i < View.answerButtons.Length + 1; i++)
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
            public void GetView(DialogueSystemView view)
            {
                View = view;
            }
      
    }
}
