using System;
using UnityEngine;

namespace BeastHunter
{
    public sealed class ControlTransferringState : CharacterBaseState, IAwake, ITearDown, IUpdate
    {
        #region ClassLifeCycle

        public ControlTransferringState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            StateName = CharacterStatesEnum.ControlTransferring;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
           // _characterModel.BehaviorPuppet.OnPostActivate += () => _stateMachine.
            //    SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingUp]);
        }


        #endregion


        #region ITearDown

        public void TearDown()
        {
          //  _characterModel.BehaviorPuppet.OnPostActivate -= () => _stateMachine.
            //   SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingUp]);
        }

        #endregion


        #region Methods

        protected override void EnableActions()
        {
            base.EnableActions();
            _characterModel.StartControl();
          
        }

        protected override void DisableActions()
        {
            _characterModel.StopControl();
            _inputModel.OnAim = null;
            _inputModel.OnAttack = null;
            _inputModel.OnRunStart = null;
            _inputModel.OnRunStop = null;
            _inputModel.OnJump = null;
            base.DisableActions();
        }

        public override bool CanBeActivated()
        {
            _stateMachine.BackState.OnEnemyHealthBar(false);
            if (!_characterModel.IsGrounded)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.MidAir]);
            }
            return !_characterModel.CurrentStats.BaseStats.IsDead && _characterModel.IsGrounded;
        }

        public override void Initialize(CharacterBaseState previousState = null)
        {
            base.Initialize(previousState);

            _characterModel.CurrentSpeed = 1;
            //_characterModel.CharacterRigitbody.constraints = UnityEngine.RigidbodyConstraints.FreezePositionX |
            //     UnityEngine.RigidbodyConstraints.FreezePositionZ | UnityEngine.RigidbodyConstraints.FreezeRotation;
        }

        public void Updating()
        {
            _stateMachine.BackState.CountSpeed();
            ControlMovement();
        }


        private void ControlMovement()
        {
            _stateMachine.BackState.RotateCharacter(false, true);
            _stateMachine.BackState.MoveCharacter(true, true);
        }

        #endregion
    }
}
