using UnityEngine;


namespace BeastHunter
{
    public class ArmsAttackingState : ArmsBaseState
    {
        #region ClassLifeCycle

        public ArmsAttackingState(GameContext context, 
            CharacterArmsStateMachine stateMachine) : base(context, stateMachine)
        {
            IsTargeting = false;
            IsAttacking = true;
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

