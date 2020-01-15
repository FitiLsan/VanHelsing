using UnityEngine;

namespace DialogueSystem

{
    public class ButtonClick : MonoBehaviour
    {
        private int _toNode;
        private DialogueSystem dialogueSystem;


        public void OnButtonClick(int buttonNumber)
        {
            dialogueSystem = FindObjectOfType<DialogueSystem>();
            dialogueSystem._currentNode = _toNode;
            dialogueSystem.DialogueAnswerClear();
        }
    }
}