using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


namespace BeastHunter
{
    public class PlayerInteractionCatch : MonoBehaviour
    {
        public LookAtIK LookAtIK;
        public InteractionSystem interactionSystem; // Reference to the InteractionSystem component on the character
        public InteractionObject target; // The object to interact with
        public bool interrupt = false;
        public int ClosestTriggerIndex; // for debug

        private void Update()
        {
            InteractionTriggerUpdate();
        }
        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void InteractionTriggerUpdate()
        {
            ClosestTriggerIndex = interactionSystem.GetClosestTriggerIndex();
            if (ClosestTriggerIndex == -1)
            {
                target = null;
                LookAtIK.solver.target = null;
                return;
            }
            if (ClosestTriggerIndex >= 0)
            {
                target = interactionSystem.GetClosestInteractionObjectInRange();
                interactionSystem.TriggerInteraction(ClosestTriggerIndex, interrupt);
                LookAtIK.solver.target = target.transform;
                MessageBroker.Default.Publish(this);
            }
        }

        public void StartInteract()
        {
            interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, target, interrupt);
            interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, target, interrupt);
        }

        public void StopInteract()
        {
            interactionSystem.StopAll();
        }
    }
}