using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class HitBoxTrigger : MonoBehaviour
    {
        public event Action HitActionEvent;
        private void OnTriggerEnter(Collider other)
        {
           var trig =  other.GetComponent<ITrigger>();
            if(trig!=null && trig.Type == InteractableObjectType.Player && !other.isTrigger)
            {
                HitActionEvent?.Invoke();
            }
        }
    }
}