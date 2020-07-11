using UnityEngine;


namespace BeastHunter
{
    public class BossMainState : BossBaseState
    {
        #region Constants

        private float SPEED_COUNT_FRAME = 0.15f;

        #endregion


        #region Fields

        private Vector3 _lastPosition;
        private Vector3 _currentPosition;

        private float _speedCountTime;

        #endregion


        #region ClassLifeCycle

        public BossMainState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            _stateMachine._model.BossBehavior.OnFilterHandler += OnFilterHandler;
            _stateMachine._model.BossBehavior.OnTriggerEnterHandler += OnTriggerEnterHandler;
            _stateMachine._model.BossBehavior.OnTriggerExitHandler += OnTriggerExitHandler;
            GlobalEventsModel.OnPlayerDie += OnPlayerDieHandler;
            GlobalEventsModel.OnBossStunned += OnBossStunnedHandler;
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = false;
        }

        public override void Execute()
        {
            if (!_stateMachine._model.IsDead)
            {
                SpeedCheck();
                HealthCheck();
            }          
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
            _stateMachine._model.BossBehavior.OnFilterHandler -= OnFilterHandler;
            _stateMachine._model.BossBehavior.OnTriggerEnterHandler -= OnTriggerEnterHandler;
            _stateMachine._model.BossBehavior.OnTriggerExitHandler -= OnTriggerExitHandler;
            GlobalEventsModel.OnPlayerDie -= OnPlayerDieHandler;
            GlobalEventsModel.OnBossStunned -= OnBossStunnedHandler;
        }

        private void OnBossStunnedHandler()
        {
            if (!_stateMachine._model.IsDead)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Stunned);
            }         
        }

        private void OnPlayerDieHandler()
        {
            if (!_stateMachine._model.IsDead)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Patroling);
            }
        }

        private void SpeedCheck()
        {
            if (_speedCountTime > 0)
            {
                _speedCountTime -= Time.fixedDeltaTime;
            }
            else
            {
                _speedCountTime = SPEED_COUNT_FRAME;
                _currentPosition = _stateMachine._model.BossTransform.position;
                _stateMachine._model.CurrentSpeed = Mathf.Sqrt((_currentPosition - _lastPosition).sqrMagnitude) /
                    SPEED_COUNT_FRAME;
                _lastPosition = _currentPosition;
            }

            _stateMachine._model.BossAnimator.SetFloat("Speed", _stateMachine._model.CurrentSpeed);
        }

        private void HealthCheck()
        {
            if (_stateMachine._model.CurrentHealth <= 0)
            {
                _stateMachine.SetCurrentStateAnyway(BossStatesEnum.Dead);
            }
        }

        private bool OnFilterHandler(Collider tagObject)
        {
            return !tagObject.isTrigger && tagObject.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger thisdObject, Collider enteredObject)
        {
            if(thisdObject.Type == InteractableObjectType.Enemy)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
            }         
        }

        private void OnTriggerExitHandler(ITrigger thisdObject, Collider exitedObject)
        {
            if (thisdObject.Type == InteractableObjectType.Enemy)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Searching);
            }
        }

        #endregion
    }
}

