using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeastHunter
{
    public class ResetMenu : MonoBehaviour
    {
        public Canvas MenuPrefab;

        private void Start()
        {
            Time.timeScale = 1;
            MenuPrefab.enabled = false;
            Services.SharedInstance.Context.InputModel.OnPressCancel += Switcher;
        }

        public void On()
        {
            MenuPrefab.enabled = true;
            LockCharAction.LockAction(true);
            Time.timeScale = 0;
        }

        public void Off()
        {
            MenuPrefab.enabled = false;
            LockCharAction.LockAction(false);
            Time.timeScale = 1;
        }

        public void Switcher()
        {
            if (!MenuPrefab.enabled)
            {
                On();
            }
            else
            {
                Off();
            }
        }
    }

}