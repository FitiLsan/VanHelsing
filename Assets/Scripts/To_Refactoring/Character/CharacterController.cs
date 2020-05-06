using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterController : IAwake, IUpdate, ITearDown, IDealDamage
    {
        #region Fields

        private CharacterModel _characterModel;
        private InputModel _inputModel;

        private readonly GameContext _context;
        private Services _services;
        private CharacterStateMachine _stateMachine;
        private CharacterAnimationController _animationController;

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
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _characterModel = _context.CharacterModel;
            _animationController = new CharacterAnimationController(_characterModel.CharacterAnimator);

            _speedCountFrame = _characterModel.CharacterCommonSettings.SpeedMeasureFrame;
            _lastPosition = _characterModel.CharacterTransform.position;
            _currentPosition = _lastPosition;
            _currentHealth = _characterModel.CharacterCommonSettings.HealthPoints;
            _currentBattleIgnoreTime = 0;

            _stateMachine = new CharacterStateMachine(_inputModel, _characterModel, _animationController);
            _stateMachine.SetStartState(_stateMachine._defaultIdleState);

            _inputModel.OnJump += SetJumpingState;
            _inputModel.OnBattleExit += ExitBattle;
            _inputModel.OnTargetLock += ChangeTargetMode;
            _inputModel.OnAttack += SetAttackingState;
           // _inputModel.OnDance += ChangeDancingState;

            _characterModel.PlayerBehaviour.OnFilterHandler += OnFilterHandler;
            _characterModel.PlayerBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
            _characterModel.PlayerBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            _characterModel.PlayerBehaviour.OnTakeDamageHandler += TakeDamage;

            _stateMachine.OnStateChangeHandler += OnStateChange;

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
            if (_stateMachine.CurrentState != _stateMachine._deadState)
            {
                BattleIgnoreCheck();
                GroundCheck();
                MovementCheck();
                SpeedCheck();
                StateCheck();
                _stateMachine.CurrentState.Execute();
            }

            _animationController.UpdateAnimationParameters(_inputModel.inputStruct._inputTotalAxisX, 
                _inputModel.inputStruct._inputTotalAxisY, _characterModel.CurrentSpeed, _characterModel.AnimationSpeed);
        }

        #endregion

        #region ITearDownController

        public void TearDown()
        {
            _characterModel.PlayerBehaviour.OnFilterHandler -= OnFilterHandler;
            _characterModel.PlayerBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
            _characterModel.PlayerBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            _characterModel.PlayerBehaviour.OnTakeDamageHandler -= TakeDamage;

            _inputModel.OnJump -= SetJumpingState;
            _inputModel.OnBattleExit -= ExitBattle;
            _inputModel.OnTargetLock -= ChangeTargetMode;
            _inputModel.OnAttack -= SetAttackingState;
            _inputModel.OnDance -= ChangeDancingState;
            _stateMachine.OnStateChangeHandler -= OnStateChange;

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
                enemy.GetComponent<InteractableObjectBehavior>().OnTakeDamageHandler(damage);
            }
        }

        #endregion


        #region ITakeDamage

        private void TakeDamage(DamageStruct damage)
        {
            if (_stateMachine.CurrentState != _stateMachine._deadState)
            {
                _currentHealth -= damage.damage;
                float stunProbability = Random.Range(0f, 1f);

                if (_currentHealth <= 0)
                {
                    _stateMachine.SetStateAnyway(_stateMachine._deadState);
                }
                else if (stunProbability > damage.stunProbability)
                {
                    _stateMachine.SetStateAnyway(_stateMachine._stunnedState);
                }

                _characterModel.IsInBattleMode = true; 
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

        private void GroundCheck()
        {
            if (_stateMachine.CurrentState != _stateMachine._jumpingState)
            {
                _characterModel.IsGrounded = _services.PhysicsService.CheckGround(_characterModel.CharacterCapsuleCollider.
                    transform.position + Vector3.up, _characterModel.CharacterCommonSettings.GroundCheckHeight, out _groundHit);
            }
            else
            {
                _characterModel.IsGrounded = _services.PhysicsService.CheckGround(_characterModel.CharacterCapsuleCollider.
                    transform.position + Vector3.up, _characterModel.CharacterCommonSettings.GroundCheckHeight *
                        _characterModel.CharacterCommonSettings.GroundCHeckHeightReduction, out _groundHit);
            }
        }

        private void MovementCheck()
        {
            if (_inputModel.inputStruct._inputAxisX != 0 || _inputModel.inputStruct._inputAxisY != 0)
            {
                _characterModel.IsMoving = true;
            }
            else
            {
                _characterModel.IsMoving = false;
            }
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

        #endregion


        #region StateControl

        private void StateCheck()
        {
            if (_stateMachine.CurrentState.CanExit)
            {
                if (!_characterModel.IsGrounded)
                {
                    _stateMachine.SetState(_stateMachine._fallingState);
                }
                else if (_characterModel.IsInBattleMode)
                {
                    if (_characterModel.IsMoving)
                    {
                        _stateMachine.SetState(_stateMachine._battleMovementState);
                    }
                    else
                    {
                        _stateMachine.SetState(_stateMachine._battleIdleState);
                    }
                }
                else
                {
                    if (_characterModel.IsMoving)
                    {
                        _stateMachine.SetState(_stateMachine._defaultMovementState);
                    }
                    else
                    {
                        _stateMachine.SetState(_stateMachine._defaultIdleState);
                    }
                }        
            }
        }

        private void ChangeTargetMode()
        {
            if (_characterModel.IsInBattleMode)
            {
                if (_stateMachine.CurrentState == _stateMachine._battleTargetMovementState)
                {
                    if (_characterModel.IsMoving)
                    {
                        _stateMachine.SetStateOverride(_stateMachine._battleMovementState);
                    }
                    else
                    {
                        _stateMachine.SetStateOverride(_stateMachine._battleIdleState);
                    }
                }
                else
                {
                    if (_characterModel.IsEnemyNear)
                    {
                        _stateMachine.SetStateOverride(_stateMachine._battleTargetMovementState);
                    }
                }
            }
            else
            {
                if (_characterModel.IsEnemyNear)
                {
                    _stateMachine.SetStateOverride(_stateMachine._battleTargetMovementState);
                }
            }
        }

        private void SetJumpingState()
        {
            if (_characterModel.IsGrounded)
            {
                if (!_characterModel.IsInBattleMode)
                {
                    _stateMachine.SetStateOverride(_stateMachine._jumpingState);
                }
                else
                {
                    if(_stateMachine.CurrentState == _stateMachine._battleTargetMovementState)
                    {
                        _stateMachine.SetStateOverride(_stateMachine._rollingTargetState);
                    }
                    else
                    {
                        _stateMachine.SetStateOverride(_stateMachine._rollingState);
                    }
                }
            }
        }

        private void SetDodgingState()
        {
            _stateMachine.SetState(_stateMachine._dodgingState);
        }

        private void SetFallingState()
        {
            _stateMachine.SetState(_stateMachine._fallingState);
        }

        private void ChangeDancingState()
        {
            if(_stateMachine.CurrentState == _stateMachine._defaultIdleState)
            {
                _stateMachine.SetStateOverride(_stateMachine._dancingState);
            }
            else if (_stateMachine.CurrentState == _stateMachine._dancingState)
            {
                _stateMachine.ReturnStateOverride();
            }
        }

        private void SetAttackingState()
        {
            if (_stateMachine.CurrentState != _stateMachine._attackingState && _characterModel.IsGrounded)
            {
                _characterModel.IsInBattleMode = true;
                _stateMachine.SetStateOverride(_stateMachine._attackingState);
            }
        }

        private void ExitBattle()
        {
            _currentBattleIgnoreTime = _characterModel.CharacterCommonSettings.BattleIgnoreTime;

            if (_characterModel.IsMoving)
            {
                _stateMachine.SetStateOverride(_stateMachine._defaultMovementState);
            }
            else
            {
                _stateMachine.SetStateOverride(_stateMachine._defaultIdleState);
            }

            _characterModel.IsInBattleMode = false;
        }

        private void OnStateChange(CharacterBaseState previousState, CharacterBaseState newState)
        {

        }

        #endregion


        #region TriggerControl

        private bool OnFilterHandler(Collider tagObject)
        {
            return !tagObject.isTrigger && tagObject.CompareTag(TagManager.ENEMY);
        }

        private void OnTriggerEnterHandler(ITrigger thisdObject, Collider enteredObject)
        {
            if(thisdObject.Type == InteractableObjectType.Player && !_characterModel.EnemiesInTrigger.
                Contains(enteredObject))
            {          
                _characterModel.EnemiesInTrigger.Add(enteredObject);
                CheckTrigger();
            }
        }

        private void OnTriggerExitHandler(ITrigger thisdObject, Collider exitedObject)
        {
            if(thisdObject.Type == InteractableObjectType.Player && _characterModel.EnemiesInTrigger.
                Contains(exitedObject))
            {
                _characterModel.EnemiesInTrigger.Remove(exitedObject);
                CheckTrigger();
            }
        }

        private bool OnHitBoxFilter(Collider hitedObject)
        {
            bool isEnemyColliderHit = hitedObject.CompareTag(TagManager.ENEMY);

            if (hitedObject.isTrigger || _stateMachine.CurrentState != _stateMachine._attackingState)
            {
                isEnemyColliderHit = false;
            }

            return isEnemyColliderHit;
        }

        private void OnHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.IsInteractable)
            {
                DealDamage(enemy.transform.GetComponent<InteractableObjectBehavior>(), 
                    _characterModel.CharacterCommonSettings.CharacterDamage);
                hitBox.IsInteractable = false;
            }
        }

        private void OnHitEnded(ITrigger hitBox, Collider enemy)
        {

        }

        #endregion
    }
}

