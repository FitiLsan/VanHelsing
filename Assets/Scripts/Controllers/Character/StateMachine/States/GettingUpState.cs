using UnityEngine;


namespace BeastHunter
{
    public sealed class GettingUpState : CharacterBaseState, IAwake, IUpdate, ITearDown
    {
        #region Constants

        private const float REGAIN_BALANCE_TIME = 0.5f;

        #endregion


        #region Fields

        private bool _isRegainedBalace;
        private float _regainBalanceTime;

        #endregion


        #region ClassLifeCycle

        public GettingUpState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.GettingUp;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _characterModel.BehaviorPuppet.onRegainBalance.unityEvent.AddListener(RegainBalance);
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            CountRegainBalanceTime();
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _characterModel.BehaviorPuppet.onRegainBalance.unityEvent.RemoveListener(RegainBalance);
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return !_characterModel.CurrentStats.BaseStats.IsDead;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize(previousState);
            _isRegainedBalace = false;
            _regainBalanceTime = REGAIN_BALANCE_TIME;

            if (!_characterModel.CharacterStartStats.BaseStats.IsDead)
            {
                _characterModel.CharacterRigitbody.isKinematic = false;
            }          
        }

        private void RegainBalance()
        {
            _isRegainedBalace = true;
        }

        private void CountRegainBalanceTime()
        {
            if (_isRegainedBalace)
            {
                _regainBalanceTime -= Time.deltaTime;

                if (_regainBalanceTime <= 0f)
                {
                    _characterModel.CharacterRigitbody.constraints = RigidbodyConstraints.FreezeRotation;

                    if (_inputModel.IsInputMove)
                    {
                        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
                    }
                    else
                    {
                        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Idle]);
                    }               
                }
            }
        }

        #endregion
    }
}

