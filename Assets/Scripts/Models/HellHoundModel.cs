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
        private InteractableObjectBehavior[] InteractableObjects;
        private InteractableObjectBehavior detectionSphereIO;
        private List<InteractableObjectBehavior> hitBoxesIO;
        private SphereCollider detectionSphere;

        public HellHoundData.BehaviourState BehaviourState;
        public Vector3 SpawnPoint;
        public float IdlingTimer;
        public Transform ChasingTarget;

        #endregion


        #region Properties

        public Animator Animator { get; }
        public GameObject HellHound { get; }
        public NavMeshAgent NavMeshAgent { get; }
        public Rigidbody Rigidbody { get; }
        public Transform Transform { get; }

        #endregion


        #region ClassLifeCycle

        public HellHoundModel(GameObject gameObject, HellHoundData hellHoundData)
        {
            this.hellHoundData = hellHoundData;
            HellHound = gameObject;

            Animator = HellHound.GetComponent<Animator>();
            NavMeshAgent = HellHound.GetComponent<NavMeshAgent>();
            Rigidbody = HellHound.GetComponent<Rigidbody>();
            Transform = HellHound.transform;

            SpawnPoint = Rigidbody.position;
            BehaviourState = HellHoundData.BehaviourState.None;

            InteractableObjects = HellHound.GetComponentsInChildren<InteractableObjectBehavior>();
            //hitBoxesIO = GetHitBoxListIO(); //todo
            detectionSphereIO = GetDetectionSphereIO();
            detectionSphere = detectionSphereIO.GetComponent<SphereCollider>();

            if (detectionSphere == null) Debug.LogWarning(HellHound.name + " not found SphereCollider in DetectionSphere gameobject");
            else detectionSphere.radius = hellHoundData.Stats.DetectionRadius;

            CurrentHealth = this.hellHoundData.BaseStats.MainStats.MaxHealth;
            IsDead = false;

            detectionSphereIO.OnFilterHandler = DetectionFilter;
            detectionSphereIO.OnTriggerEnterHandler = OnDetectionEnemy;
            detectionSphereIO.OnTriggerExitHandler = OnLostSightEnemy;
        }

        #endregion

        #region Methods

        public void OnDead()
        {
            Animator.SetBool("IsDead", true);
            NavMeshAgent.enabled = false;
        }

        private bool DetectionFilter(Collider collider)
        {
            return !collider.isTrigger
                && (collider.CompareTag(TagManager.PLAYER) && collider.gameObject.name == "Player"
                || collider.CompareTag(TagManager.RABBIT));
        }

        private void OnDetectionEnemy(ITrigger trigger, Collider collider)
        {
            if (collider.CompareTag(TagManager.PLAYER) && (ChasingTarget == null || ChasingTarget.name != "Player"))
            {
                Debug.Log("The dog is chasing " + collider.name);
                ChasingTarget = collider.transform;
                BehaviourState = HellHoundData.BehaviourState.Chasing;
                NavMeshAgent.speed = hellHoundData.Stats.RunSpeed;
            }
            else if (collider.CompareTag(TagManager.RABBIT) && ChasingTarget == null)
            {
                Debug.Log("The dog is chasing " + collider.name);
                ChasingTarget = collider.transform;
                BehaviourState = HellHoundData.BehaviourState.Chasing;
            }
        }

        private void OnLostSightEnemy(ITrigger trigger, Collider collider)
        {
            if (collider.transform.Equals(ChasingTarget))
            {
                Debug.Log("The dog is stopped chasing");
                ChasingTarget = null;
                BehaviourState = HellHoundData.BehaviourState.None;
            }
        }

        private InteractableObjectBehavior GetDetectionSphereIO()
        {
            /*Note: need  create a separate InteractableObjectType for detection triggers (for example "InteractableObjectType.DetectionRadius"),
            because if set trigger.Type to InteractableObjectType.Enemy, then the player deals damage on the detection trigger*/

            for (int i =0; i< InteractableObjects.Length; i++)
            {
                if (InteractableObjects[i].Type == InteractableObjectType.Sphere) return InteractableObjects[i];
            }
            Debug.LogError("Not found InteractableObjectType.Sphere in " + HellHound.name);
            return null;
        }

        private List<InteractableObjectBehavior> GetHitBoxListIO()
        {
            List<InteractableObjectBehavior> result = new List<InteractableObjectBehavior>();
            for (int i = 0; i < InteractableObjects.Length; i++)
            {
                if (InteractableObjects[i].Type == InteractableObjectType.HitBox) result.Add(InteractableObjects[i]);
            }
            if (result.Count == 0) Debug.LogWarning("Not found InteractableObjectType.HitBox in " + HellHound.name);
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
