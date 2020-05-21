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
            _inputModel.OnAttackLeft += SetAttackingLeftState;
            _inputModel.OnAttackRight += SetAttackingRightState;
            _inputModel.OnDance += ChangeDancingState;

            _characterModel.PlayerBehavior.OnFilterHandler += OnFilterHandler;
            _characterModel.PlayerBehavior.OnTriggerEnterHandler += OnTriggerEnterHandler;
            _characterModel.PlayerBehavior.OnTriggerExitHandler += OnTriggerExitHandler;
            _characterModel.PlayerBehavior.OnTakeDamageHandler += TakeDamage;

            _stateMachine.OnStateChangeHandler += OnStateChange;
            LockCharAction.LockCharacterMovement += SetTalkingState;

            SetRightWeapon(_services.InventoryService.GetAllWeapons()[0]);
            //SetLeftWeapon(_services.InventoryService.Feast);
            //SetRightWeapon(_services.InventoryService.Feast);
            _services.InventoryService.HideWepons(_characterModel);
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
                GetClosestEnemy();
                _stateMachine.CurrentState.Execute();
            }
            Debug.Log(_stateMachine.CurrentState + " enemy neaar:" + _characterModel.IsEnemyNear + " battle:" + _characterModel.IsInBattleMode +
                " weapon hands: " + _characterModel.IsWeaponInHands);
            _animationController.UpdateAnimationParameters(_inputModel.inputStruct._inputTotalAxisX, 
                _inputModel.inputStruct._inputTotalAxisY, _characterModel.CurrentSpeed, _characterModel.AnimationSpeed);
        }

        #endregion

        #region ITearDownController

        public void TearDown()
        {
            _inputModel.OnJump -= SetJumpingState;
            _inputModel.OnBattleExit -= ExitBattle;
            _inputModel.OnTargetLock -= ChangeTargetMode;
            _inputModel.OnAttackLeft -= SetAttackingLeftState;
            _inputModel.OnAttackRight -= SetAttackingRightState;
            _inputModel.OnDance -= ChangeDancingState;
            
            _characterModel.PlayerBehavior.OnFilterHandler -= OnFilterHandler;
            _characterModel.PlayerBehavior.OnTriggerEnterHandler -= OnTriggerEnterHandler;
            _characterModel.PlayerBehavior.OnTriggerExitHandler -= OnTriggerExitHandler;
            _characterModel.PlayerBehavior.OnTakeDamageHandler -= TakeDamage;

            _stateMachine.OnStateChangeHandler -= OnStateChange;
            _stateMachine.TearDownStates();
            LockCharAction.LockCharacterMovement -= SetTalkingState;
        }

        #endregion


        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            if (enemy != null && damage != null)
            {
                enemy.OnTakeDamageHandler(damage);
            }
        }

        #endregion


        #region ITakeDamage

        private void TakeDamage(Damage damage)
        {
            if (_stateMachine.CurrentState != _stateMachine._deadState)
            {
                _currentHealth -= damage.PhysicalDamage;
                Debug.Log("player got " + damage.PhysicalDamage + " physical damage");
                float stunProbability = Random.Range(0f, 1f);

                if (_currentHealth <= 0)
                {
                    _stateMachine.SetStateAnyway(_stateMachine._deadState);
                }
                else if (damage.StunProbability > stunProbability)
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
                _characterModel.ClosestEnemy = null;
            }
        }

        private void GetClosestEnemy()
        {
            if(_characterModel.IsEnemyNear && _stateMachine.CurrentState != _stateMachine._battleTargetMovementState &&
                _stateMachine.CurrentState != _stateMachine._rollingTargetState)
            {
                Collider enemy = null;

                float minimalDistance = _characterModel.CharacterCommonSettings.SphereColliderRadius;
                float countDistance = minimalDistance;

                foreach (var collider in _characterModel.EnemiesInTrigger)
                {
                    countDistance = Mathf.Sqrt((_characterModel.CharacterTransform.position -
                        collider.transform.position).sqrMagnitude);

                    if (countDistance < minimalDistance)
                    {
                        minimalDistance = countDistance;
                        enemy = collider;
                    }
                }

                _characterModel.ClosestEnemy = enemy;
            } 
        }

        private void SetLeftWeapon(WeaponItem newWeapon)
        {
            if(newWeapon.WeaponHandType == WeaponHandType.OneHanded)
            {
                if (_characterModel.LeftWeaponBehavior != null)
                {
                    _characterModel.LeftWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
                    _characterModel.LeftWeaponBehavior.OnTriggerEnterHandler -= OnLeftHitBoxHit;
                }

                _services.InventoryService.SetWeaponInLeftHand(_characterModel, newWeapon, out WeaponHitBoxBehavior newBehaviour);
                _characterModel.LeftWeaponBehavior = newBehaviour;

                _characterModel.LeftWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
                _characterModel.LeftWeaponBehavior.OnTriggerEnterHandler += OnLeftHitBoxHit;
            }
            else
            {
                // if two handed
            }
        }

        private void SetRightWeapon(WeaponItem newWeapon)
        {
            if (newWeapon.WeaponHandType == WeaponHandType.OneHanded)
            {
                if (_characterModel.RightWeaponBehavior != null)
                {
                    _characterModel.RightWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
                    _characterModel.RightWeaponBehavior.OnTriggerEnterHandler -= OnRightHitBoxHit;
                }

                _services.InventoryService.SetWeaponInRightHand(_characterModel, newWeapon, out WeaponHitBoxBehavior newBehaviour);
                _characterModel.RightWeaponBehavior = newBehaviour;

                _characterModel.RightWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
                _characterModel.RightWeaponBehavior.OnTriggerEnterHandler += OnRightHitBoxHit;
            }
            else
            {
                if (_characterModel.RightWeaponBehavior != null)
                {
                    _characterModel.RightWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
                    _characterModel.RightWeaponBehavior.OnTriggerEnterHandler -= OnRightHitBoxHit;
                }
                if (_characterModel.LeftWeaponBehavior != null)
                {
                    _characterModel.LeftWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
                    _characterModel.LeftWeaponBehavior.OnTriggerEnterHandler -= OnLeftHitBoxHit;
                }

                _services.InventoryService.SetWeaponInRightHand(_characterModel, newWeapon, out WeaponHitBoxBehavior newBehaviour);
                _characterModel.RightWeaponBehavior = newBehaviour;
                _characterModel.LeftWeaponBehavior = newBehaviour;

                _characterModel.RightWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
                _characterModel.RightWeaponBehavior.OnTriggerEnterHandler += OnRightHitBoxHit;
                _characterModel.LeftWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
                _characterModel.LeftWeaponBehavior.OnTriggerEnterHandler += OnLeftHitBoxHit;
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

        private void SetAttackingLeftState()
        {
            if (_characterModel.IsGrounded &&_stateMachine.CurrentState != _stateMachine._attackingLeftState &&
                _stateMachine.CurrentState != _stateMachine._attackingRightState &&
                    _stateMachine.CurrentState != _stateMachine._talkingState)
            {
                _characterModel.IsInBattleMode = true;
                _stateMachine.SetStateOverride(_stateMachine._attackingLeftState);
            }
        }

        private void SetAttackingRightState()
        {
            if (_characterModel.IsGrounded && _stateMachine.CurrentState != _stateMachine._attackingLeftState &&
                _stateMachine.CurrentState != _stateMachine._attackingRightState &&
                    _stateMachine.CurrentState != _stateMachine._talkingState)
            {
                _characterModel.IsInBattleMode = true;
                _stateMachine.SetStateOverride(_stateMachine._attackingRightState);
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

        private void SetTalkingState(bool isStartsTalking)
        {
            if (isStartsTalking)
            {
                if (_stateMachine.CurrentState.Type == StateType.Default)
                {
                    _stateMachine.SetState(_stateMachine._talkingState);
                }
                else if (_stateMachine.CurrentState.Type == StateType.Battle)
                {
                    _stateMachine.SetState(_stateMachine._removingWeaponState, _stateMachine._talkingState);
                }

                _services.CameraService.SetActiveCamera(_services.CameraService.CharacterDialogCamera);
            }
            else
            {
                _stateMachine.SetStateAnyway(_stateMachine._defaultIdleState);
                _services.CameraService.SetActiveCamera(_services.CameraService.CharacterFreelookCamera);
            }
        }

        private void OnStateChange(CharacterBaseState previousState, CharacterBaseState currentState)
        {
            if(_characterModel.TargetMarkTransform.GetComponentInParent<Transform>().tag == 
                _characterModel.CharacterCommonSettings.InstanceTag && currentState == _stateMachine._battleTargetMovementState)
            {
                _characterModel.TargetMark.transform.SetParent(_characterModel.ClosestEnemy.transform);
                _characterModel.TargetMarkTransform.localPosition = Vector3.zero + Vector3.up * 
                    _characterModel.CharacterCommonSettings.TargetMarkHeight;
                _characterModel.TargetMark.SetActive(true);
            }
            else if ((previousState == _stateMachine._battleTargetMovementState || previousState == _stateMachine._rollingTargetState) &&
                    currentState != _stateMachine._battleTargetMovementState && currentState != _stateMachine._rollingTargetState &&
                        currentState != _stateMachine._attackingLeftState)
            {
                _characterModel.TargetMark.transform.SetParent(_characterModel.CharacterTransform);
                _characterModel.TargetMarkTransform.localPosition = Vector3.zero;
                _characterModel.TargetMark.SetActive(false);
            }

            if(currentState.Type == StateType.Battle && !_characterModel.IsWeaponInHands)
            {
                _characterModel.CharacterSphereCollider.radius *= _characterModel.CharacterCommonSettings.SphereColliderRadiusIncrease;
                _stateMachine.SetStateOverride(_stateMachine._gettingWeaponState);         
            }

            if(currentState.Type != StateType.Battle && _characterModel.IsWeaponInHands)
            {
                _characterModel.CharacterSphereCollider.radius = _characterModel.CharacterCommonSettings.SphereColliderRadius;
                _stateMachine.SetStateOverride(_stateMachine._removingWeaponState);
            }

            if(!previousState.IsTargeting && currentState.IsTargeting && _stateMachine.CurrentState != _stateMachine._attackingLeftState &&
                _stateMachine.CurrentState != _stateMachine._attackingRightState)
            {
                _services.CameraService.SetActiveCamera(_services.CameraService.CharacterTargetCamera);
            }
            else if(previousState.IsTargeting && !currentState.IsTargeting && _stateMachine.CurrentState != _stateMachine._attackingLeftState &&
                _stateMachine.CurrentState != _stateMachine._attackingRightState)
            {
                _services.CameraService.SetActiveCamera(_services.CameraService.CharacterFreelookCamera);
            }
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
            bool isEnemyColliderHit = hitedObject.CompareTag(TagManager.ENEMY) || hitedObject.CompareTag(TagManager.RABBIT);
            
            if (hitedObject.isTrigger || (_stateMachine.CurrentState != _stateMachine._attackingLeftState && 
                _stateMachine.CurrentState != _stateMachine._attackingRightState))
            {
                isEnemyColliderHit = false;
            }

            return isEnemyColliderHit;
        }

        private void OnLeftHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.IsInteractable)
            {
                InteractableObjectBehavior enemyBehavior = enemy.transform.GetComponent<InteractableObjectBehavior>();

                DealDamage(enemyBehavior, _services.AttackService.CountDamage(_characterModel.LeftHandWeapon, 
                    _characterModel.CharacterStatsSettings, enemyBehavior.Stats));
                hitBox.IsInteractable = false;
            }
        }

        private void OnRightHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.IsInteractable)
            {
                InteractableObjectBehavior enemyBehavior = enemy.transform.GetComponent<InteractableObjectBehavior>();

                DealDamage(enemyBehavior, _services.AttackService.CountDamage(_characterModel.RightHandWeapon,
                    _characterModel.CharacterStatsSettings, enemyBehavior.Stats));
                hitBox.IsInteractable = false;
            }
        }

        #endregion
    }
}

