using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public sealed class DialogueLoadToGUI
    {
        #region Fields

        private static StringBuilder stringBuilder = new StringBuilder();

        #endregion


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
                    stringBuilder.Clear();
                    buttonText.color = Color.green;
                    buttonText.text += stringBuilder.Append("(начать квест)");
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].IsEndQuest)
                {
                    stringBuilder.Clear();
                    buttonText.color = Color.red;
                    buttonText.text += stringBuilder.Append("(закончить квест)");
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].IsEnd)
                {
                    stringBuilder.Clear();
                    buttonText.text += stringBuilder.Append ("(выход)");
                }

                if (dialogueNode[currentNode].PlayerAnswers[i].HasTaskQuest)
                {
                    stringBuilder.Clear();
                    buttonText.color = Color.yellow;
                    buttonText.text += stringBuilder.Append("(задача)");
                }
            }
        }

        #endregion
    }
}
