using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    public class BossPatrolingState : BossBaseState
    {
        #region Constants

        private const float MINIMAL_TARGET_DISTANCE = 5f;
        private const float MAXIMAL_TARGET_DISTANCE = 10f;
        private const float MAXIMAL_PATH_DISTANCE = 30f;
        private const float MINIMAL_DISTANCE_TO_TARGET = 0.3f;
        private const float PROBABILITY_TO_CONTINUE_PATROLING = 0.5f;
        private const float PROBABILITY_TO_START_SEARCHING = 0.5f;
        private const float MAXIMAL_DISTANCE_FROM_ANCHOR = 30f;
        private const float STUCK_TIME_COUNT = 5f;

        #endregion


        #region Fields

        private Vector3 _target;
        private Vector3 _currentPosition;
        private Vector3 _checkTargetPosition;

        private NavMeshPath _path;

        private bool _isTargetSet;

        private float _stuckTIme;

        #endregion


        #region ClassLifeCycle

        public BossPatrolingState(BossStateMachine stateMachine) : base(stateMachine)
        {
            _path = new NavMeshPath();
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = true;
            _isTargetSet = false;
            _stuckTIme = STUCK_TIME_COUNT;
            _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData.BossSettings.WalkSpeed;
            _stateMachine._model.BossAnimator.Play("MovingState");           
        }

        public override void Execute()
        {
            CheckTarget();
            CheckStuck();
            CheckIfFarFromAnchor();
            CheckIfNearTarget();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        private void CheckStuck()
        {
            if (_isTargetSet && _stateMachine._model.CurrentSpeed == 0)
            {
                if(_stuckTIme > 0)
                {
                    _stuckTIme -= Time.deltaTime;

                    if(_stuckTIme <= 0)
                    {
                        _isTargetSet = false;
                        _stuckTIme = STUCK_TIME_COUNT;
                    }
                }
            }
        }

        private void CheckTarget()
        {
            if (!_isTargetSet)
            {
                _currentPosition = _stateMachine._model.BossTransform.position;
                _checkTargetPosition = _currentPosition;

                _checkTargetPosition = Services.SharedInstance.PhysicsService.GetGroundedPosition(
                    new Vector3(
                        (Random.Range(0, 2) * 2 - 1) * Random.Range(MINIMAL_TARGET_DISTANCE, MAXIMAL_TARGET_DISTANCE)
                            + _currentPosition.x,
                                _currentPosition.y,
                                    (Random.Range(0, 2) * 2 - 1) *
                                        Random.Range(MINIMAL_TARGET_DISTANCE, MAXIMAL_TARGET_DISTANCE)
                                            + _currentPosition.z));

                _stateMachine._model.BossNavAgent.CalculatePath(_checkTargetPosition, _path);

                if (_path.status == NavMeshPathStatus.PathComplete)
                {
                    _target = _checkTargetPosition;
                    _stateMachine._model.BossNavAgent.SetDestination(_target);
                    _isTargetSet = true;
                }
            }
        }

        private void CheckIfFarFromAnchor()
        {
            if (_isTargetSet && Mathf.Sqrt((_stateMachine._model.BossTransform.position - 
                _stateMachine._model.AnchorPosition).sqrMagnitude) >= MAXIMAL_DISTANCE_FROM_ANCHOR)
            {
                _stateMachine._model.BossNavAgent.SetDestination(_target);
            }
        }

        private void CheckIfNearTarget()
        {
            if (_isTargetSet &&  Mathf.Sqrt((_stateMachine._model.BossTransform.position - _target)
                .sqrMagnitude) <= MINIMAL_DISTANCE_TO_TARGET)              
            {
                if (Random.Range(0f, 1f) > PROBABILITY_TO_CONTINUE_PATROLING)
                {
                    if(Random.Range(0f, 1f) < PROBABILITY_TO_START_SEARCHING)
                    {
                        _stateMachine.SetCurrentStateOverride(BossStatesEnum.Idle);
                    }
                    else
                    {
                        _stateMachine.SetCurrentStateOverride(BossStatesEnum.Searching);
                    }                  
                }
                else
                {
                    _isTargetSet = false;
                }
            }
        }

        #endregion
    }
}

