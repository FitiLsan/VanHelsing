using UnityEngine;
using UnityEngine.AI;
using Extensions;


namespace BeastHunter
{
    public sealed class HellHoundModel : EnemyModel
    {
        #region Fields

        private HellHoundData _hellHoundData;
        private InteractableObjectBehavior[] _interactableObjects;
        private InteractableObjectBehavior _detectionSphereIO;
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
        public Rigidbody Rigidbody { get; }
        public Transform Transform { get; }
        public Collider AttackCollider { get; }
        public InteractableObjectBehavior WeaponIO { get; }
        public float JumpingAttackTimer { get; set; }
        public HellHoundData.BehaviourState BehaviourState { get; set; }
        public Vector3 SpawnPoint { get; }
        public Transform ChasingTarget { get; set; }
        public bool IsAttacking { get; set; }

        #endregion


        #region ClassLifeCycle

        public HellHoundModel(GameObject gameObject, HellHoundData hellHoundData)
        {
            _hellHoundData = hellHoundData;
            HellHound = gameObject;

            Transform = HellHound.transform;
            BehaviourState = HellHoundData.BehaviourState.None;

            Rigidbody = HellHound.GetComponent<Rigidbody>();
            SpawnPoint = Rigidbody.position;

            Animator = HellHound.GetComponent<Animator>();
            Animator.SetFloat("JumpSpeedRate", hellHoundData.Stats.JumpingSpeedRate);
            Animator.SetFloat("JumpBackSpeedRate", hellHoundData.Stats.BackJumpAnimationSpeedRate);
            Animator.SetFloat("JumpBackIntensity", hellHoundData.Stats.BackJumpAnimationIntensity);

            _interactableObjects = HellHound.GetComponentsInChildren<InteractableObjectBehavior>();

            _detectionSphereIO = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.Sphere);
            _detectionSphereIO.OnFilterHandler = Filter;
            _detectionSphereIO.OnTriggerEnterHandler = OnDetectionEnemy;
            _detectionSphereIO.OnTriggerExitHandler = OnLostEnemy;

            _detectionSphere = _detectionSphereIO.GetComponent<SphereCollider>();
            if (_detectionSphere == null) Debug.LogError(this + " not found SphereCollider in DetectionSphere gameobject");
            else _detectionSphere.radius = hellHoundData.Stats.DetectionRadius;

            WeaponIO = HellHound.GetComponentInChildren<WeaponHitBoxBehavior>();
            WeaponIO.OnFilterHandler = Filter;
            WeaponIO.OnTriggerEnterHandler = OnHitEnemy;

            AttackCollider = WeaponIO.GetComponent<BoxCollider>();
            AttackCollider.enabled = false;

            NavMeshAgent = HellHound.GetComponent<NavMeshAgent>();
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

        #endregion


        #region EnemyModel

        public override void Execute()
        {
            if (!IsDead)
            {
                _hellHoundData.Act(this);
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
