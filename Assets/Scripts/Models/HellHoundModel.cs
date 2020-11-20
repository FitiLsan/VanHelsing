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
        private InteractableObjectBehavior weaponIO;
        private SphereCollider detectionSphere;
        private HellHoundAttackStateBehaviour[] attackStates;

        public HellHoundData.BehaviourState BehaviourState;
        public Vector3 SpawnPoint;
        public Vector3 TargetPoint;
        public float IdlingTimer;
        public Transform ChasingTarget;
        public bool IsAttacking;

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
            Animator.SetFloat("JumpingBackSpeedRate", hellHoundData.Stats.BackJumpAnimationSpeedRate);
            Animator.SetFloat("JumpingBackIntensity", hellHoundData.Stats.BackJumpAnimationIntensity);

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

            attackStates = Animator.GetBehaviours<HellHoundAttackStateBehaviour>();
            for (int i = 0; i < attackStates.Length; i++)
            {
                attackStates[i].OnStateEnterHandler += OnAttackStateEnter;
                attackStates[i].OnStateExitHandler += OnAttackStateExit;
            }

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
            Damage damage = new Damage()
            {
                PhysicalDamage = hellHoundData.Stats.PhysicalDamage,
                StunProbability = hellHoundData.Stats.StunProbability
            };

            InteractableObjectBehavior enemy = collider.gameObject.GetComponent<InteractableObjectBehavior>();
            Debug.Log("The dog is attacking " + enemy);

            if (enemy != null) weaponIO.DealDamageEvent(enemy, damage);
            else Debug.LogError(this + " not found enemy InteractableObjectBehavior");

            AttackCollider.enabled = false;
        }

        void OnAttackStateEnter()
        {
            IsAttacking = true;
            AttackCollider.enabled = true;
        }

        void OnAttackStateExit()
        {
            IsAttacking = false;
            AttackCollider.enabled = false;
        }

        private bool DetectionFilter(Collider collider)
        {
            InteractableObjectBehavior IOBehavior = collider.GetComponent<InteractableObjectBehavior>();
            return !collider.isTrigger
                && collider.CompareTag(TagManager.PLAYER) && IOBehavior != null && IOBehavior.Type == InteractableObjectType.Player;
        }

        private void OnDetectionEnemy(ITrigger trigger, Collider collider)
        {
            Debug.Log("The dog noticed " + collider.name);
            ChasingTarget = collider.transform;
            BehaviourState = hellHoundData.SetChasingState(NavMeshAgent);
        }

        private void OnLostSightEnemy(ITrigger trigger, Collider collider)
        {
            if (collider.transform.Equals(ChasingTarget))
            {
                Debug.Log("The dog lost sight of the target");
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
