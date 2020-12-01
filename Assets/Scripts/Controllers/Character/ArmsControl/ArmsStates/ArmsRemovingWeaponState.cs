using UnityEngine;


namespace BeastHunter
{
    public class ArmsRemovingWeaponState : ArmsBaseState
    {
        #region ClassLifeCycle

        public ArmsRemovingWeaponState(GameContext context,
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
