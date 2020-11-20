using UnityEngine;


namespace BeastHunter
{
    public sealed class UIBestiarySwitcher : MonoBehaviour
    {
        #region Fields

        Canvas canvas;

        #endregion


        #region Methods

        public void On()
        {
            canvas.enabled = true;
            LockCharAction.LockAction(true);
        }

        public void Off()
        {
            canvas.enabled = false;
            LockCharAction.LockAction(false);
        }

        public void Switcher()
        {
            if (!canvas.enabled)
            {
                On();
            }
            else
            {
                Off();
            }
        }

        #endregion


        #region UnityMethods

        public void Start()
        {
            canvas = gameObject.GetComponent<Canvas>();
            canvas.enabled = false;
        }

        public void Update()
        {
            if (Input.GetButtonDown("Bestiary"))
            {
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.BestiaryOpened, null);
                Switcher();
            }

            if (Input.GetButtonDown("Cancel"))
            {
                if (canvas.enabled)
                {
                    Off();
                }
            }
        }

        #endregion
    }
}