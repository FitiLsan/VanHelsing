using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace BeastHunter
{
    public class ButtonMenuClick : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SceneManager.LoadScene(0);
        }


        private void RestartGame()
        {
            Debug.Log("RESTART GAME");
        }
    }
}