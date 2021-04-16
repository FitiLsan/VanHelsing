using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class PlayerInteractionCatch : MonoBehaviour
    {
        public InteractionSystem interactionSystem; // Reference to the InteractionSystem component on the character
        public InteractionObject target; // The object to interact with
        public bool interrupt;
        public int ClosestTriggerIndex;
        public InteractionObject currentTarget;

        private void Update()
        {
            InteractionTriggerUpdate();

            if (Input.GetKeyDown(KeyCode.N))
            {
                interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, target, interrupt);
                interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, target, interrupt);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                interactionSystem.StopAll();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void InteractionTriggerUpdate()
        {
            ClosestTriggerIndex = interactionSystem.GetClosestTriggerIndex();
                if (ClosestTriggerIndex == -1)
                {
                    return;
                }
                target = interactionSystem.GetClosestInteractionObjectInRange();
                interactionSystem.TriggerInteraction(ClosestTriggerIndex, false, out currentTarget);

            
        }
    }
}