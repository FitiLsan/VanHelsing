using UnityEngine;


namespace BeastHunter
{
    public sealed class ButtonClick : MonoBehaviour
    {

        #region Events

        public delegate void ButtonCheck(int number);
        public static event ButtonCheck MouseClickEvent;

        public delegate void KeyBoardButtonDown(string buttonName);
        public static event KeyBoardButtonDown KeyBoardButtonDownEvent;

        #endregion


        #region Methods

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

        #endregion


        #region UnityMethods

        private void Update()
        {
            ButtonClickKeyBoard();
        }

        #endregion

    }
}
                                     