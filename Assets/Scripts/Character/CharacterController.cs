using UnityEngine;
using Cinemachine;


namespace BeastHunter
{
    public sealed class CharacterController : IAwake, IUpdate, ITearDown, IDealDamage
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
        private DancingState _dancingState;
        private DeadState _deadState;

        private Vector3 _groundHit;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;

        private float _speedCountTime;
        private float _speedCountFrame;
        private float _currentHealth;
        private float _currentBattleIgnoreTime;

        #endregion


        #region ClassLifeCycle

        public CharacterController(GameContext context)
        {
            _context = context;
            _inputModel = _context.InputModel;
            _services = Services.SharedInstance;
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            Cursor.visible = false;  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            _characterModel = _context.CharacterModel;

            _speedCountFrame = _characterModel.CharacterCommonSettings.SpeedMeasureFrame;
            _lastPosition = _characterModel.CharacterTransform.position;
            _currentPosition = _lastPosition;
            _currentHealth = _characterModel.CharacterCommonSettings.HealthPoints;
            _currentBattleIgnoreTime = 0;

            _defaultIdleState = new DefaultIdleState(_characterModel, _inputModel);
            _battleIdleState = new BattleIdleState(_characterModel, _inputModel);
            _defaultMovementState = new DefaultMovementState(_characterModel, _inputModel);
            _battleMovementState = new BattleMovementState(_characterModel, _inputModel);
            _attackingState = new AttackingState(_characterModel, _inputModel);
            _jumpingState = new JumpingState(_characterModel, _inputModel);
            _dodgingState = new DodgingState(_characterModel, _inputModel);
            _fallingState = new FallingState(_characterModel, _inputModel);
            _dancingState = new DancingState(_characterModel, _inputModel);
            _deadState = new DeadState(_characterModel, _inputModel);

            _stateMachine = new CharacterStateMachine(_defaultIdleState);

            _inputModel.OnJump += SetJumpingState;
            _inputModel.OnBattleExit += ExitBattle;
            _inputModel.OnTargetLock += ChangeCameraFix;
            _inputModel.OnAttack += SetAttackingState;
            _inputModel.OnDance += ChangeDancingState;

            _characterModel.PlayerBehaviour.OnFilterHandler += OnFilterHandler;
            _characterModel.PlayerBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
            _characterModel.PlayerBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            _characterModel.PlayerBehaviour.OnTakeDamage += TakeDamage;

            foreach (var hitBoxBehavior in _characterModel.PlayerHitBoxes)
            {
                hitBoxBehavior.OnFilterHandler += OnHitBoxFilter;
                hitBoxBehavior.OnTriggerEnterHandler += OnHitBoxHit;
                hitBoxBehavior.OnTriggerExitHandler += OnHitEnded;
            }
        }

        #endregion


        #region Updating

        public void Updating()
        {
            if (!_characterModel.IsDead)
            {
                BattleIgnoreCheck();
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
            _inputModel.OnDance -= ChangeDancingState;

            foreach (var hitBoxBehavior in _characterModel.PlayerHitBoxes)
            {
                hitBoxBehavior.OnFilterHandler -= OnHitBoxFilter;
                hitBoxBehavior.OnTriggerEnterHandler -= OnHitBoxHit;
                hitBoxBehavior.OnTriggerExitHandler -= OnHitEnded;
            }
        }

        #endregion


        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, DamageStruct damage)
        {
            if(enemy.GetComponent<InteractableObjectBehavior>() != null)
            {
                enemy.GetComponent<InteractableObjectBehavior>().OnTakeDamage(damage);
            }
        }

        #endregion


        #region ITakeDamage

        private void TakeDamage(DamageStruct damage)
        {
            if (!_characterModel.IsDead)
            {
                _currentHealth -= damage.damage;

                if (!_characterModel.IsInBattleMode)
                {
                    _currentBattleIgnoreTime = 0;
                    _characterModel.IsInBattleMode = true;
                }
            }
        }

        #endregion


        #region Methods     

        private void BattleIgnoreCheck()
        {
            if(_currentBattleIgnoreTime > 0)
            {
                _currentBattleIgnoreTime -= Time.deltaTime;
            }
        }

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

            if (_characterModel.IsEnemyNear && _currentBattleIgnoreTime <= 0)
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
            if (_characterModel.IsCameraFixed)
            {
                _characterModel.IsCameraFixed = false;
                _characterModel.IsTargeting = false;
            }
            else
            {
                if (_characterModel.IsEnemyNear)
                {
                    _characterModel.IsCameraFixed = true;
                    _characterModel.IsInBattleMode = true;
                    _characterModel.IsTargeting = true;
                }
            }

            CheckCameraFixation();
        }

        private void CheckCameraFixation()
        {
            if (_characterModel.IsCameraFixed)
            {
                _characterModel.CharacterCamera.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 1f;
                _characterModel.CharacterTargetCamera.Priority = 15;
            }
            else
            {
                _characterModel.CharacterCamera.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 0f;
                _characterModel.CharacterTargetCamera.Priority = 5;
            }
        }

        #endregion


        #region StateControl

        private void StateCheck()
        {
            if(_stateMachine.CurrentState != _attackingState)
            {
                _characterModel.IsAttacking = false;
            }

            if (_characterModel.IsDead)
            {
                _stateMachine.SetStateAnyway(_deadState);
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

            if(_stateMachine.CurrentState != _dancingState)
            {
                _characterModel.IsDansing = false;
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

        private void ChangeDancingState()
        {
            if(_stateMachine.CurrentState == _defaultIdleState)
            {
                _stateMachine.SetStateOverride(_dancingState);
            }
            else if (_stateMachine.CurrentState == _dancingState)
            {
                _stateMachine.ReturnStateOverride();
            }
        }

        private void SetAttackingState()
        {
            if (_stateMachine.CurrentState != _attackingState)
            {
                _stateMachine.SetStateOverride(_attackingState);
                _characterModel.IsInBattleMode = true;
            }
        }

        private void ExitBattle()
        {
            _currentBattleIgnoreTime = _characterModel.CharacterCommonSettings.BattleIgnoreTime;
            _characterModel.IsInBattleMode = false;
            _characterModel.IsTargeting = false;
            _characterModel.IsCameraFixed = false;
            CheckCameraFixation();
        }

        #endregion


        #region TriggerControl

        private bool OnFilterHandler(Collider tagObject)
        {
            return !tagObject.isTrigger && tagObject.CompareTag(TagManager.ENEMY);
        }

        private void OnTriggerEnterHandler(ITrigger thisdObject, Collider enteredObject)
        {
            if(thisdObject.Type == InteractableObjectType.Player && !_characterModel.EnemiesInTrigger.Contains(enteredObject))
            {          
                _characterModel.EnemiesInTrigger.Add(enteredObject);
                CheckTrigger();
            }
        }

        private void OnTriggerExitHandler(ITrigger thisdObject, Collider exitedObject)
        {
            if(thisdObject.Type == InteractableObjectType.Player && _characterModel.EnemiesInTrigger.Contains(exitedObject))
            {
                _characterModel.EnemiesInTrigger.Remove(exitedObject);
                CheckTrigger();
            }
        }

        private bool OnHitBoxFilter(Collider hitedObject)
        {
            bool isEnemyColliderHit = hitedObject.CompareTag(TagManager.ENEMY);

            if (hitedObject.isTrigger || !_characterModel.IsAttacking)
            {
                isEnemyColliderHit = false;
            }

            return isEnemyColliderHit;
        }

        private void OnHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.Type == InteractableObjectType.HitBox && hitBox.IsInteractable)
            {
                DealDamage(enemy.transform.GetComponent<InteractableObjectBehavior>(), _characterModel.CharacterCommonSettings.CharacterDamage);
                hitBox.IsInteractable = false;
            }
        }

        private void OnHitEnded(ITrigger hitBox, Collider enemy)
        {
            if (hitBox.IsInteractable)
            {
                hitBox.IsInteractable = false;
            }
        }

        #endregion
    }
}

