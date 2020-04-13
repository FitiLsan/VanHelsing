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

        public void SetDefaultMovementAnimation(bool isMoving, bool isGrounded, float moveSpeed)
        {
            CharacterAnimator.SetBool("IsMoving", isMoving);
            CharacterAnimator.SetBool("IsGrounded", isGrounded);
            CharacterAnimator.SetFloat("MovementSpeed", moveSpeed);
        }

        public void SetBattleAnimation(float axisY, float axisX, bool isMoving)
        {
            CharacterAnimator.SetFloat("AxisY", axisY);
            CharacterAnimator.SetFloat("AxisX", axisX);
            CharacterAnimator.SetBool("IsMoving", isMoving);
        }

        public void SetJumpingAnimation()
        {
            //TODO
        }

        public void SetFallingAnimation()
        {
            //TODO
        }

        public void SetAnimationsSpeed(float speed)
        {
            CharacterAnimator.speed = speed;
        }

        public void SetAnimationsBaseSpeed()
        {
            CharacterAnimator.speed = 1;
        }

        public void ChangeRuntimeAnimator(RuntimeAnimatorController newController)
        {
            CharacterAnimator.runtimeAnimatorController = newController;
        }

        #endregion
    }
}

