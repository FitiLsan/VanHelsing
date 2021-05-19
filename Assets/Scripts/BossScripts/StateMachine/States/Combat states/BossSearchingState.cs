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
        private bool _isSearching;

        #endregion


        #region ClassLifeCycle

        public BossSearchingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            IsBattleState = true;
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = true;
            _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
            _stateMachine._model.BossAnimator.Play("SearchingState", 0, 0f);
            _searchingTime = SEARCHING_TIME;
        }

        public override void Execute()
        {
            CheckSearchingTime();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        private void CheckSearchingTime()
        {
            if(_searchingTime > 0)
            {
                _isSearching = true;
                _searchingTime -= Time.deltaTime;

                if(_searchingTime <= 0)
                {
                    _isSearching = false;
                    _stateMachine.SetCurrentStateOverride(BossStatesEnum.Patroling);
                }
            }
        }

        private void Searching()
        {

        }

        #endregion
    }
}

