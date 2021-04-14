using UnityEngine;


namespace BeastHunter
{
    public sealed class BossIdleState : BossBaseState
    {
        #region Constants

        private const float MINIMAL_IDLE_TIME = 4f;
        private const float MAXIMAL_IDLE_TIME = 7f;
        private const float PROBABILITY_TO_CHECK_HUNGER = 0.7f;
        private const float PROBABILITY_TO_CHECK_STAMINA = 0.5f;

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
            if(_stateMachine.LastStateType!= BossStatesEnum.Patroling)
            {
                _stateMachine._model.BossAnimator.Play("IdleState", 0, 0f);
            }
            _timeToIdle = Random.Range(MINIMAL_IDLE_TIME, MAXIMAL_IDLE_TIME);
            _bossData.SetNavMeshAgentSpeed(_bossModel, _bossModel.BossNavAgent, 0);
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
            }
            if (_timeToIdle <= 0)
            {
                RandomCheck();
            }
        }

        private void HungerCheck()
        {
            if (_isHunger && !_stateMachine.CurrentState.IsBattleState)
            {
                if (_stateMachine.CurrentState != _stateMachine.States[BossStatesEnum.Eating])
                {
                   // _stateMachine.SetCurrentStateOverride(BossStatesEnum.Eating);
                }
            }
        }

        private void StaminaCheck()
        {
            if (_bossModel.CurrentStats.BaseStats.CurrentStaminaPoints >= _bossModel.CurrentStats.BaseStats.
                MaximalStaminaPoints && !_stateMachine.CurrentState.IsBattleState)
            {
               // _stateMachine.SetCurrentStateOverride(BossStatesEnum.Resting);
            }
        }

        private void RandomCheck()
        {
            var randomNum = Random.Range(0f, 1f);

            if (randomNum < PROBABILITY_TO_CHECK_HUNGER)
            {
                HungerCheck();
            }
            if (randomNum < PROBABILITY_TO_CHECK_STAMINA)
            {
                StaminaCheck();
            }
            else
            {
                _stateMachine.SetCurrentState(BossStatesEnum.Patroling);
            }

        }
        #endregion
    }
}

