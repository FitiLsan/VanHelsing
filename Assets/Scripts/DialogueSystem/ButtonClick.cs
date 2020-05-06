using UnityEngine;
using System;

namespace BeastHunter
{
    public sealed class ButtonClick : MonoBehaviour
    {

        #region Events

        public static event Action<int>  MouseClickEvent;
        public static event Action<string> KeyBoardButtonDownEvent;

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
                                     