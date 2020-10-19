using UnityEngine;
using UniRx;


namespace BeastHunter
{
    public class TrapPlacingState : CharacterBaseState, IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        private float _timeToPlaceTrap;
        private float _timeForTrapToAppear;
        private float _exitTIme;
        private bool _isTrapSet;

        #endregion


        #region ClassLifeCycle

        public TrapPlacingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            _context = context;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _characterModel.CurrentPlacingTrapModel.Subscribe(UpdateTimes);
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            CountTimers();
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _characterModel.CurrentPlacingTrapModel.Dispose();
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return _characterModel.CurrentPlacingTrapModel != null;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();
            _animationController.PlayTrapPlacingAnimation();
            _exitTIme = _timeToPlaceTrap;
            _isTrapSet = false;
        }

        private void CountTimers()
        {
            _exitTIme -= Time.deltaTime;

            if (_exitTIme <= 0)
            {
                _stateMachine.ReturnState();
            }
            else if (_exitTIme < _timeForTrapToAppear && !_isTrapSet)
            {
                SetTrap();
            }
        }

        private void SetTrap()
        {
            _stateMachine.BackState.OnTrapPlace?.Invoke();
            _isTrapSet = true;
        }

        private void UpdateTimes(TrapModel currentTrapModel)
        {
            if(currentTrapModel != null)
            {
                _timeToPlaceTrap = currentTrapModel.TrapStruct.TotalTimeToPlaceTrap;
                _timeForTrapToAppear = currentTrapModel.TrapStruct.TimeBeforeTrapAppear;
            }
        }

        #endregion
    }
}

