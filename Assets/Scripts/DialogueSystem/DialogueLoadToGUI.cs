using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem;


namespace BeastHunter
{
    public sealed class DialogueLoadToGUI
    {

        #region Methods 

        public static void GetDialogueNode(List<Dialogue> dialogueNode, int currentNode, Text dialogueNPCText, Button[] AnswerButtons)
        {
            dialogueNPCText.text = dialogueNode[currentNode].npcText;

            for (var i = 0; i < dialogueNode[currentNode].PlayerAnswers.Count; i++)
            {
                var _buttonText = AnswerButtons[i].GetComponentInChildren<Text>();
                AnswerButtons[i].gameObject.SetActive(true);
                _buttonText.color = Color.black;
                _buttonText.name = dialogueNode[currentNode].PlayerAnswers[i].Text;
                _buttonText.text = dialogueNode[currentNode].PlayerAnswers[i].Text;

                if (dialogueNode[currentNode].PlayerAnswers[i].IsStartQuest)
                {
                    _buttonText.color = Color.green;
                    _buttonText.text += "(начать квест)";
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].IsEndQuest)
                {
                    _buttonText.color = Color.red;
                    _buttonText.text += "(закончить квест)";
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].IsEnd)
                {
                    _buttonText.text += "(выход)";
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].HasTaskQuest)
                {
                    _buttonText.color = Color.yellow;
                    _buttonText.text += "(задача)";
                }

            }
        }

        #endregion
    
    }
}
