using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class BackState
    {
        #region Fields

        public Dictionary<InputEventTypes, UnityAction> InputActions;

        private readonly GameContext _context;
        private readonly CharacterStateMachine _stateMachine;
        private readonly Services _services;
        private readonly CharacterModel _characterModel;
        private readonly InputModel _inputModel;
        private readonly CharacterAnimationController _animationController;

        private Vector3 _groundHit;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;

        private float _speedCountTime;
        private float _speedCountFrame;
        private float _currentHealth;
        private float _currentBattleIgnoreTime;

        #endregion


        #region ClassLifeCycle

        public BackState(GameContext context, CharacterStateMachine stateMachine)
        {
            InputActions = new Dictionary<InputEventTypes, UnityAction>();
            _context = context;
            _stateMachine = stateMachine;
            _services = Services.SharedInstance;
            _characterModel = _context.CharacterModel;
            _inputModel = _context.InputModel;
            _animationController = stateMachine.AnimationController;
        }

        #endregion


        #region Methods

        public void OnAwake()
        {
            _characterModel.PlayerBehavior.OnFilterHandler += OnFilterHandler;
            _characterModel.PlayerBehavior.OnTriggerEnterHandler += OnTriggerEnterHandler;
            _characterModel.PlayerBehavior.OnTriggerExitHandler += OnTriggerExitHandler;
            _characterModel.PlayerBehavior.SetTakeDamageEvent(TakeDamage);

            _stateMachine.OnBeforeStateChangeHangler += OnBeforeStateChange;
            _stateMachine.OnStateChangeHandler += OnStateChange;
            _stateMachine.OnAfterStateChangeHandler += OnAfterStateChange;

            LockCharAction.LockCharacterMovement += ExitTalkingState;
            StartDialogueData.StartDialog += SetTalkingState;
            //GlobalEventsModel.OnBossDie += StopTarget; TO REFACTOR

            SetRightWeapon(_services.InventoryService.GetAllWeapons()[0]);
            //SetLeftWeapon(_services.InventoryService.Feast);
            //SetRightWeapon(_services.InventoryService.Feast);
            _services.InventoryService.HideWepons(_characterModel);
            _services.TrapService.LoadTraps();
        }

        public void Initialize()
        {
            _speedCountFrame = _characterModel.CharacterCommonSettings.SpeedMeasureFrame;
            _speedCountTime = _speedCountFrame;
            _lastPosition = _characterModel.CharacterTransform.position;
            _currentPosition = _lastPosition;
            _currentHealth = _characterModel.CharacterCommonSettings.HealthPoints; // TO REFACTOR
            _currentBattleIgnoreTime = 0; // TO REFACTOR
        }

        public void Execute()
        {
            BattleIgnoreCheck();
            GroundCheck();
            MovementCheck();
            SpeedCheck();
            AnimationCheck();
            GetClosestEnemy();

            Debug.Log(_stateMachine.CurrentState);
        }

        public void OnExit() // TO REFACTOR
        {

        } 

        public void OnTearDown()
        {
            _characterModel.PlayerBehavior.OnFilterHandler -= OnFilterHandler;
            _characterModel.PlayerBehavior.OnTriggerEnterHandler -= OnTriggerEnterHandler;
            _characterModel.PlayerBehavior.OnTriggerExitHandler -= OnTriggerExitHandler;
            _characterModel.PlayerBehavior.DeleteTakeDamageEvent(TakeDamage);

            _stateMachine.OnBeforeStateChangeHangler -= OnBeforeStateChange;
            _stateMachine.OnStateChangeHandler -= OnStateChange;
            _stateMachine.OnAfterStateChangeHandler -= OnAfterStateChange;

            LockCharAction.LockCharacterMovement -= ExitTalkingState;
            StartDialogueData.StartDialog -= SetTalkingState;
            GlobalEventsModel.OnBossDie -= StopTarget;
        }


        #region StatesSwitchMethods

        public void SetDefaultIdleState()
        {
            _stateMachine.SetStartState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
        }

        public void SetDefaultMovementState()
        {
            _stateMachine.SetStartState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultMovement]);
        }

        public void SetJumpingState()
        {
            if (_characterModel.IsGrounded) _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Jumping]);
        }

        public void SetRollingState()
        {
            if (_characterModel.IsGrounded) _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Rolling]);
        }

        public void SetTargetRollingState()
        {
            if (_characterModel.IsGrounded) _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.RollingTarget]);
        }

        public void SetAttackingStateLeft()
        {
            if (_characterModel.IsGrounded)
            {
                if (_characterModel.IsWeaponInHands)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.AttackingFromLeft]);
                }
                else
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingWeapon]);
                }
            }
        }

        public void SetAttackingStateRight()
        {
            if (_characterModel.IsGrounded)
            {
                if (_characterModel.IsWeaponInHands)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.AttackingFromRight]);
                }
                else
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingWeapon]);
                }
            }
        }

        public void GetWeapon()
        {
            if (_characterModel.IsGrounded)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingWeapon]);
            }
        }

        public void RemoveWeapon()
        {
            if (_characterModel.IsGrounded)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.RemovingWeapon]);
            }
        }

        #endregion


        #region ITakeDamage

        private void TakeDamage(int id, Damage damage)
        {
            if (!_characterModel.IsDead && !_characterModel.IsDodging)
            {
                _currentHealth -= damage.PhysicalDamage;

                float stunProbability = UnityEngine.Random.Range(0f, 1f);

                if (_currentHealth <= 0)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dead]);
                }
                else if (damage.StunProbability > stunProbability)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Stunned]);
                }

                _characterModel.IsInBattleMode = true;

                Debug.Log("Player has: " + _currentHealth + " of HP");
            }
        }

        #endregion


        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            if (enemy != null && damage != null)
            {
                //enemy.TakeDamageEvent(damage);
                _context.NpcModels[GetParent(enemy.transform).GetInstanceID()].TakeDamage(damage);
            }
        }

        private GameObject GetParent(Transform instance)
        {
            while (instance.parent != null)
            {
                instance = instance.parent;
            }

            return instance.gameObject;
        }

        #endregion


        #region InFrameMethods

        private void AnimationCheck()
        {
            _animationController.UpdateAnimationParameters(_context.InputModel.InputTotalAxisX,
                _context.InputModel.InputTotalAxisY, _characterModel.CurrentSpeed, _characterModel.AnimationSpeed);
        }

        private void BattleIgnoreCheck()
        {
            if (_currentBattleIgnoreTime > 0) _currentBattleIgnoreTime -= Time.deltaTime;
        }

        private void GroundCheck()
        {
            if (_stateMachine.CurrentState != _stateMachine.CharacterStates[CharacterStatesEnum.Jumping])
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

        // TO REFACTOR
        private void FeetCheckIK()
        {
            if (_stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle])
            {
                // TODO
            }
        }

        private void MovementCheck()
        {
            _inputModel.IsInputMove = _context.InputModel.IsInputMove;
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

                if (_currentPosition == _lastPosition)
                {
                    _characterModel.CurrentSpeed = 0;
                }
                else
                {
                    _speedCountTime = _speedCountFrame;
                    _currentPosition = _characterModel.CharacterTransform.position;
                    _characterModel.CurrentSpeed = Mathf.Sqrt((_currentPosition - _lastPosition).sqrMagnitude) /
                        _speedCountFrame;                   
                }

                _lastPosition = _currentPosition;
            }

            _characterModel.VerticalSpeed = _characterModel.CharacterRigitbody.velocity.y;
        }

        private void CheckTrigger()
        {
            _characterModel.IsEnemyNear = _characterModel.EnemiesInTrigger.Count > 0;

            if (_stateMachine.CurrentState.Type != StateType.Sneaking)
            {
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
        }

        private void GetClosestEnemy()
        {
            if (_characterModel.IsEnemyNear &&
                    _stateMachine.CurrentState != _stateMachine.CharacterStates[CharacterStatesEnum.RollingTarget])
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

        #endregion


        #region ActionsMethods

        private void StopTarget()
        {
            _characterModel.ClosestEnemy = null;
            _characterModel.IsEnemyNear = false;
        }

        private void PlaceTrap1()
        {
            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.TrapPlace]);
            new InitializeTrapController(_context, Data.TrapData);
        }

        private void PlaceTrap2()
        {
            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.TrapPlace]);
            new InitializeTrapController(_context, Data.TrapData2);
        }

        private void OpenCloseTimeSkip()
        {
            if (!_characterModel.IsDead && _characterModel.IsGrounded)
            {
                if (_services.TimeSkipService.IsOpen)
                {
                    _services.TimeSkipService.CloseTimeSkipMenu();
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
                }
                else
                {
                    _services.TimeSkipService.OpenTimeSkipMenu();
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.SettingTimeSkip]);
                }
            }
        }

        private void ChangeSneakingMode()
        {
            if (_characterModel.IsSneaking)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.CrouchToDefault]);
            }
            else
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultToCrouch]);
            }
        }

        private void ChangeTargetMode()
        {
            if (_characterModel.IsInBattleMode)
            {
                if (_stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement])
                {
                    if (_inputModel.IsInputMove)
                    {
                        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.BattleMovement]);
                    }
                    else
                    {
                        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.BattleIdle]);
                    }
                }
                else
                {
                    if (_characterModel.IsEnemyNear)
                    {
                        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement]);
                    }
                }
            }
            else
            {
                if (_characterModel.IsEnemyNear && _characterModel.ClosestEnemy != null)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingWeapon],
                        _stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement]);
                }
            }
        }


        private void SetDodgingState()
        {
            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dodging]);
        }

        private void SetFallingState()
        {
            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Falling]);
        }

        private void ChangeDancingState()
        {
            if (_stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle])
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dancing]);
            }
            else if (_stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.Dancing])
            {
                _stateMachine.ReturnState();
            }
        }

        private void ExitBattle()
        {
            _currentBattleIgnoreTime = _characterModel.CharacterCommonSettings.BattleIgnoreTime;

            if (_inputModel.IsInputMove)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultMovement]);
            }
            else
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
            }

            _characterModel.IsInBattleMode = false;
        }

        private void SetTalkingState(Vector3 npcPosition)
        {
            if (_stateMachine.CurrentState.Type == StateType.Default)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Talking]);
            }
            else if (_stateMachine.CurrentState.Type == StateType.Battle)
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.RemovingWeapon],
                    _stateMachine.CharacterStates[CharacterStatesEnum.Talking]);
            }

            _characterModel.PlayerBehavior.SetLookAtTarget(npcPosition);
            _services.CameraService.SetActiveCamera(_services.CameraService.CharacterDialogCamera);
        }

        private void ExitTalkingState(bool isStartsTalking)
        {
            if (!isStartsTalking && _stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.Talking])
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
                _services.CameraService.SetActiveCamera(_services.CameraService.CharacterFreelookCamera);
                _characterModel.PlayerBehavior.SetLookAtTarget(Vector3.zero);
            }
        }

        #endregion


        #region OnStateChangeEventHandlers

        private void OnBeforeStateChange(CharacterBaseState currentState, CharacterBaseState newState)
        {

        }

        private void OnStateChange(CharacterBaseState previousState, CharacterBaseState currentState)
        {
            
        }

        private void OnAfterStateChange(CharacterBaseState currentState)
        {
            
        }

        #endregion


        #region SettingWeaponMethods

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
        private void SetLeftWeapon(WeaponItem newWeapon)
        {
            if (newWeapon.WeaponHandType == WeaponHandType.OneHanded)
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

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
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


        #region TriggersControlMethods

        private bool OnFilterHandler(Collider tagObject)
        {
            return !tagObject.isTrigger && tagObject.CompareTag(TagManager.ENEMY);
        }

        private void OnTriggerEnterHandler(ITrigger thisdObject, Collider enteredObject)
        {
            if (thisdObject.Type == InteractableObjectType.Player && !_characterModel.EnemiesInTrigger.
                Contains(enteredObject))
            {
                _characterModel.EnemiesInTrigger.Add(enteredObject);
                CheckTrigger();
            }
        }

        private void OnTriggerExitHandler(ITrigger thisdObject, Collider exitedObject)
        {
            if (thisdObject.Type == InteractableObjectType.Player && _characterModel.EnemiesInTrigger.
                Contains(exitedObject))
            {
                _characterModel.EnemiesInTrigger.Remove(exitedObject);
                CheckTrigger();
            }
        }

        private bool OnHitBoxFilter(Collider hitedObject)
        {
            bool isEnemyColliderHit = hitedObject.CompareTag(TagManager.ENEMY) || hitedObject.CompareTag(TagManager.HITBOX);

            if (hitedObject.isTrigger || !_stateMachine.CurrentState.IsAttacking)
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

                if (enemyBehavior.Type == InteractableObjectType.WeakHitBox)
                {
                    GlobalEventsModel.OnBossWeakPointHitted?.Invoke(enemy);
                }

                DealDamage(enemyBehavior, _services.AttackService.CountDamage(_characterModel.LeftHandWeapon,
                    _characterModel.CharacterStatsSettings, _context.NpcModels[GetParent(enemyBehavior.transform).
                        GetInstanceID()].GetStats().BaseStats));
                hitBox.IsInteractable = false;
            }
        }

        private void OnRightHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.IsInteractable)
            {
                InteractableObjectBehavior enemyBehavior = enemy.transform.GetComponent<InteractableObjectBehavior>();

                if (enemyBehavior.Type == InteractableObjectType.WeakHitBox)
                {
                    GlobalEventsModel.OnBossWeakPointHitted?.Invoke(enemy);
                }

                DealDamage(enemyBehavior, _services.AttackService.CountDamage(_characterModel.RightHandWeapon,
                    _characterModel.CharacterStatsSettings, _context.NpcModels[GetParent(enemyBehavior.transform).
                        GetInstanceID()].GetStats().BaseStats));
                hitBox.IsInteractable = false;
            }
        }

        #endregion


        #endregion
    }
}

