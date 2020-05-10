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
               // StartScript.GetStartScript.InputController.LockAction();
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
               // StartScript.GetStartScript.InputController.UnlockAction();
            }
        }

        #endregion
    }
}