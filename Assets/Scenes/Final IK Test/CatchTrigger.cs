using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class CatchTrigger : MonoBehaviour
    {
        public static event Action<GameObject, GameObject> CatchedEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player") && !other.isTrigger )
            {
                CatchedEvent?.Invoke(gameObject, other.gameObject);
            }
        }
    }
}