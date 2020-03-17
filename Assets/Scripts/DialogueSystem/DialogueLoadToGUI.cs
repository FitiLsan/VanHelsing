using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{

    public class DialogueLoadToGUI
    {        
    public static void GetDialogueNode(List<Dialogue> dialogueNode, int _currentNode, Text _dialogueNPCText, Button[] _AnswerButtons)
        {
            _dialogueNPCText.text = dialogueNode[_currentNode].NpcText;

            for (var i = 0; i < dialogueNode[_currentNode].PlayerAnswers.Count; i++)
            {
               var _buttonText = _AnswerButtons[i].GetComponentInChildren<Text>();

                _AnswerButtons[i].gameObject.SetActive(true);
                _buttonText.color = Color.black;
                _buttonText.name = dialogueNode[_currentNode].PlayerAnswers[i].Text;
                _buttonText.text = dialogueNode[_currentNode].PlayerAnswers[i].Text;

                if (dialogueNode[_currentNode].PlayerAnswers[i].IsStartQuest)
                {
                    _buttonText.color = Color.green;
                    _buttonText.text += "(начать квест)";

                }
                if (dialogueNode[_currentNode].PlayerAnswers[i].IsEndQuest)
                {
                    _buttonText.color = Color.red;
                    _buttonText.text += "(закончить квест)";
                }
                if (dialogueNode[_currentNode].PlayerAnswers[i].IsEnd)
                {
                    _buttonText.text += "(выход)";
                }
                if (dialogueNode[_currentNode].PlayerAnswers[i].HasTaskQuest)
                {
                    _buttonText.color = Color.yellow;
                    _buttonText.text += "(задача)";
                }

            }
        }
    }
}
