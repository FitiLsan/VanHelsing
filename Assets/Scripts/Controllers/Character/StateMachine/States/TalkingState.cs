using UnityEngine;


namespace BeastHunter
{
    public sealed class TalkingState : CharacterBaseState
    {
        #region Fields

        private Collider _npcTargetCollider;

        #endregion


        #region ClassLifeCycle

        public TalkingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.NotActive;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();
            _animationController.PlayIdleAnimation();
        }

        #endregion
    }
}

