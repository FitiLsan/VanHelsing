using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


namespace BeastHunter
{
    public class PlayerBehaviour : MonoBehaviour
    {
        #region Fields

        public List<Collider> CollidersInTriger { get; private set; }
        public UnityEvent OnCollidersNearChange { get; private set; }

        #endregion


        #region UnityMethods

        public void Awake()
        {
            CollidersInTriger = new List<Collider>();

            if (OnCollidersNearChange == null)
            {
                OnCollidersNearChange = new UnityEvent();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            CollidersInTriger.Add(other);

            if (OnCollidersNearChange != null)
            {
                OnCollidersNearChange.Invoke();
            }
        }

        public void OnTriggerExit(Collider other)
        {
            CollidersInTriger.Remove(other);

            if (OnCollidersNearChange != null)
            {
                OnCollidersNearChange.Invoke();
            }
        }

        #endregion
    }
}

