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
        private InteractableObjectBehavior[] behaviorObjects;
        private InteractableObjectBehavior detectionSphere;
        private List<InteractableObjectBehavior> hitBoxes;

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

            SpawnPoint = Rigidbody.position;
            BehaviourState = HellHoundData.BehaviourState.None;

            behaviorObjects = HellHound.GetComponentsInChildren<InteractableObjectBehavior>();
            hitBoxes = GetHitBoxList();
            detectionSphere = GetDetectionSphere();

            CurrentHealth = this.hellHoundData.BaseStats.MainStats.MaxHealth;
            IsDead = false;

            detectionSphere.OnFilterHandler = DetectionFilter;
            detectionSphere.OnTriggerEnterHandler = OnDetectionEnemy;
        }

        #endregion

        #region Methods

        public void OnDead()
        {
            animator.SetBool("IsDead", true);
        }

        private void OnDetectionEnemy(ITrigger trigger, Collider collider)
        {
            Debug.Log("The dog is chasing");
            BehaviourState = HellHoundData.BehaviourState.Chasing;
        }

        private bool DetectionFilter(Collider collider)
        {
            return !collider.isTrigger
                && (collider.CompareTag(TagManager.PLAYER) || collider.CompareTag(TagManager.RABBIT));
        }

        private InteractableObjectBehavior GetDetectionSphere()
        {
            /*Note: need  create a separate InteractableObjectType for detection triggers (for example "InteractableObjectType.DetectionRadius"),
            because if set trigger.Type to InteractableObjectType.Enemy, then the player deals damage on the detection trigger*/

            for (int i =0; i< behaviorObjects.Length; i++)
            {
                if (behaviorObjects[i].Type == InteractableObjectType.Sphere) return behaviorObjects[i];
            }
            return null;
        }

        private List<InteractableObjectBehavior> GetHitBoxList()
        {
            List<InteractableObjectBehavior> result = new List<InteractableObjectBehavior>();
            for (int i = 0; i < behaviorObjects.Length; i++)
            {
                if (behaviorObjects[i].Type == InteractableObjectType.HitBox) result.Add(behaviorObjects[i]);
            }
            return result;
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
