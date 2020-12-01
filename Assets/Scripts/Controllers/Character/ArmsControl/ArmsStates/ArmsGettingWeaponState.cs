using UnityEngine;


namespace BeastHunter
{
    public class ArmsGettingWeaponState : ArmsBaseState
    {
        #region ClassLifeCycle

        public ArmsGettingWeaponState(GameContext context, 
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
