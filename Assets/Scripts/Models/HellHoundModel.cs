//using System;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BeastHunter
{
    public sealed class HellHoundModel : EnemyModel
    {
        #region Fields

        private HellHoundData hellHoundData;
        private Animator animator;
        private InteractableObjectBehavior hellHoundBehavior;

        public HellHoundData.BehaviourState BehaviourState;
        public Vector3 SpawnPoint;
        public float IdlingTimer;

        #endregion


        #region Properties

        public GameObject HellHound { get; }
        public NavMeshAgent NavMeshAgent { get; }
        public Rigidbody Rigidbody { get; }

        #endregion


        #region ClassLifeCycle

        public HellHoundModel(GameObject gameObject, HellHoundData hellHoundData)
        {
            this.hellHoundData = hellHoundData;
            HellHound = gameObject;

            animator = HellHound.GetComponent<Animator>();
            NavMeshAgent = HellHound.GetComponent<NavMeshAgent>();
            Rigidbody = HellHound.GetComponent<Rigidbody>();
            hellHoundBehavior = HellHound.GetComponent<InteractableObjectBehavior>();

            SpawnPoint = Rigidbody.position;
            BehaviourState = HellHoundData.BehaviourState.None;

            CurrentHealth = this.hellHoundData.BaseStats.MainStats.MaxHealth;
            IsDead = false;

            hellHoundBehavior.OnFilterHandler = OnFilterHandler;
            hellHoundBehavior.OnTriggerEnterHandler = OnTriggerEnterHandler;
        }

        #endregion

        #region Methods

        bool OnFilterHandler(Collider collider)
        {
            return collider.CompareTag(TagManager.PLAYER);
        }

        void OnTriggerEnterHandler(ITrigger trigger, Collider collider)
        {
            if (collider.CompareTag(TagManager.PLAYER) && !collider.isTrigger)
            {
                BehaviourState = HellHoundData.BehaviourState.Chasing;
            }
        }

        public void OnDead()
        {
            animator.SetBool("IsDead", true);
        }

        #endregion


        #region EnemyModel

        public override void DoSmth(string how)
        {
            hellHoundData.Do(how);
        }

        public override void Execute()
        {
            if (!IsDead)
            {
                hellHoundData.Act(this);
            }
        }

        public override EnemyStats GetStats()
        {
            return hellHoundData.BaseStats;
        }

        public override void OnAwake()
        {
            
        }

        public override void OnTearDown()
        {
            
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                hellHoundData.TakeDamage(this, damage);
            }
        }

        #endregion

    }
}
