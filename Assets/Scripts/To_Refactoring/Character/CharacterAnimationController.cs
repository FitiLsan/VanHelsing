using UnityEngine;


namespace BeastHunter
{
    public class CharacterAnimationController
    {
        #region Fields

        private int _defaultMovementHash;
        private int _defaultIdleHash;
        private int _jumpHash;
        private int _fallHash;
        private int _battleIdleHash;
        private int _battleMovementHash;
        private int _battleTargetMovementHash;
        private int _rollHash;
        private int _rollForwardHash;
        private int _dancingHash;
        private int _stunnedHash;
        private int _attackHash;
        private int _deadHash;

        #endregion


        #region Properties

        private Animator CharacterAnimator { get; set; }

        #endregion


        #region ClassLifeCycles

        public CharacterAnimationController(Animator characterAnimator)
        {
            CharacterAnimator = characterAnimator;
            Initialize();
        }

        #endregion


        #region Methods

        public void Initialize()
        {
            _defaultMovementHash = Animator.StringToHash("DefaultMovement");
            _jumpHash = Animator.StringToHash("Jump");
            _defaultIdleHash = Animator.StringToHash("DefaultIdle");
            _dancingHash = Animator.StringToHash("Dancing");
            _fallHash = Animator.StringToHash("Fall");
            _rollHash = Animator.StringToHash("Roll");
            _rollForwardHash = Animator.StringToHash("RollForward");
            _battleIdleHash = Animator.StringToHash("BattleIdle");
            _battleMovementHash = Animator.StringToHash("BattleMovement");
            _battleTargetMovementHash = Animator.StringToHash("BattleTargetMovement");
            _attackHash = Animator.StringToHash("Attack");
            _stunnedHash = Animator.StringToHash("Stunned");
            _deadHash = Animator.StringToHash("Dead");
        }

        public void UpdateAnimationParameters(float axisX, float axisY, float moveSpeed, float animationSpeed)
        {
            if (CharacterAnimator != null)
            {
                CharacterAnimator.SetFloat("AxisX", axisX);
                CharacterAnimator.SetFloat("AxisY", axisY);
                CharacterAnimator.SetFloat("MoveSpeed", moveSpeed);
                CharacterAnimator.speed = animationSpeed;
            }
        }

        public void PlayDefaultIdleAnimation()
        {
            CharacterAnimator.Play(_defaultIdleHash);
        }

        public void PlayDefaultMovementAnimation()
        {
            CharacterAnimator.Play(_defaultMovementHash);
        }

        public void PlayBattleIdleAnimation()
        {
            CharacterAnimator.Play(_battleIdleHash);
        }

        public void PlayBattleMovementAnimation()
        {
            CharacterAnimator.Play(_battleMovementHash);
        }

        public void PlayBattleTargetMovementAnimation()
        {
            CharacterAnimator.Play(_battleTargetMovementHash);
        }

        public void PlayJumpAnimation()
        {
            CharacterAnimator.Play(_jumpHash);
        }

        public void PlayRollAnimation(float rollingX, float rollingY)
        {
            CharacterAnimator.SetFloat("RollingX", rollingX);
            CharacterAnimator.SetFloat("RollingY", rollingY);
            CharacterAnimator.Play(_rollHash);
        }

        public void PlayRollForwardAnimation()
        {
            CharacterAnimator.Play(_rollForwardHash);
        }

        public void PlayDancingAnimation()
        {
            CharacterAnimator.Play(_dancingHash);
        }

        public void PlayAttackAnimation(float attackForce)
        {
            CharacterAnimator.SetFloat("AttackForce", attackForce);
            CharacterAnimator.Play(_attackHash);
        }

        public void PlayStunnedAnimation()
        {
            CharacterAnimator.Play(_stunnedHash);
        }

        public void PlayDeadAnimation()
        {
            CharacterAnimator.Play(_deadHash);
        }

        public void PlayFallAnimation()
        {
            CharacterAnimator.Play(_fallHash);
        }

        #endregion
    }
}

