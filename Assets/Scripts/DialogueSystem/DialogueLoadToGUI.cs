using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
                var buttonText = AnswerButtons[i].GetComponentInChildren<Text>();
                AnswerButtons[i].gameObject.SetActive(true);
                buttonText.color = Color.black;
                buttonText.name = dialogueNode[currentNode].PlayerAnswers[i].Text;
                buttonText.text = dialogueNode[currentNode].PlayerAnswers[i].Text;

                if (dialogueNode[currentNode].PlayerAnswers[i].IsStartQuest)
                {
                    buttonText.color = Color.green;
                    buttonText.text += "(начать квест)";
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].IsEndQuest)
                {
                    buttonText.color = Color.red;
                    buttonText.text += "(закончить квест)";
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].IsEnd)
                {
                    buttonText.text += "(выход)";
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].HasTaskQuest)
                {
                    buttonText.color = Color.yellow;
                    buttonText.text += "(задача)";
                }
            }
        }

        #endregion

    }
}
