﻿namespace BeastHunter
{
    public sealed class DeadState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public DeadState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            _animationController.PlayDeadAnimation();
        }

        public override void Execute()
        {

        }

        public override void OnExit()
        {

        }

        public override void OnTearDown()
        {
        }

        #endregion
    }
}

