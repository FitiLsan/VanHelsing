using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterController : IAwake, IUpdate
    {
        #region Fields

        public CharacterModel _characterModel;
        public CharacterAnimationsController _characterAnimationsController;

        private readonly GameContext _context;
    
        private CharacterInputController _inputController;
        private CharacterStateMachine _stateMachine;
        private Services _services;

        private DefaultMovementState _defaultMovementState;
        private BattleState _battleState;
        private JumpingState _jumpingState;
        private FallingState _fallingState;

        private Vector3 _groundHit;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;

        private float _speedCountTime;
        private float _speedCountFrame;

        #endregion


        #region Properties

        public float CurrentSpeed { get; private set; }
        public float BattleIgnoreTime { get; private set; }
        public float CurrentBattleIgnoreTime { get; private set; }

        public bool IsGrounded { get; private set; }
        public bool IsEnemyNear { get; private set; }
        public bool IsInBattleMode { get; private set; }
        public bool IsCameraFixed { get; private set; }

        #endregion


        #region ClassLifeCycle

        public CharacterController(GameContext context, Services services)
        {
            _context = context;
            _services = services;
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            Cursor.visible = false;  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            IsInBattleMode = false;
            IsEnemyNear = false;

            _characterModel = _context._characterModel;
            _characterAnimationsController = new CharacterAnimationsController(_characterModel.CharacterAnimator);
            _inputController = new CharacterInputController();
            _inputController.Initialize();
            _speedCountFrame = _characterModel.CharacterStruct.SpeedMeasureFrame;
            _lastPosition = _characterModel.CharacterTransform.position;
            _currentPosition = _lastPosition;

            _defaultMovementState = new DefaultMovementState(_inputController, this, _characterModel);
            _battleState = new BattleState(_inputController, this, _characterModel);
            _jumpingState = new JumpingState(_inputController, this, _characterModel);
            _fallingState = new FallingState(_inputController, this, _characterModel);

            _stateMachine = new CharacterStateMachine(_defaultMovementState);
            BattleIgnoreTime = _characterModel.CharacterStruct.BattleIgnoreTime;
            CurrentBattleIgnoreTime = BattleIgnoreTime;

            _characterModel.PlayerBehaviour.OnCollidersNearChange.AddListener(CheckTrigger);
            _inputController.OnJump.AddListener(SetJumpingState);
            _inputController.OnBattleExit.AddListener(SetDefaultMovementState);
            _inputController.OnTargetLock.AddListener(ChangeCameraFix);
            IsCameraFixed = false;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            GroundCheck();
            SpeedCheck();
            StateCheck();

            _inputController.GetInput();
            _stateMachine.CurrentState.Execute();
        }

        #endregion


        #region Methods     

        private void GroundCheck()
        {
            IsGrounded = _services.PhysicsService.CheckGround(_characterModel.CharacterCapsuleCollider.transform.position + 
                Vector3.up, _characterModel.CharacterStruct.GroundCheckHeight, out _groundHit);
        }

        private void SpeedCheck()
        {
            if (_speedCountTime > 0)
            {
                _speedCountTime -= Time.fixedDeltaTime;
            }
            else
            {
                _speedCountTime = _speedCountFrame;
                _currentPosition = _characterModel.CharacterTransform.position;
                CurrentSpeed = Mathf.Sqrt((_currentPosition - _lastPosition).sqrMagnitude) / _speedCountFrame;
                _lastPosition = _currentPosition;
            }
        }

        private void CheckTrigger()
        {
            bool foundEnemy = false;

            foreach (var collider in _characterModel._collidersInTrigger)
            {
                if (collider.tag == "Enemy")
                {
                    foundEnemy = true;
                    break;
                }
            }

            IsEnemyNear = foundEnemy;

            if (IsEnemyNear)
            {
                _stateMachine.SetState(_battleState);
            }
            else
            {
                SetDefaultMovementState();
            }
        }

        private void ChangeCameraFix()
        {
            IsCameraFixed = !IsCameraFixed;

            if (IsCameraFixed)
            {
                _characterModel.CharacterCinemachineTargetCamera.Priority = 15;
            }
            else
            {
                _characterModel.CharacterCinemachineTargetCamera.Priority = 5;
            }
        }

        private void StateCheck()
        {
            // TODO
        }

        private void SetJumpingState()
        {
            _stateMachine.SetState(_jumpingState);
        }

        private void SetFallingState()
        {
            _stateMachine.SetState(_fallingState);
        }

        private void SetDefaultMovementState()
        {
            _stateMachine.SetState(_defaultMovementState);
        }

        private void SetBattleState()
        {
            if (IsEnemyNear)
            {
                _stateMachine.SetState(_battleState);
                CurrentBattleIgnoreTime = BattleIgnoreTime;
            }
        }

        #endregion
    }
}

