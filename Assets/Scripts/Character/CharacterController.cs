using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterController : IAwake, IUpdate, ITearDown, IDealDamage, ITakeDamage
    {
        #region Fields

        public CharacterModel _characterModel;
        private InputModel _inputModel;

        private readonly GameContext _context;

        private CharacterStateMachine _stateMachine;
        private Services _services;

        private DefaultIdleState _defaultIdleState;
        private BattleIdleState _battleIdleState;
        private DefaultMovementState _defaultMovementState;
        private BattleMovementState _battleMovementState;
        private AttackingState _attackingState;
        private JumpingState _jumpingState;
        private DodgingState _dodgingState;
        private FallingState _fallingState;
        private DeadState _deadState;

        private Vector3 _groundHit;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;

        private float _speedCountTime;
        private float _speedCountFrame;
        private float _currentHealth;

        #endregion


        #region ClassLifeCycle

        public CharacterController(GameContext context)
        {
            _context = context;
            _inputModel = _context._inputModel;
            _services = Services.SharedInstance;
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            Cursor.visible = false;  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            _characterModel = _context._characterModel;

            _speedCountFrame = _characterModel.CharacterCommonSettings.SpeedMeasureFrame;
            _lastPosition = _characterModel.CharacterTransform.position;
            _currentPosition = _lastPosition;
            _currentHealth = _characterModel.CharacterCommonSettings.HealthPoints;

            _defaultIdleState = new DefaultIdleState(_characterModel, _inputModel);
            _battleIdleState = new BattleIdleState(_characterModel, _inputModel);
            _defaultMovementState = new DefaultMovementState(_characterModel, _inputModel);
            _battleMovementState = new BattleMovementState(_characterModel, _inputModel);
            _attackingState = new AttackingState(_characterModel, _inputModel);
            _jumpingState = new JumpingState(_characterModel, _inputModel);
            _dodgingState = new DodgingState(_characterModel, _inputModel);
            _fallingState = new FallingState(_characterModel, _inputModel);
            _deadState = new DeadState(_characterModel, _inputModel);

            _stateMachine = new CharacterStateMachine(_defaultIdleState);

            _inputModel.OnJump += SetJumpingState;
            _inputModel.OnBattleExit += ExitBattle;
            _inputModel.OnTargetLock += ChangeCameraFix;
            _inputModel.OnAttack += SetAttackingState;

            _characterModel.PlayerBehaviour.OnFilterHandler += OnFilterHandler;
            _characterModel.PlayerBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
            _characterModel.PlayerBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            if (!_characterModel.IsDead)
            {
                HealthCheck();
                GroundCheck();
                SpeedCheck();
                StateCheck();

                _stateMachine.CurrentState.Execute();
            }
        }

        #endregion

        #region ITearDownController

        public void TearDown()
        {
            _characterModel.PlayerBehaviour.OnFilterHandler -= OnFilterHandler;
            _characterModel.PlayerBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
            _characterModel.PlayerBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            _inputModel.OnJump -= SetJumpingState;
            _inputModel.OnBattleExit -= ExitBattle;
            _inputModel.OnTargetLock -= ChangeCameraFix;
            _inputModel.OnAttack -= SetAttackingState;
        }

        #endregion


        #region IDealDamage

        public void DealDamage(Collider enemy, DamageStruct damage)
        {

        }

        #endregion


        #region ITakeDamage

        public void TakeDamage(DamageStruct damage)
        {

        }

        #endregion


        #region Methods     

        private void HealthCheck()
        {
            if(_currentHealth <= 0)
            {
                _characterModel.IsDead = true;
            }
        }

        private void GroundCheck()
        {
            _characterModel.IsGrounded = _services.PhysicsService.CheckGround(_characterModel.CharacterCapsuleCollider.
                transform.position + Vector3.up, _characterModel.CharacterCommonSettings.GroundCheckHeight, out _groundHit);
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
                _characterModel.CurrentSpeed = Mathf.Sqrt((_currentPosition - _lastPosition).sqrMagnitude) / 
                    _speedCountFrame;
                _lastPosition = _currentPosition;
            }

            _characterModel.VerticalSpeed = _characterModel.CharacterRigitbody.velocity.y;
        }

        private void CheckTrigger()
        {
            _characterModel.IsEnemyNear = _characterModel.EnemiesInTrigger.Count > 0;

            if (_characterModel.IsEnemyNear)
            {
                _characterModel.IsInBattleMode = true;
            }
            else
            {
                _characterModel.IsInBattleMode = false;
            }
        }

        private void ChangeCameraFix()
        {
            _characterModel.IsCameraFixed = !_characterModel.IsCameraFixed;

            if (_characterModel.IsCameraFixed)
            {
                _characterModel.CharacterTargetCamera.Priority = 15;
            }
            else
            {
                _characterModel.CharacterTargetCamera.Priority = 5;
            }
        }

        #endregion


        #region StateControl

        private void StateCheck()
        {
            if (_characterModel.IsDead)
            {
                _stateMachine.SetStateOverride(_deadState);
            }
            else if (!_characterModel.IsAttacking && (_inputModel.inputStruct._inputAxisX != 0 ||
                _inputModel.inputStruct._inputAxisY != 0))
            {
                if (_characterModel.IsInBattleMode)
                {
                    _stateMachine.SetStateOverride(_battleMovementState);
                }
                else
                {
                    _stateMachine.SetState(_defaultMovementState);
                }
            }
            else if (!_characterModel.IsAttacking)
            {
                if (_characterModel.IsInBattleMode)
                {
                    _stateMachine.SetStateOverride(_battleIdleState);
                }
                else
                {
                    _stateMachine.SetState(_defaultIdleState);
                }
            }
        }

        private void SetJumpingState()
        {
            if (_characterModel.IsGrounded)
            {
                _stateMachine.SetStateOverride(_jumpingState);
            }
        }

        private void SetDodgingState()
        {
            _stateMachine.SetState(_dodgingState);
        }

        private void SetFallingState()
        {
            _stateMachine.SetState(_fallingState);
        }

        private void SetAttackingState()
        {
            if (_stateMachine.CurrentState != _attackingState && _characterModel.IsInBattleMode)
            {
                _stateMachine.SetStateOverride(_attackingState);
            }
        }

        private void ExitBattle()
        {
            _characterModel.IsInBattleMode = false;
        }

        #endregion


        #region TriggerControl

        private bool OnFilterHandler(Collider tagObject)
        {
            return tagObject.CompareTag(TagManager.ENEMY);
        }

        private void OnTriggerEnterHandler(ITrigger thisdObject, Collider enteredObject)
        {
            _characterModel.EnemiesInTrigger.Add(enteredObject);
            CheckTrigger();
        }

        private void OnTriggerExitHandler(ITrigger thisdObject, Collider exitedObject)
        {
            _characterModel.EnemiesInTrigger.Remove(exitedObject);
            CheckTrigger();
        }

        #endregion
    }
}

