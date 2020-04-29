using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterAnimationsController
    {
        #region Properties

        public Animator CharacterAnimator { get; }

        #endregion


        #region ClassLifeCycles

        public CharacterAnimationsController(Animator characterAnimator)
        {
            CharacterAnimator = characterAnimator;
        }

        #endregion


        #region Methods

        public void UpdateAnimation(bool isGrounded, bool isRunning, bool isInBattleMode, bool isMovingForward, 
            bool isTurningRight, bool isTurningLeft)
        {
            CharacterAnimator.SetBool("IsGrounded", isGrounded);
            CharacterAnimator.SetBool("IsRunning", isRunning);
            CharacterAnimator.SetBool("IsInBattleMode", isInBattleMode);
            CharacterAnimator.SetBool("IsMovingForward", isMovingForward);
            CharacterAnimator.SetBool("IsTurningRight", isTurningRight);
            CharacterAnimator.SetBool("IsTurningLeft", isTurningLeft);
        }

        public void SetAnimationsSpeed(float speed)
        {
            CharacterAnimator.speed = speed;
        }

        public void SetAnimationsBaseSpeed()
        {
            CharacterAnimator.speed = 1;
        }

        #endregion
    }
}

