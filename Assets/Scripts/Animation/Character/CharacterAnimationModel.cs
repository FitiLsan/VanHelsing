using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterAnimationModel
    {
        #region Properties

        public CharacterAnimationData CharacterAnimationData {get;}
        public Animator CharacterAnimator { get; }
        public RuntimeAnimatorController CharacterAnimatorController { get; }

        public int MovementAnimationHash { get; }
        public int StrafingAnimationHash { get; }
        public int SlidingForwardAnimationHash { get; }
        public int LongDodgeAnimationHash { get; }
        public int ShortDodgeAnimationHash { get; }
        public int TrapPlacingAnimationHash { get; }

        public float CrouchLevel { get; set; }
        public float CurrentAnimationTime
        {
            get
            {
                return CharacterAnimator.GetCurrentAnimatorStateInfo(0).length;
            }
        }

        #endregion


        #region ClassLifeCycle

        public CharacterAnimationModel(Animator characterAnimator, 
            RuntimeAnimatorController animatorController, bool doApplyRootMotion)
        {
            CharacterAnimationData = Data.CharacterData.CharacterAnimationData;
            CharacterAnimator = characterAnimator;
            CharacterAnimatorController = animatorController;
            characterAnimator.runtimeAnimatorController = CharacterAnimatorController;
            CharacterAnimator.applyRootMotion = doApplyRootMotion;

            MovementAnimationHash = Animator.StringToHash(CharacterAnimationData.MovementAnimationName);
            StrafingAnimationHash = Animator.StringToHash(CharacterAnimationData.StrafingAnimationName);
            SlidingForwardAnimationHash = Animator.StringToHash(CharacterAnimationData.SlidingForwardAnimationName);
            LongDodgeAnimationHash = Animator.StringToHash(CharacterAnimationData.LongDodgeAnimationName);
            ShortDodgeAnimationHash = Animator.StringToHash(CharacterAnimationData.ShortDodgeAnimationName);
            TrapPlacingAnimationHash = Animator.StringToHash(CharacterAnimationData.TrapPlacingAnimationName);
        }

        #endregion
    }
}

