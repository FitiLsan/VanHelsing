using RootMotion.Dynamics;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class testanimhand : MonoBehaviour
    {
        Animator animator;
        public InteractionSystem interactionSystem; // Reference to the InteractionSystem component on the character
        public InteractionObject target; // The object to interact with
        public Transform targetParent;
        public bool interrupt;
        public FullBodyBipedEffector currentHand;
        public int ClosestTriggerIndex;
        public InteractionObject currentTarget;
        public PuppetMaster puppetMaster;

        float time = 5f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        //  interactionSystem.OnInteractionPickUp += OnPickUp;
           // interactionSystem.OnInteractionStop += OnDrop;

        }
        private void Start()
        {
            animator.Play("IdleState", 0, 0);
            

        }
        private void Update()
        {
            time -= Time.deltaTime;
            InteractionTriggerUpdate();

            if (Input.GetKeyDown(KeyCode.B))
            {
                animator.Play("BossCatchAttack_R", 0, 0);
                TimeRemaining timeRemaining = new TimeRemaining(() => interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, target, interrupt), 0.2f);
                timeRemaining.AddTimeRemaining();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                animator.Play("BossCatch_L", 0, 0);
                interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, target, interrupt);
            }

            //if (Input.GetKeyDown(KeyCode.J))
            //{
            //    interactionSystem.PauseAll();
            //}
           // if (Input.GetKeyDown(KeyCode.H))
           if(time<=0)
            {
                time = 3f;
                animator.Play("FingerAttack", 0, 0);

            //    TimeRemaining timeRemaining = new TimeRemaining(() =>, 2f);
            //    timeRemaining.AddTimeRemaining(2f);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
               interactionSystem.StopInteraction(currentHand);
                target.GetComponent<Rigidbody>().isKinematic = false;
                currentTarget.transform.rotation = Quaternion.Euler(0,0,0);
               puppetMaster.mode = PuppetMaster.Mode.Active;
               puppetMaster.state = PuppetMaster.State.Frozen;
                TimeRemaining timeRemaining = new TimeRemaining(() => puppetMaster.state = PuppetMaster.State.Alive, 1f);
                timeRemaining.AddTimeRemaining(1f);

                if (currentHand == FullBodyBipedEffector.RightHand)
                {
                    animator.Play("IdleState", 0, 0);
                }
                if (currentHand == FullBodyBipedEffector.LeftHand)
                {
                    animator.Play("IdleState", 0, 0);
                }
                currentTarget.transform.parent = targetParent;
                
            }
        }
        private void OnPickUp(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
        {
            puppetMaster = targetParent.GetComponentInChildren<PuppetMaster>();
            puppetMaster.mode = PuppetMaster.Mode.Disabled;

            currentHand = effectorType;
            if (effectorType == FullBodyBipedEffector.LeftHand)
            {
                animator.SetFloat("IdleState", 9);
                animator.Play("Catch_Blend_Idle", 0, 0);
            }
            if (effectorType == FullBodyBipedEffector.RightHand)
            {
                animator.SetFloat("IdleState", 3);
                animator.Play("Catch_Blend_Idle", 0, 0);
            }
    
        }
        private void OnDrop(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
        {
            animator.SetFloat("IdleState", 6);
            animator.Play("Catch_Blend_Idle", 0, 0);
        }

        private void InteractionTriggerUpdate()
        {
            ClosestTriggerIndex = interactionSystem.GetClosestTriggerIndex();
           
            if (Input.GetKeyDown(KeyCode.G))
            {
               
                if (ClosestTriggerIndex == -1)
                {
                    return;
                }
                target = interactionSystem.GetClosestInteractionObjectInRange();
                targetParent = target.transform.root;
                interactionSystem.TriggerInteraction(ClosestTriggerIndex, false, out currentTarget);
              
            }
        }     
    }
}