using UnityEngine;


namespace BeastHunter
{
    public sealed class LockCharAction
    {
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
        }

        #endregion
    }
}