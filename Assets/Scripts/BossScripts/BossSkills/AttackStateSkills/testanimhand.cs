using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class testanimhand : MonoBehaviour
    {
        Animator animator;
        public Transform Rhand;
        public Transform HandTarget;
        public InteractionSystem interactionSystem; // Reference to the InteractionSystem component on the character
        public InteractionObject sphere; // The object to interact with
        public bool interrupt;
        public bool isCatch;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            interactionSystem.OnInteractionPickUp += OnPause;
            interactionSystem.OnInteractionResume += OnDrop;

        }
        private void Start()
        {
           
            animator.Play("IdleState", 0, 0);
            

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, sphere, interrupt);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, sphere, interrupt);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                // interactionSystem.PauseInteraction(FullBodyBipedEffector.RightHand);
                animator.Play("IdleState", 0, 0);
                sphere.transform.parent = null;
                sphere.GetComponent<Rigidbody>().isKinematic = false;

            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                animator.Play("BossCatchAttack_R", 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                animator.Play("BossFeastsAttack_1", 0, 0);
            }
        }
        private void OnPause(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
        {
            animator.Play("Catch_R_Idle", 0, 0);
            //  sphere.transform.parent = interactionSystem.transform;
        }
        private void OnDrop(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
        {
            animator.Play("IdleState", 0, 0);
        }
    }
}