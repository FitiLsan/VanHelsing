using UnityEngine;


namespace BeastHunter
{
    public class SneakingState : CharacterBaseState, IUpdate
    {
        #region Constants

        private const float CHANGE_TIME = 1f;
        private const float ZERO_CROUCH_LEVEL = 0f;
        private const float FULL_CROUCH_LEVEL = 0.8f;
        private const float SNEAK_LEVEL_DECREASE_WHILE_RUNNING = 1.5f;
        private const float POSE_CHANGE_SPEED = 0.3f;

        #endregion


        #region Properties

        private CharacterPositionEnum CharacterPose { get; set; }

        private float _crouchLevel;
        private float _targetCrouchLevelForAnimation;
        private float _crouchLevelForAnimation;
        private float _exitTime;

        private bool _isMoving;

        #endregion


        #region ClassLifeCycle

        public SneakingState(GameContext context, CharacterStateMachine stateMachine) : base(context, stateMachine)
        {
            Type = StateType.Default;
            IsTargeting = false;
            IsAttacking = false;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _stateMachine.BackState.CountSpeedSneaking();
            ControlMovement();
            ControlPose();
            ContolAnimation();
        }

        #endregion


        #region Methods

        public override void Initialize()
        {
            base.Initialize();

            CharacterPose = CharacterPositionEnum.StandingToSneaking;
            _crouchLevel = ZERO_CROUCH_LEVEL;
            _crouchLevelForAnimation = _crouchLevel;
            _isMoving = _inputModel.IsInputMove;

            _stateMachine.BackState.OnMove = () => _isMoving = true;
            _stateMachine.BackState.OnStop = () => _isMoving = false;
            _stateMachine.BackState.OnJump = () => CharacterPose = CharacterPositionEnum.SneakingToStanding;
            _stateMachine.BackState.OnSneak = () => CharacterPose = CharacterPositionEnum.SneakingToStanding;

            _animationController.PlayMovementAnimation();
        }

        public override void OnExit()
        {
            base.OnExit();

            _stateMachine.BackState.OnMove = null;
            _stateMachine.BackState.OnStop = null;
            _stateMachine.BackState.OnJump = null;
            _stateMachine.BackState.OnSneak = null;
        }

        private void ControlMovement()
        {
            _stateMachine.BackState.RotateCharacter(_isMoving);
            _stateMachine.BackState.MoveCharacter();
        }

        private void ControlPose()
        {
            switch (CharacterPose)
            {
                case CharacterPositionEnum.None:
                    break;
                case CharacterPositionEnum.Standing:
                    ExitState();
                    break;
                case CharacterPositionEnum.StandingToSneaking:
                    IncreaseSneak();
                    break;
                case CharacterPositionEnum.Sneaking:
                    break;
                case CharacterPositionEnum.SneakingToStanding:
                    DecreaceSneak();
                    break;
                default:
                    break;
            }
        }

        private void ContolAnimation()
        {
            _targetCrouchLevelForAnimation = _inputModel.IsInputRun ? 
                _crouchLevel / SNEAK_LEVEL_DECREASE_WHILE_RUNNING : _crouchLevel;

            _crouchLevelForAnimation = Mathf.SmoothStep(_crouchLevelForAnimation, _targetCrouchLevelForAnimation,
                POSE_CHANGE_SPEED);

            _animationController.SetCrouchLevel(_crouchLevelForAnimation);
        }

        private void ExitState()
        {
            if (_inputModel.IsInputMove)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultMovement]);
            }
            else
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
            }
        }

        private void IncreaseSneak()
        {
            if(_crouchLevel >= FULL_CROUCH_LEVEL)
            {
                _crouchLevel = FULL_CROUCH_LEVEL;
                CharacterPose = CharacterPositionEnum.Sneaking;
            }
            else
            {
                _crouchLevel += Time.deltaTime;
            }           
        }

        private void DecreaceSneak()
        {
            if (_crouchLevel <= ZERO_CROUCH_LEVEL)
            {
                _crouchLevel = ZERO_CROUCH_LEVEL;
                CharacterPose = CharacterPositionEnum.Standing;
            }
            else
            {
                _crouchLevel -= Time.deltaTime;
            }
        }

        #endregion
    }
}

