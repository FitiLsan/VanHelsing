using UnityEngine;
using Interfaces;
using UnityEngine.AI;


namespace Models
{
    [CreateAssetMenu(menuName = "GuardianModel", fileName = "NewGuardianModel")]
    public class NewGuardianModel : NewNPCModel, IDamageable, ISetDamage
    {
        #region PrivateData

        [Tooltip("If this guatdian is melee")]
        [SerializeField] private bool _isMelee;

        [Tooltip("If this guardian has shield")]
        [SerializeField] private bool _hasShield;

        [Tooltip("Distance to atack")]
        [SerializeField] private float _distanceToAttack;

        [Tooltip("Amount of base health points")]
        [SerializeField] private float _baseHealthPoints;

        [Tooltip("Base speed of health regeneration")]
        [SerializeField] private float _baseHealthRegenSpeed;

        [Tooltip("Amount of base stamina points")]
        [SerializeField] private float _baseStaminaPoints;

        [Tooltip("Base speed of stamina regeneration")]
        [SerializeField] private float _baseStaminaRegenSpeed;

        [Tooltip("Base damage amount")]
        [SerializeField] private float _baseDamage;

        [Tooltip("Time between attacks")]
        [SerializeField] private float _timeBetweenAttacks;

        [Tooltip("Chance to start conversation with another npc")]
        [SerializeField] private float _chanceToChatWithOthers;

        [Tooltip("Point to where this guardian is attached")]
        [SerializeField] private NPCPoint _anchorPoint;

        [Tooltip("Maximal distance that guardian can be far from the anchor point")]
        [SerializeField] private float _maxDistanceToAnchorPoint;

        private NPCPoint _targetPoint;
        private GameObject _targetEnemy;
        private Vector3 _currentPosition;
        private bool _isChasing;
        private bool _isDay;
        private float _timeOnPoint;
        private NavMeshAgent _navMeshAgent;
        private GameObject _prefabOnScene;

        #endregion


        #region Fields


        #endregion


        #region Properties

        public bool IsMelee => _isMelee;
        public bool HasShield => _hasShield;

        public float DistanceToAttack => _distanceToAttack;
        public float BaseHealthPoints => _baseHealthPoints;
        public float BaseHealthRegenSpeed => _baseHealthRegenSpeed;
        public float BaseStaminaPoints => _baseStaminaPoints;
        public float BaseStaminaRegenSpeed => _baseStaminaRegenSpeed;
        public float BaseDamage => _baseDamage;
        public float TimeBetweenAttacks => _timeBetweenAttacks;
        public float ChanceToChatWIthOthers => _chanceToChatWithOthers;
        public float MaxDistanceToAnchorPoint => _maxDistanceToAnchorPoint;

        public NPCPoint AnchorPoint => _anchorPoint;

        #endregion


        #region Methods

        public override void OnAwake()
        {
            
        }

        public override void Tick()
        {
            UpdatePosition();
            MakeRoutineActions();
        }

        public override void Init(GameObject prefab)
        {
            _prefabOnScene = prefab;
            _navMeshAgent = _prefabOnScene.GetComponent<NavMeshAgent>();
            _navMeshAgent.Warp(NPCPosition);
            _isDay = true;
            _isOnPoint = false;

            if (_isDay && !IsEscaping)
            {
                _targetPoint = _dayNavigationPoints[CurrentPointIndex];
            }
            else if (!_isDay && !IsEscaping)
            {
                _targetPoint = _nightNavigationPoints[CurrentPointIndex];
            }
            else
            {
                _targetPoint = _safePlacePoint;
            }

            _navMeshAgent.SetDestination(_targetPoint.PointPosition);
        }

        public override void OnTriggerEnter(Collider other)
        {

        }

        public override void OnTriggerStay(Collider other)
        {

        }

        public override void OnTriggerExit(Collider other)
        {

        }

        public override void OnDayBegin()
        {
            _isDay = true;
        }

        public override void OnNightBegin()
        {
            _isDay = false;
        }

        public override void OnDangerAppear()
        {
            _isEscaping = true;
            _isRunning = true;
        }

        public override void OnDangerEnd()
        {
            _isEscaping = false;
            _isRunning = false;
        }

        public void TakeDamage(float damage)
        {
            _baseHealthPoints -= damage;
        }

        public void ApplyDamage(float damage)
        {
            
        }

        private bool Distance()
        {
            float distance;

            if (_isChasing)
            {
                distance = Mathf.Sqrt(
                    Mathf.Pow(_currentPosition.x - _targetEnemy.GetComponent<Transform>().position.x, 2) +
                    Mathf.Pow(_currentPosition.y - _targetEnemy.GetComponent<Transform>().position.y, 2) +
                    Mathf.Pow(_currentPosition.z - _targetEnemy.GetComponent<Transform>().position.z, 2));

                if (distance > DistanceToAttack) return false;
                return true;
            }

            distance = Mathf.Sqrt(Mathf.Pow(_currentPosition.x - _targetPoint.PointPosition.x, 2) +
                                  Mathf.Pow(_currentPosition.y - _targetPoint.PointPosition.y, 2) +
                                  Mathf.Pow(_currentPosition.z - _targetPoint.PointPosition.z, 2));

            if (distance > _targetPoint.DistanceToActivate) return false;
            return true;
        }

        private void UpdatePosition()
        {
            _currentPosition = _prefabOnScene.transform.position;
        }

        private void MakeRoutineActions()
        {
            if (_isOnPoint)
            {
                WaitOnPoint();
            }
            else
            {
                if (IsEscaping) return;
                CheckDistance();
            }
        }

        private void ComeToPoint()
        {
            if (_isOnPoint) return;

            _isOnPoint = true;
            _timeOnPoint = _targetPoint.TimeOnPoint;
        }

        private void WaitOnPoint()
        {
            _timeOnPoint -= Time.deltaTime;

            if (_timeOnPoint <= 0)
            {
                ChangeToNextPoint();
            }
        }

        private void CheckDistance()
        {
            if (_isOnPoint || !Distance()) return;
            ComeToPoint();
        }

        /// <summary>
        /// Needed to be removed - checks if the path can be calculated
        /// </summary>
        private void IsCurrentPointReachable()
        {
            NavMeshPath path = new NavMeshPath();
            _navMeshAgent.CalculatePath(_targetPoint.PointPosition, path);
        }

        private void ChangeToNextPoint()
        {
            if (_isDay)
            {
                if (CurrentPointIndex == _dayNavigationPoints.Length - 1)
                {
                    _currentPointIndex = 0;
                }
                else
                {
                    _currentPointIndex++;
                }

                _targetPoint = _dayNavigationPoints[_currentPointIndex];
            }
            else
            {
                if (CurrentPointIndex == _nightNavigationPoints.Length - 1)
                {
                    _currentPointIndex = 0;
                }
                else
                {
                    _currentPointIndex++;
                }

                _targetPoint = _nightNavigationPoints[_currentPointIndex];
            }

            _isOnPoint = false;
            _navMeshAgent.SetDestination(_targetPoint.PointPosition);
            IsCurrentPointReachable();
        }

        #endregion

    }
}

