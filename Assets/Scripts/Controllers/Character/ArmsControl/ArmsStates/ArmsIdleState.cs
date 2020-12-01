using UnityEngine;


namespace BeastHunter
{
    public class ArmsIdleState : ArmsBaseState
    {
        #region ClassLifeCycle

        public ArmsIdleState(GameContext context, 
            CharacterArmsStateMachine stateMachine) : base(context, stateMachine)
        {
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region Methods

        public override bool CanBeActivated()
        {
            return !IsActive;
        }

        #endregion
    }
}
