using UnityEngine;


namespace BeastHunter
{
    public sealed class TalkingState : CharacterBaseState
    {
        #region Constants

        #endregion


        #region Fields

        private Collider _npcTargetCollider;

        #endregion


        #region Properties

        #endregion


        #region ClassLifeCycle

        public TalkingState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
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
            _animationController.PlayDefaultIdleAnimation();
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

