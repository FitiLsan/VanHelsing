using UnityEngine;
using UnityEngine.AI;
using Extensions;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class HellHoundModel : EnemyModel
    {
        #region Fields

        private HellHoundData _hellHoundData;
        private InteractableObjectBehavior[] _interactableObjects;
        private SphereCollider _detectionSphere;
        private HellHoundAttackStateBehaviour[] _attackStates;

        public float Timer;
        public float RotatePosition1;
        public float RotatePosition2;

        #endregion


        #region Properties

        public Animator Animator { get; }
        public GameObject HellHound { get; }
        public NavMeshAgent NavMeshAgent { get; }
        public Transform Transform { get; }
        public Collider AttackCollider { get; }
        public InteractableObjectBehavior WeaponIO { get; }
        public HellHoundData.BehaviourState BehaviourState { get; set; }
        public Vector3 SpawnPoint { get; }
        public Transform ChasingTarget { get; set; }
        public float JumpingAttackTimer { get; set; }
        public bool IsAttacking { get; set; }

        #endregion


        #region ClassLifeCycle

        public HellHoundModel(GameObject objectOnScene, HellHoundData data) : base(objectOnScene, data)
        {
            _hellHoundData = data;
            HellHound = objectOnScene;

            Transform = HellHound.transform;
            BehaviourState = HellHoundData.BehaviourState.None;

            SpawnPoint = Transform.position;

            Animator = HellHound.GetComponent<Animator>();
            Animator.SetFloat("JumpSpeedRate", _hellHoundData.Stats.JumpingSpeedRate);
            Animator.SetFloat("JumpBackSpeedRate", _hellHoundData.Stats.BackJumpAnimationSpeedRate);
            Animator.SetFloat("JumpBackIntensity", _hellHoundData.Stats.BackJumpAnimationIntensity);

            _interactableObjects = HellHound.GetComponentsInChildren<InteractableObjectBehavior>();

            InteractableObjectBehavior _detectionSphereIO = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.Sphere);
            _detectionSphereIO.OnFilterHandler = Filter;
            _detectionSphereIO.OnTriggerEnterHandler = OnDetectionEnemy;
            _detectionSphereIO.OnTriggerExitHandler = OnLostEnemy;

            _detectionSphere = _detectionSphereIO.GetComponent<SphereCollider>();
            if (_detectionSphere == null) Debug.LogError(this + " not found SphereCollider in DetectionSphere gameobject");
            else _detectionSphere.radius = _hellHoundData.Stats.DetectionRadius;

            WeaponIO = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.HitBox);
            WeaponIO.OnFilterHandler = Filter;
            WeaponIO.OnTriggerEnterHandler = OnHitEnemy;

            AttackCollider = WeaponIO.GetComponent<BoxCollider>();
            AttackCollider.enabled = false;

            NavMeshAgent = HellHound.GetComponent<NavMeshAgent>();
            NavMeshAgent.angularSpeed = _hellHoundData.Stats.AngularSpeed;
            NavMeshAgent.acceleration = _hellHoundData.Stats.Acceleration;
            NavMeshAgent.stoppingDistance = _hellHoundData.Stats.StoppingDistance;
            NavMeshAgent.baseOffset = _hellHoundData.Stats.BaseOffsetByY;

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
            NavMeshAgent.enabled = false;
            Object.Destroy(_detectionSphere);
            Object.Destroy(AttackCollider);
            ChasingTarget = null;

            for (int i = 0; i < _attackStates.Length; i++)
            {
                _attackStates[i].OnStateEnterHandler -= OnAttackStateEnter;
                _attackStates[i].OnStateExitHandler -= OnAttackStateExit;
            }

            for (int i = 0; i < _interactableObjects.Length; i++)
            {
                Object.Destroy(_interactableObjects[i]);
            }
            _interactableObjects = null;
        }

        public override void TakeDamage(Damage damage)
        {
            if (!CurrentStats.BaseStats.IsDead)
            {
                _hellHoundData.TakeDamage(this, damage);
            }
        }

        #endregion
    }
}
