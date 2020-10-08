using UnityEngine;


namespace BeastHunter
{
    public class TrapPlacingState : CharacterBaseState, IUpdate
    {
        #region Constants

        private const float TRAP_PLACING_TIME = 3f;
        private const float TIME_TO_SET_TRAP = 1.5f;

        #endregion


        #region Fields

        private readonly GameContext _context;
        private float _exitTIme;
        private bool _isTrapSet;

        #endregion


        #region ClassLifeCycle

        public TrapPlacingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            _context = context;
            IsTargeting = false;
            IsAttacking = false;
            CheckTimes();
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            CountTimers();
        }

        #endregion


        #region Methods

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize();
            _animationController.PlayTrapPlacingAnimation();
            _exitTIme = TRAP_PLACING_TIME;
            _isTrapSet = false;
        }

        private void CheckTimes()
        {
            if(TIME_TO_SET_TRAP > TRAP_PLACING_TIME)
            {
                throw new System.Exception("Time to set trap must be less then trap placing time!");
            }
        }

        private void CountTimers()
        {
            _exitTIme -= Time.deltaTime;

            if (_exitTIme <= 0)
            {
                _stateMachine.ReturnState();
            }
            else if (_exitTIme < TIME_TO_SET_TRAP && !_isTrapSet)
            {
                SetTrap(0);
            }
        }

        private void SetTrap(int trapNumber)
        {
            switch (trapNumber)
            {
                case 0:
                    new InitializeTrapController(_context, Data.TrapData);
                    _isTrapSet = true;
                    break;
                case 1:
                    new InitializeTrapController(_context, Data.TrapData2);
                    _isTrapSet = true;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}

