using UnityEngine;

namespace DialogueSystem
{
    public class ButtonClick : MonoBehaviour
    {
        public delegate void ButtonCheck(int number);
        public static event ButtonCheck MouseClickEvent;

        public delegate void KeyBoardButtonDown(string buttonName);
        public static event KeyBoardButtonDown KeyBoardButtonDownEvent;

        public void ButtonClickMouse(int buttonNumber)
        {
            MouseClickEvent?.Invoke(buttonNumber);
        }

        public void ButtonClickKeyBoard()
        {
            if(Input.anyKeyDown)
            {
                KeyBoardButtonDownEvent?.Invoke(Input.inputString);
            }
        }
        private void Update()
        {
            ButtonClickKeyBoard();
        }
    }
}
                                     