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
        private SphereCollider detectionSphere;
        private InteractableObjectBehavior weaponIO;

        public HellHoundData.BehaviourState BehaviourState;
        public Vector3 SpawnPoint;
        public float IdlingTimer;
        public Transform ChasingTarget;

        public float RotatePosition1;
        public float RotatePosition2;

        #endregion


        #region Properties

        public Animator Animator { get; }
        public GameObject HellHound { get; }
        public NavMeshAgent NavMeshAgent { get; }
        public Rigidbody Rigidbody { get; }
        public Transform Transform { get; }
        public Collider AttackCollider { get; }

        #endregion


        #region ClassLifeCycle

        public HellHoundModel(GameObject gameObject, HellHoundData hellHoundData)
        {
            this.hellHoundData = hellHoundData;
            HellHound = gameObject;

            Transform = HellHound.transform;
            BehaviourState = HellHoundData.BehaviourState.None;

            Rigidbody = HellHound.GetComponent<Rigidbody>();
            SpawnPoint = Rigidbody.position;

            Animator = HellHound.GetComponent<Animator>();
            Animator.SetFloat("JumpingSpeedRate", hellHoundData.Stats.JumpingSpeedRate);
            Animator.SetFloat("JumpingBackSpeedRate", hellHoundData.Stats.JumpingBackSpeedRate);
            Animator.SetFloat("JumpingBackForce", hellHoundData.Stats.JumpingBackForce);

            InteractableObjects = HellHound.GetComponentsInChildren<InteractableObjectBehavior>();

            detectionSphereIO = GetInteractableObject(InteractableObjectType.Sphere);
            detectionSphereIO.OnFilterHandler = DetectionFilter;
            detectionSphereIO.OnTriggerEnterHandler = OnDetectionEnemy;
            detectionSphereIO.OnTriggerExitHandler = OnLostSightEnemy;

            detectionSphere = detectionSphereIO.GetComponent<SphereCollider>();
            if (detectionSphere == null) Debug.LogError(this + " not found SphereCollider in DetectionSphere gameobject");
            else detectionSphere.radius = hellHoundData.Stats.DetectionRadius;

            weaponIO = HellHound.GetComponentInChildren<WeaponHitBoxBehavior>();
            weaponIO.OnFilterHandler = HitFilter;
            weaponIO.OnTriggerEnterHandler = OnHitEnemy;

            NavMeshAgent = HellHound.GetComponent<NavMeshAgent>();
            NavMeshAgent.angularSpeed = hellHoundData.Stats.AngularSpeed;
            NavMeshAgent.acceleration = hellHoundData.Stats.Acceleration;
            NavMeshAgent.stoppingDistance = hellHoundData.Stats.StoppingDistance;

            AttackCollider = weaponIO.GetComponent<BoxCollider>();
            AttackCollider.enabled = false;

            CurrentHealth = this.hellHoundData.BaseStats.MainStats.MaxHealth;
            IsDead = false;
        }

        #endregion


        #region Methods

        public void OnDead()
        {
            Animator.SetBool("IsDead", true);
            NavMeshAgent.enabled = false;
        }

        private bool HitFilter(Collider collider)
        {
            InteractableObjectBehavior IOBehavior = collider.GetComponent<InteractableObjectBehavior>();
            return !collider.isTrigger
                && collider.CompareTag(TagManager.PLAYER) && IOBehavior != null && IOBehavior.Type == InteractableObjectType.Player;
        }


        private void OnHitEnemy(ITrigger trigger, Collider collider)
        {
            Damage damage = new Damage() { PhysicalDamage = 10 };  //for debug only, need damage from basestats?

            InteractableObjectBehavior enemy = collider.gameObject.GetComponent<InteractableObjectBehavior>();
            Debug.Log("The dog is attacking : " + enemy);

            if (enemy != null) weaponIO.DealDamageEvent(enemy, damage);
            else Debug.LogError(this + " not found enemy InteractableObjectBehavior");

            AttackCollider.enabled = false;
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
                NavMeshAgent.speed = hellHoundData.Stats.MaxChasingSpeed;
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

        private InteractableObjectBehavior GetInteractableObject(InteractableObjectType type)
        {
            for (int i =0; i< InteractableObjects.Length; i++)
            {
                if (InteractableObjects[i].Type == type) return InteractableObjects[i];
            }
            Debug.LogWarning(this + "  not found InteractableObject of type " + type);
            return null;
        }

        private List<InteractableObjectBehavior> GetInteractableObjects(InteractableObjectType type)
        {
            List<InteractableObjectBehavior> result = new List<InteractableObjectBehavior>();
            for (int i = 0; i < InteractableObjects.Length; i++)
            {
                if (InteractableObjects[i].Type == type) result.Add(InteractableObjects[i]);
            }
            if (result.Count == 0) Debug.LogWarning(this + " not found InteractableObjects of type " + type);
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
