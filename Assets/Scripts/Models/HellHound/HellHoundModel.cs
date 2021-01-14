using UnityEngine;
using UnityEngine.AI;
using Extensions;
using System.Collections.Generic;

namespace BeastHunter
{
    public sealed class HellHoundModel : EnemyModel
    {
        #region Fields

        private GameObject _prefab;
        private HellHoundData _hellHoundData;
        private InteractableObjectBehavior[] _interactableObjects;
        private InteractableObjectBehavior _weaponIO;
        private SphereCollider _detectionSphere;
        private HellHoundAttackStateBehaviour[] _attackStates;
        private bool _isCleared;

        public float StateTimer;
        public float RotatePosition1;
        public float RotatePosition2;

        #endregion


        #region Properties

        public Animator Animator { get; }
        public NavMeshAgent NavMeshAgent { get; }
        public Transform Transform { get; }
        public Collider AttackCollider { get; }
        public float JumpingAttackTimer { get; set; }
        public HellHoundData.BehaviourState BehaviourState { get; set; }
        public Vector3 SpawnPoint { get; }
        public Transform ChasingTarget { get; set; }
        public bool IsAttacking { get; set; }

        #endregion


        #region ClassLifeCycle

        public HellHoundModel(GameObject prefab, HellHoundData hellHoundData)
        {
            _hellHoundData = hellHoundData;
            _prefab = prefab;

            Transform = _prefab.transform;
            BehaviourState = HellHoundData.BehaviourState.None;

            SpawnPoint = Transform.position;

            Animator = _prefab.GetComponent<Animator>();
            Animator.SetFloat("JumpSpeedRate", hellHoundData.Stats.JumpingSpeedRate);
            Animator.SetFloat("JumpBackSpeedRate", hellHoundData.Stats.BackJumpAnimationSpeedRate);
            Animator.SetFloat("JumpBackIntensity", hellHoundData.Stats.BackJumpAnimationIntensity);

            _interactableObjects = _prefab.GetComponentsInChildren<InteractableObjectBehavior>();

            InteractableObjectBehavior _detectionSphereIO = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.Sphere);
            _detectionSphereIO.OnFilterHandler = Filter;
            _detectionSphereIO.OnTriggerEnterHandler = OnDetectionEnemy;
            _detectionSphereIO.OnTriggerExitHandler = OnLostEnemy;

            _detectionSphere = _detectionSphereIO.GetComponent<SphereCollider>();
            if (_detectionSphere == null) Debug.LogError(this + " not found SphereCollider in DetectionSphere gameobject");
            else _detectionSphere.radius = hellHoundData.Stats.DetectionRadius;

            _weaponIO = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.HitBox);
            _weaponIO.OnFilterHandler = Filter;
            _weaponIO.OnTriggerEnterHandler = OnHitEnemy;

            AttackCollider = _weaponIO.GetComponent<BoxCollider>();
            AttackCollider.enabled = false;

            NavMeshAgent = _prefab.GetComponent<NavMeshAgent>();
            NavMeshAgent.angularSpeed = hellHoundData.Stats.AngularSpeed;
            NavMeshAgent.acceleration = hellHoundData.Stats.Acceleration;
            NavMeshAgent.stoppingDistance = hellHoundData.Stats.StoppingDistance;
            NavMeshAgent.baseOffset = hellHoundData.Stats.BaseOffsetByY;

            _attackStates = Animator.GetBehaviours<HellHoundAttackStateBehaviour>();
            for (int i = 0; i < _attackStates.Length; i++)
            {
                _attackStates[i].OnStateEnterHandler += OnAttackStateEnter;
                _attackStates[i].OnStateExitHandler += OnAttackStateExit;
            }

            List <InteractableObjectBehavior> hitBoxesIO = _interactableObjects.GetInteractableObjectsByType(InteractableObjectType.Enemy);
            for (int i = 0; i < hitBoxesIO.Count; i++)
            {
                if (hitBoxesIO[i].gameObject.TryGetComponent(out CapsuleCollider collider) && collider.enabled)
                {
                    hitBoxesIO[i].SetTakeDamageEvent(OnTakeDamage);
                }
            }

            CurrentHealth = _hellHoundData.BaseStats.MainStats.MaxHealth;
            IsDead = false;
        }

        #endregion


        #region Methods

        private bool Filter(Collider collider) => _hellHoundData.Filter(collider);
        private void OnDetectionEnemy(ITrigger trigger, Collider collider) => _hellHoundData.OnDetectionEnemy(collider, this);
        private void OnLostEnemy(ITrigger trigger, Collider collider) => _hellHoundData.OnLostEnemy(collider, this);
        private void OnHitEnemy(ITrigger trigger, Collider collider) => _hellHoundData.OnHitEnemy(collider, this);
        private void OnAttackStateEnter() => _hellHoundData.OnAttackStateEnter(this);
        private void OnAttackStateExit() => _hellHoundData.OnAttackStateExit(this);
        private void OnTakeDamage(int id, Damage damage) => TakeDamage(damage);

        public void Clean()
        {
            _isCleared = true;

            Object.Destroy(NavMeshAgent);
            Object.Destroy(AttackCollider);
            Object.Destroy(_prefab.GetComponentInChildren<Rigidbody>());
            Object.Destroy(_detectionSphere.gameObject);

            for (int i = 0; i < _interactableObjects.Length; i++)
            {
                Object.Destroy(_interactableObjects[i]);
            }
            _interactableObjects = null;

            ChasingTarget = null;

            for (int i = 0; i < _attackStates.Length; i++)
            {
                _attackStates[i].OnStateEnterHandler -= OnAttackStateEnter;
                _attackStates[i].OnStateExitHandler -= OnAttackStateExit;
            }
        }

        #endregion


        #region EnemyModel

        public override void Execute()
        {
            if (!IsDead)
            {
                _hellHoundData.Act(this);
            }
            else if (!_isCleared)
            {
                Clean();
            }
        }

        public override EnemyStats GetStats()
        {
            return _hellHoundData.BaseStats;
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
                _hellHoundData.TakeDamage(this, damage);
            }
        }

        #endregion

    }
}
