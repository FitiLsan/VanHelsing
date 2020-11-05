using UnityEngine;


namespace BeastHunter
{
    public sealed class BossIdleState : BossBaseState
    {
        #region Constants

        private const float MINIMAL_IDLE_TIME = 5f;
        private const float MAXIMAL_IDLE_TIME = 10f;

        #endregion


        #region Fields

        private float _timeToIdle;

        #endregion


        #region ClassLifeCycle

        public BossIdleState(BossStateMachine stateMachine) : base (stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
        }

        public override void Initialise()
        {
            CanExit = true;
            CanBeOverriden = true;

            _timeToIdle = Random.Range(MINIMAL_IDLE_TIME, MAXIMAL_IDLE_TIME);

            _stateMachine._model.BossAnimator.Play("IdleState", 0, 0f);
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

                _stateMachine._model.BossData.Act(_stateMachine._model.BossData.BossStats.IdlePattern, _stateMachine._model);

                if (_timeToIdle <= 0)
                {
                    _stateMachine.SetCurrentState(BossStatesEnum.Patroling);
                }
            }
        }

        #endregion
    }
}

