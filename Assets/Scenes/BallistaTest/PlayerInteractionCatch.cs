using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

namespace BeastHunter
{
    public class PlayerInteractionCatch : MonoBehaviour
    {
        public LookAtIK LookAtIK;
        public InteractionSystem interactionSystem; // Reference to the InteractionSystem component on the character
        public InteractionObject target; // The object to interact with
        public bool interrupt = false;
        public int ClosestTriggerIndex; // for debug

        [SerializeField] private InteractionObject _hitTargetUp;
        [SerializeField] private InteractionObject _hitTargetMiddle;
        [SerializeField] private InteractionObject _hitTargetDown;

        private CinemachineFreeLook _cameraYAxis;
        private InteractionObject _hitTarget;

        private void Start()
        {
              Services.SharedInstance.Context.InputModel.OnPressNumberFour += () => TryHit(_hitTargetUp, 0.2f);
            //  Services.SharedInstance.Context.InputModel.OnPressNumberThree += () => TryHit();
            // Services.SharedInstance.Context.InputModel.OnPressNumberTwo += () => TryHit();
          
            Services.SharedInstance.Context.InputModel.OnAttackOffset += () => TryHit(_hitTarget, 0.3f);
            _cameraYAxis = Services.SharedInstance.CameraService.CharacterFreelookCamera;
        }
        private void Update()
        {
               InteractionTriggerUpdate();
            Debug.Log($"Camera Y: {_cameraYAxis.m_YAxis.Value}");
            if (_cameraYAxis.m_YAxis.Value > 0.7f)
                _hitTarget = _hitTargetDown;
            else if (_cameraYAxis.m_YAxis.Value < 0.1)
                _hitTarget = _hitTargetUp;
            else _hitTarget = null;
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

        private void TryHit(InteractionObject target, float delay)
        {
            if (target == null)
                return;
            DOVirtual.DelayedCall(delay, () =>
            {
                interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, target, false);
            });
        }
    }
}