using UnityEngine;


namespace BeastHunter
{
    public sealed class BossIdleState : BossBaseState
    {
        #region Constants

        private const float MINIMAL_IDLE_TIME = 5f;
        private const float MAXIMAL_IDLE_TIME = 10f;
        private const float PROBABILITY_TO_CHECK_HUNGER = 0.9f;

        #endregion


        #region Fields

        private float _timeToIdle;
        private bool _isHunger;

        #endregion


        #region ClassLifeCycle

        public BossIdleState(BossStateMachine stateMachine) : base (stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            IsBattleState = false;
        }

        public override void Initialise()
        {
            CanExit = true;
            CanBeOverriden = true;

            _timeToIdle = Random.Range(MINIMAL_IDLE_TIME, MAXIMAL_IDLE_TIME);

            _stateMachine._model.BossAnimator.Play("IdleState", 0, 0f);
            _isHunger = _stateMachine._mainState.IsHunger;
        }

        public override void Execute()
        {
            CountIdleTime();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        private void CountIdleTime()
        {
            if(_timeToIdle > 0)
            {
                _timeToIdle -= Time.deltaTime;

                if(_timeToIdle <= 2f)
                {
                    if (Random.Range(0f, 1f) < PROBABILITY_TO_CHECK_HUNGER)
                    {
                        HungerCheck();
                    }
                }
                if(_timeToIdle <= 0)
                {
                    _stateMachine.SetCurrentState(BossStatesEnum.Patroling);
                }
            }
        }

        private void HungerCheck()
        {
            if (_isHunger & !_stateMachine.CurrentState.IsBattleState)
            {
                if (_stateMachine.CurrentState != _stateMachine.States[BossStatesEnum.Eating])
                {
                    _stateMachine.SetCurrentStateOverride(BossStatesEnum.Eating);
                }
            }
        }

        private void StaminaCheck()
        {

        }

        private void RandomCheck()
        {
            
        }
        #endregion
    }
}

