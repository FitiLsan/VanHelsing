using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterAnimationsController : IAwake, IUpdate
    {
        #region Properties

        private readonly GameContext _context;

        private CharacterModel _characterModel;
        private InputModel _inputModel;
        private Animator CharacterAnimator { get; set; }
        private bool IsCharacterMoving { get; set; }

        #endregion


        #region ClassLifeCycles

        public CharacterAnimationsController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            _characterModel = _context.CharacterModel;
            _inputModel = _context.InputModel;
            CharacterAnimator = _characterModel.CharacterAnimator;
            IsCharacterMoving = false;
            CreateAnimationEvents();
        }

        #endregion


        #region Updating

        public void Updating()
        {
            CheckCharacterMoving();
            UpdateAnimation();
        }

        #endregion


        #region Methods

        public void UpdateAnimation()
        {
            if(CharacterAnimator != null)
            {
                CharacterAnimator.SetBool("IsMoving", IsCharacterMoving);
                CharacterAnimator.SetBool("IsGrounded", _characterModel.IsGrounded);
                CharacterAnimator.SetBool("IsInBattleMode", _characterModel.IsInBattleMode);
                CharacterAnimator.SetBool("IsAttacking", _characterModel.IsAttacking);
                CharacterAnimator.SetBool("IsDead", _characterModel.IsDead);
                CharacterAnimator.SetBool("IsTargeting", _characterModel.IsTargeting);
                CharacterAnimator.SetBool("IsDancing", _characterModel.IsDansing);
                CharacterAnimator.SetFloat("MovementSpeed", _characterModel.CurrentSpeed);
                CharacterAnimator.SetFloat("VerticalSpeed", _characterModel.VerticalSpeed);
                CharacterAnimator.SetFloat("AxisX", _inputModel.inputStruct._inputAxisX);
                CharacterAnimator.SetFloat("AxisY", _inputModel.inputStruct._inputAxisY);
                CharacterAnimator.SetInteger("AttackIndex", _characterModel.AttackIndex);
            }

            CharacterAnimator.speed = _characterModel.AnimationSpeed;
        }

        public void CreateAnimationEvents()
        {
            var animationClips = CharacterAnimator.runtimeAnimatorController.animationClips;

            for (int clip = 0; clip < animationClips.Length; clip++)
            {
                foreach (var clipEvent in animationClips[clip].events)
                {
                    clipEvent.objectReferenceParameter = _characterModel.PlayerBehaviour;
                    clipEvent.functionName = "EventHit";
                    Debug.Log("added event");
                }
            }
        }

        private void setException()
        {
            throw new System.Exception("animationEvent");
        }

        public void CheckCharacterMoving()
        {
            if(_inputModel.inputStruct._inputAxisX != 0 || _inputModel.inputStruct._inputAxisY != 0)
            {
                IsCharacterMoving = true;
            }
            else
            {
                IsCharacterMoving = false;
            }
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

