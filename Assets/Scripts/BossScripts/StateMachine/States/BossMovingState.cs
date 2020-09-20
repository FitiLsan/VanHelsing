using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BeastHunter
{
    public class BossMovingState : BossBaseState
    {
        #region Constants

        private const float MINIMAL_DISTANCE_TO_TARGET = 0.3f;
        private const float STUCK_TIME_COUNT = 5f;

        #endregion


        #region Fields

        private readonly NavMeshPath _navMeshPath;
        private readonly MovementPoint[] _movementPoints;
        private readonly bool _movementLoop;
        private readonly IEnumerator<MovementPoint> _pointsEnumerator;
        private MovementPoint _currentPoint;
        private string _lastAnimationState;
        private Vector3 _target;
        private Vector3 _checkTargetPosition;
        private bool _isTargetSet;
        private bool _isWaiting;
        private float _stuckTime;
        private float _waitingTime;

        #endregion


        #region Properties

        private IEnumerable<MovementPoint> Points
        {
            get
            {
                do
                {
                    foreach (var point in _movementPoints)
                    {
                        yield return point;
                    }
                } while (_movementLoop);
            }
        }

        #endregion


        #region ClassLifeCycle

        public BossMovingState(BossStateMachine stateMachine) : base(stateMachine)
        {
            _movementPoints = stateMachine._model.MovementPoints;
            _movementLoop = stateMachine._model.MovementLoop;
            _pointsEnumerator = Points.GetEnumerator();
            _navMeshPath = new NavMeshPath();
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
            _isWaiting = false;
            _stuckTime = STUCK_TIME_COUNT;
            _waitingTime = 0f;
            _lastAnimationState = MovementStep.DEFAULT_ANIMATION_STATE;
            _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData._bossSettings.WalkSpeed;
            _stateMachine._model.BossAnimator.Play(_lastAnimationState);
        }

        public override void Execute()
        {
            CheckTarget();
            CheckStuck();
            CheckIfNearTarget();
        }

        public override void OnExit()
        {
            _stateMachine._model.BossNavAgent.autoBraking = true;
        }

        public override void OnTearDown()
        {
        }

        private void CheckTarget()
        {
            //Services.SharedInstance.UnityTimeService
            
            
            if (_isWaiting)
            {
                _waitingTime -= Time.deltaTime;

                if(_waitingTime <= 0)
                {
                    _isWaiting = false;
                    _waitingTime = 0f;
                }
            }
            
            if (!_isTargetSet && !_isWaiting)
            {
                _currentPoint = GetNextPoint();

                if (_currentPoint.Equals(null))
                {
                    _stateMachine.SetCurrentStateOverride(BossStatesEnum.Idle);
                }
                else
                {
                    _checkTargetPosition = _currentPoint.Position;
                    _waitingTime = _currentPoint.WaitingTime;

                    _stateMachine._model.BossNavAgent.autoBraking = _waitingTime > 0;
                    _stateMachine._model.BossNavAgent.CalculatePath(_checkTargetPosition, _navMeshPath);

                    if (_waitingTime == 0)
                    {
                        _lastAnimationState = _currentPoint.AnimationState;
                        _stateMachine._model.BossAnimator.Play(_lastAnimationState);
                    }
                
                    if (_navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        _target = _checkTargetPosition;
                        _stateMachine._model.BossNavAgent.SetDestination(_target);
                        _isTargetSet = true;
                    }
                }
            }
        }
        
        private void CheckStuck()
        {
            if (_isTargetSet && !_isWaiting && _stateMachine._model.CurrentSpeed == 0)
            {
                if(_stuckTime > 0)
                {
                    _stuckTime -= Time.deltaTime;

                    if(_stuckTime <= 0)
                    {
                        _isTargetSet = false;
                        _stuckTime = STUCK_TIME_COUNT;
                    }
                }
            }
        }
        
        private void CheckIfNearTarget()
        {
            if (_isTargetSet && !_isWaiting &&  Mathf.Sqrt((_stateMachine._model.BossTransform.position - _target)
                .sqrMagnitude) <= MINIMAL_DISTANCE_TO_TARGET)              
            {
                _isTargetSet = false;
                _isWaiting = _waitingTime > 0;
                
                if (_lastAnimationState != _currentPoint.AnimationState)
                {
                    _lastAnimationState = _currentPoint.AnimationState;
                    _stateMachine._model.BossAnimator.Play(_lastAnimationState);
                }
            }
        }

        private MovementPoint GetNextPoint()
        {
            return _pointsEnumerator.MoveNext()
                ? _pointsEnumerator.Current
                : null;
        }

        #endregion
    }
}