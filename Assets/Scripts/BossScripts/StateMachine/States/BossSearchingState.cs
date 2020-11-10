using UnityEngine;


namespace BeastHunter
{
    public class BossSearchingState : BossBaseState
    {
        #region Constants

        private const float SEARCHING_TIME = 5f;

        #endregion


        #region Fields

        private float _searchingTime;

        #endregion


        #region ClassLifeCycle

        public BossSearchingState(BossStateMachine stateMachine) : base(stateMachine)
        {
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
            _stateMachine._model.BossNavAgent.SetDestination(_stateMachine._model.BossTransform.position);
            _stateMachine._model.BossNavAgent.speed = 0f;
            _stateMachine._model.BossAnimator.Play("SearchingState", 0, 0f);
            _searchingTime = SEARCHING_TIME;
        }

        public override void Execute()
        {
            CheckSearchingTIme();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        private void CheckSearchingTIme()
        {
            if(_searchingTime > 0)
            {
                _searchingTime -= Time.deltaTime;

                if(_searchingTime <= 0)
                {
                    _stateMachine.SetCurrentStateOverride(BossStatesEnum.Patroling);
                }
            }
        }

        #endregion
    }
}

