using System;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BallistaData", menuName = "CreateData/SimpleInteractiveObjects/BallistaData", order = 0)]
    public sealed class BallistaData : SimpleInteractiveObjectData
    {
        #region Fields

        private GameObject currentTarget;
        private PlayerInteractionCatch _playerInteractionCatch;

        #endregion


        #region Methods

        public override void MakeInteractive(BaseInteractiveObjectModel interactiveObjectModel,
            ITrigger interactiveTrigger, Collider enteredCollider)
        {
            if (interactiveTrigger.GameObject != currentTarget)
            {
                return;
            }

            (interactiveObjectModel as BallistaModel).CanvasObject.gameObject.SetActive(true);
            interactiveObjectModel.IsInteractive = true;
        }

        public override void MakeNotInteractive(BaseInteractiveObjectModel interactiveObjectModel,
            ITrigger interactiveTrigger, Collider exitedCollider)
        {
            interactiveObjectModel.IsInteractive = false;
            (interactiveObjectModel as BallistaModel).CanvasObject.gameObject.SetActive(false);
            currentTarget = null;
        }

        protected override void Activate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            (interactiveObjectModel as BallistaModel).CanvasObject.gameObject.SetActive(false);
            (interactiveObjectModel as BallistaModel).IsActive = true;
            _playerInteractionCatch.StartInteract();
        }

        protected override void Deactivate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            (interactiveObjectModel as BallistaModel).CanvasObject.gameObject.SetActive(true);
            (interactiveObjectModel as BallistaModel).IsActive = false;
            _playerInteractionCatch.StopInteract();
        }

        public void CallTriggerInteraction(PlayerInteractionCatch playerInteractionCatch)
        {
            _playerInteractionCatch = playerInteractionCatch;
            currentTarget = playerInteractionCatch.target.gameObject;
        }


        #endregion
    }
}

