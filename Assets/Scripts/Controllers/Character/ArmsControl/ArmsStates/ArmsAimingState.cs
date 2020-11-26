using UnityEngine;


namespace BeastHunter
{
    public class ArmsAimingState : ArmsBaseState
    {
        #region ClassLifeCycle

        public ArmsAimingState(GameContext context, 
            CharacterArmsStateMachine stateMachine) : base(context, stateMachine)
        {
            IsTargeting = true;
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

