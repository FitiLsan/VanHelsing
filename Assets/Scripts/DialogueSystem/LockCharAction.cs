using UnityEngine;
using System;


namespace BeastHunter
{
    public sealed class LockCharAction
    {
        #region Properties

        public static Action<bool> LockCharacterMovement { get; set; }

        #endregion


        #region Methods

        public static void LockAction(bool isCanvasShow)
        {
            if (isCanvasShow)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            LockCharacterMovement?.Invoke(isCanvasShow);
        }

        #endregion
    }
}