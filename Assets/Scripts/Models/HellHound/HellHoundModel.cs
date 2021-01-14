using UnityEngine;
using UnityEngine.AI;
using Extensions;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class HellHoundModel : EnemyModel
    {
        #region Fields

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
        public HellHoundData.BehaviourState BehaviourState { get; set; }
        public Vector3 SpawnPoint { get; }
        public Transform ChasingTarget { get; set; }
        public float JumpingAttackTimer { get; set; }
        public bool IsAttacking { get; set; }

        #endregion


        #region ClassLifeCycle

        public HellHoundModel(GameObject objectOnScene, HellHoundData data) : base(objectOnScene, data)
        {
            Transform = ObjectOnScene.transform;
            BehaviourState = HellHoundData.BehaviourState.None;

            SpawnPoint = Transform.position;

            Animator = ObjectOnScene.GetComponent<Animator>();
            Animator.SetFloat("JumpSpeedRate", (ThisEnemyData as HellHoundData).Stats.JumpingSpeedRate);
            Animator.SetFloat("JumpBackSpeedRate", (ThisEnemyData as HellHoundData).Stats.BackJumpAnimationSpeedRate);
            Animator.SetFloat("JumpBackIntensity", (ThisEnemyData as HellHoundData).Stats.BackJumpAnimationIntensity);

            _interactableObjects = ObjectOnScene.GetComponentsInChildren<InteractableObjectBehavior>();

            InteractableObjectBehavior _detectionSphereIO = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.Sphere);
            _detectionSphereIO.OnFilterHandler = Filter;
            _detectionSphereIO.OnTriggerEnterHandler = OnDetectionEnemy;
            _detectionSphereIO.OnTriggerExitHandler = OnLostEnemy;

            _detectionSphere = _detectionSphereIO.GetComponent<SphereCollider>();
            if (_detectionSphere == null) Debug.LogError(this + " not found SphereCollider in DetectionSphere gameobject");
            else _detectionSphere.radius = (ThisEnemyData as HellHoundData).Stats.DetectionRadius;

            _weaponIO = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.HitBox);
            _weaponIO.OnFilterHandler = Filter;
            _weaponIO.OnTriggerEnterHandler = OnHitEnemy;

            AttackCollider = _weaponIO.GetComponent<BoxCollider>();
            AttackCollider.enabled = false;

            NavMeshAgent = ObjectOnScene.GetComponent<NavMeshAgent>();
            NavMeshAgent.angularSpeed = (ThisEnemyData as HellHoundData).Stats.AngularSpeed;
            NavMeshAgent.acceleration = (ThisEnemyData as HellHoundData).Stats.Acceleration;
            NavMeshAgent.stoppingDistance = (ThisEnemyData as HellHoundData).Stats.StoppingDistance;
            NavMeshAgent.baseOffset = (ThisEnemyData as HellHoundData).Stats.BaseOffsetByY;

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

        private bool Filter(Collider collider) => (ThisEnemyData as HellHoundData).Filter(collider);
        private void OnDetectionEnemy(ITrigger trigger, Collider collider) => (ThisEnemyData as HellHoundData).OnDetectionEnemy(collider, this);
        private void OnLostEnemy(ITrigger trigger, Collider collider) => (ThisEnemyData as HellHoundData).OnLostEnemy(collider, this);
        private void OnHitEnemy(ITrigger trigger, Collider collider) => (ThisEnemyData as HellHoundData).OnHitEnemy(collider, this);
        private void OnAttackStateEnter() => (ThisEnemyData as HellHoundData).OnAttackStateEnter(this);
        private void OnAttackStateExit() => (ThisEnemyData as HellHoundData).OnAttackStateExit(this);
        private void OnTakeDamage(int id, Damage damage) => TakeDamage(damage);

        public void Clean()
        {
            _isCleared = true;

            Object.Destroy(NavMeshAgent);
            Object.Destroy(AttackCollider);
            Object.Destroy(ObjectOnScene.GetComponentInChildren<Rigidbody>());
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

        public override void TakeDamage(Damage damage)
        {
            if (!CurrentStats.BaseStats.IsDead)
            {
                (ThisEnemyData as HellHoundData).TakeDamage(this, damage);
            }
        }

        #endregion

    }
}
