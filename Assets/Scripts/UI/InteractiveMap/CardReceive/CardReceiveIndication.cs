using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class CardReceiveIndication : MonoBehaviour
    {
        private void Start()
        {
            
        }

        public static void GetCard(EventArgs args)
        {
            if(!(args is CardArgs cardArgs))
            {
                return;
            }
            Debug.Log($"Get new Card ID :{cardArgs.CardId}");
        }
    }
}