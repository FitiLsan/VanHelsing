using UnityEngine;
using System;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    public class ButtonClick : MonoBehaviour
    {

        #region Events

        public static event Action<string> KeyBoardButtonDownEvent;

        #endregion


        #region Methods

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
                                     