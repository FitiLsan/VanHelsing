using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using RootMotion.Dynamics;


namespace BeastHunter
{
    public sealed class BackState : IAwake, IUpdate, ITearDown, ITakeDamage, IDealDamage
    {
        #region Constants

        private const float CAMERA_LOW_SIDE_ANGLE = 45f;
        private const float CAMERA_HALF_SIDE_ANGLE = 90f;
        private const float CAMERA_BACK_SIDE_ANGLE = 225f;
        private const float CAMERA_BACK_ANGLE = 180f;
        private const float ANGULAR_VELOCITY_FADE_SPEED = 0.2f;
        private const float TIME_SCALE_FOR_WEAPON_WHEEL = 0.2f;
        private const float WEAPON_WHEEL_MAX_CYCLE_DISTANCE = 15f;

        private const string WEAPON_WHEEL_PANEL_NAME = "Panel";
        private const string WEAPON_WHEEL_CYCLE_NAME = "Cycle";

        #endregion


        #region Fields

        public Dictionary<InputEventTypes, UnityAction> InputActions;

        public Action OnMove;
        public Action OnStop;
        public Action OnAttack;
        public Action OnJump;
        public Action OnSneak;
        public Action OnBattleExit;
        public Action OnWeaponWheelOpen;
        public Action OnWeaponWheelClose;
        public Action OnWeaponChoose;
        public Action OnTimeSkipOpenClose;
        
        private readonly GameContext _context;
        private readonly CharacterStateMachine _stateMachine;
        private readonly Services _services;
        private readonly CharacterModel _characterModel;
        private readonly InputModel _inputModel;
        private readonly CharacterAnimationController _animationController;
        private readonly PuppetMaster _puppetController;

        private GameObject _weaponWheelUI;
        private WeaponCircle[] _weaponWheelItems;
        private WeaponCircle _closestWeaponOnWheel;
        private Transform _cameraTransform;
        private Transform _weaponWheelTransform;

        private Vector3 _groundHit;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;

        private float[] _speedArray;
        private float _currentHealth;
        private float _currentBattleIgnoreTime;
        private float _targetAngleVelocity;
        private float _targetSpeed;
        private float _speedChangeLag;
        private float _currentVelocity;
        private float _weaponWheelDistance;

        private bool _isWeaponWheelOpen;

        #endregion


        #region Properties

        private float TargetDirection { get; set; }
        private float CurrentDirecton { get; set; }
        private float AdditionalDirection { get; set; }
        private float CurrentAngle { get; set; }
        private float MovementSpeed { get; set; }

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
            _puppetController = _characterModel.PuppetMaster;
            _speedArray = new float[3] { 0, 0, 0, };

            _weaponWheelUI = GameObject.Instantiate(Data.WeaponWheelUI);
            _weaponWheelTransform = _weaponWheelUI.transform.
                Find(WEAPON_WHEEL_PANEL_NAME).Find(WEAPON_WHEEL_CYCLE_NAME).transform;
            _weaponWheelItems = _weaponWheelUI.transform.Find(WEAPON_WHEEL_PANEL_NAME).GetComponentsInChildren<WeaponCircle>();

            _cameraTransform = _services.CameraService.CharacterCamera.transform;
            CloseWeaponWheel();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _characterModel.PlayerBehavior.SetTakeDamageEvent(TakeDamage);

            _stateMachine.OnBeforeStateChangeHangler += OnBeforeStateChange;
            _stateMachine.OnStateChangeHandler += OnStateChange;
            _stateMachine.OnAfterStateChangeHandler += OnAfterStateChange;

            //LockCharAction.LockCharacterMovement += ExitTalkingState;
            //StartDialogueData.StartDialog += SetTalkingState;
            //GlobalEventsModel.OnBossDie += StopTarget; // TO REFACTOR

            //_services.InventoryService.HideWepons(_characterModel);
            _services.TrapService.LoadTraps();

            _services.EventManager.StartListening(InputEventTypes.MoveStart, OnMoveHandler);
            _services.EventManager.StartListening(InputEventTypes.MoveStop, OnStopHandler);
            _services.EventManager.StartListening(InputEventTypes.Sneak, OnSneakHandler);
            _services.EventManager.StartListening(InputEventTypes.TimeSkipMenu, OnTimeSkipOpenCloseHandler);
            _services.EventManager.StartListening(InputEventTypes.WeaponWheelOpen, OnWeaponWheelOpenHandler);
            _services.EventManager.StartListening(InputEventTypes.WeaponWheelClose, OnWeaponWheelCloseHandler);
            _services.EventManager.StartListening(InputEventTypes.AttackLeft, OnAttackHandler);

            OnWeaponWheelOpen += OpenWeaponWheel;
            OnWeaponWheelClose += CloseWeaponWheel;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            GroundCheck();
            MovementCheck();
            SpeedCheck();
            AnimationCheck();
            ControlWeaponWheel();
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _characterModel.PlayerBehavior.DeleteTakeDamageEvent(TakeDamage);

            _stateMachine.OnBeforeStateChangeHangler -= OnBeforeStateChange;
            _stateMachine.OnStateChangeHandler -= OnStateChange;
            _stateMachine.OnAfterStateChangeHandler -= OnAfterStateChange;

            //LockCharAction.LockCharacterMovement -= ExitTalkingState;
            //StartDialogueData.StartDialog -= SetTalkingState;
            //GlobalEventsModel.OnBossDie -= StopTarget;

            _services.EventManager.StopListening(InputEventTypes.MoveStart, OnMoveHandler);
            _services.EventManager.StopListening(InputEventTypes.MoveStop, OnStopHandler);
            _services.EventManager.StopListening(InputEventTypes.Sneak, OnSneakHandler);
            _services.EventManager.StopListening(InputEventTypes.TimeSkipMenu, OnTimeSkipOpenCloseHandler);
            _services.EventManager.StopListening(InputEventTypes.WeaponWheelOpen, OnWeaponWheelOpenHandler);
            _services.EventManager.StopListening(InputEventTypes.WeaponWheelClose, OnWeaponWheelCloseHandler);
            _services.EventManager.StopListening(InputEventTypes.AttackLeft, OnAttackHandler);

            OnWeaponWheelOpen -= OpenWeaponWheel;
            OnWeaponWheelClose -= CloseWeaponWheel;
        }

        #endregion


        #region ITakeDamage

        public void TakeDamage(Damage damage)
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

        public void TakeDamage(int id, Damage damage)
        {
            TakeDamage(damage);
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


        #region EnterExitMethods

        public void Initialize()
        {
            _lastPosition = _characterModel.CharacterTransform.position;
            _currentPosition = _lastPosition;
            _currentHealth = _characterModel.CharacterCommonSettings.HealthPoints; // TO REFACTOR
            _currentBattleIgnoreTime = 0; // TO REFACTOR
        }


        public void OnExit() // TO REFACTOR
        {
        }

        #endregion


        #region ActionHandlers

        public void OnMoveHandler()
        {
            OnMove?.Invoke();
        }

        public void OnStopHandler()
        {
            OnStop?.Invoke();
        }

        public void OnJumpHandler()
        {
            OnJump?.Invoke();
        }

        public void OnSneakHandler()
        {
            OnSneak?.Invoke();
        }

        public void OnWeaponWheelOpenHandler()
        {
            OnWeaponWheelOpen?.Invoke();
        }

        public void OnWeaponWheelCloseHandler()
        {
            OnWeaponWheelClose?.Invoke();
        }

        public void OnTimeSkipOpenCloseHandler()
        {
            OnTimeSkipOpenClose?.Invoke();
        }     

        public void OnAttackHandler()
        {
            OnAttack?.Invoke();
        }

        #endregion


        #region InFrameMethods

        private void AnimationCheck()
        {
            _animationController.UpdateAnimationParameters(_context.InputModel.InputTotalAxisX,
                _context.InputModel.InputTotalAxisY, _characterModel.CurrentSpeed, _characterModel.AnimationSpeed);
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

        private void MovementCheck()
        {
            _inputModel.IsInputMove = _context.InputModel.IsInputMove;
        }

        private void AddSpeed(float speed)
        {
            for(int index = 0; index < _speedArray.Length - 1; index++)
            {
                _speedArray[index] = _speedArray[index + 1];
            }

            _speedArray[_speedArray.Length - 1] = speed;
        }

        private float GetAverageSpeed()
        {
            return _speedArray.Sum() / _speedArray.Length;
        }

        private void SpeedCheck()
        {
            _currentPosition = _characterModel.CharacterTransform.position;

            if (_currentPosition == _lastPosition)
            {
                _characterModel.CurrentSpeed = 0;
            }
            else
            {
                AddSpeed(Mathf.Sqrt((_currentPosition - _lastPosition).sqrMagnitude) / Time.deltaTime);
                _characterModel.CurrentSpeed = GetAverageSpeed();
                _lastPosition = _currentPosition;
            }
        }

        #endregion


        #region ActionsMethods

        public void OpenWeaponWheel()
        {
            _weaponWheelUI.SetActive(true);
            _services.CameraService.LockFreeLookCamera();
            _weaponWheelTransform.localPosition = Vector3.zero;
            _closestWeaponOnWheel = null;
            _isWeaponWheelOpen = true;
        }

        public void CloseWeaponWheel()
        {
            _weaponWheelUI.SetActive(false);
            _services.CameraService.UnlockFreeLookCamera();
            _isWeaponWheelOpen = false;

            if (_closestWeaponOnWheel != null && _closestWeaponOnWheel.WeaponItem != null)
            {
                RemoveWeapon();
                GetWeapon(_closestWeaponOnWheel.WeaponItem, HumanBodyBones.RightHand);
            }
            else
            {
                RemoveWeapon();
            }
        }

        public void ControlWeaponWheel()
        {
            if (_isWeaponWheelOpen)
            {
                _weaponWheelTransform.localPosition = new Vector3(
                    Mathf.Clamp(_weaponWheelTransform.localPosition.x + _inputModel.MouseInputX, 
                    -WEAPON_WHEEL_MAX_CYCLE_DISTANCE, WEAPON_WHEEL_MAX_CYCLE_DISTANCE),
                    Mathf.Clamp(_weaponWheelTransform.localPosition.y + _inputModel.MouseInputY, 
                    -WEAPON_WHEEL_MAX_CYCLE_DISTANCE, WEAPON_WHEEL_MAX_CYCLE_DISTANCE),
                    _weaponWheelTransform.localPosition.z);

                float distanceFromCenter = Vector3.Distance(_weaponWheelTransform.localPosition, Vector3.zero);

                if (distanceFromCenter > WEAPON_WHEEL_MAX_CYCLE_DISTANCE)
                {
                    _weaponWheelTransform.localPosition *= WEAPON_WHEEL_MAX_CYCLE_DISTANCE / distanceFromCenter;
                }          

                if(distanceFromCenter > WEAPON_WHEEL_MAX_CYCLE_DISTANCE / 2)
                {
                    _closestWeaponOnWheel = GetClosestWeaponItemInWheel();
                    _closestWeaponOnWheel.Activate();
                    DisableAllWeaponItemsInWheel(_closestWeaponOnWheel);
                }
                else
                {
                    DisableAllWeaponItemsInWheel();
                    _closestWeaponOnWheel = null;
                }
            }
        }

        public WeaponCircle GetClosestWeaponItemInWheel()
        {
            float minimalDistance = Mathf.Infinity;
            float currentDistance = minimalDistance;
            WeaponCircle weaponItem = null;

            for(int item = 0; item < _weaponWheelItems.Length; item++)
            {
                currentDistance = Vector3.Distance(_weaponWheelItems[item].AnchorPosition, _weaponWheelTransform.localPosition);

                if (currentDistance < minimalDistance)
                {
                    minimalDistance = currentDistance;
                    weaponItem = _weaponWheelItems[item];
                }
            }

            return weaponItem;
        }

        public void DisableAllWeaponItemsInWheel(WeaponCircle exceptionItem = null)
        {
            foreach (var item in _weaponWheelItems)
            {
                if(item != exceptionItem) item.Deactivate();
            }
        }

        public void RemoveWeapon()
        {
            if(_characterModel.CurrentWeapon != null)
            {
                _characterModel.PuppetMaster.propMuscles[0].currentProp = null;
                GameObject.Destroy(_characterModel.CurrentWeapon, 0.1f);
            }

            _characterModel.CurrentWeaponItem = null;
        }

        public void GetWeapon(WeaponItem weapon, HumanBodyBones hand)
        {
            GameObject newWeapon = GameObject.Instantiate(weapon.WeaponPrefab);

            _characterModel.CurrentWeapon = newWeapon;
            _characterModel.CurrentWeaponItem = weapon;
            _characterModel.PuppetMaster.propMuscles[0].currentProp = newWeapon.GetComponent<PuppetMasterProp>();
        }

        #endregion


        #region OnStateChangeEventHandlers

        private void OnBeforeStateChange(CharacterBaseState currentState, CharacterBaseState newState)
        {
            // TODO
        }

        private void OnStateChange(CharacterBaseState previousState, CharacterBaseState currentState)
        {
            // TODO
        }

        private void OnAfterStateChange(CharacterBaseState currentState)
        {
            // TODO
        }

        #endregion


        #region MovementControlMethods

        public void StopCharacter()
        {
            MovementSpeed = 0f;
            _targetAngleVelocity = 0f;
            _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle, 0);
        }

        public void MoveCharacter()
        {
            if (_characterModel.IsGrounded)
            {
                _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, MovementSpeed);
            }
        }

        public void RotateCharacter(bool hasCameraControl)
        {
            if (_characterModel.IsGrounded)
            {
                if (hasCameraControl)
                {
                    switch (_inputModel.InputTotalAxisY)
                    {
                        case 1:
                            AdditionalDirection = CAMERA_LOW_SIDE_ANGLE * _inputModel.InputTotalAxisX;
                            break;
                        case -1:
                            switch (_inputModel.InputTotalAxisX)
                            {
                                case 0:
                                    AdditionalDirection = CAMERA_BACK_ANGLE;
                                    break;
                                default:
                                    AdditionalDirection = -CAMERA_BACK_SIDE_ANGLE * _inputModel.InputTotalAxisX;
                                    break;
                            }
                            break;
                        case 0:
                            AdditionalDirection = CAMERA_HALF_SIDE_ANGLE * _inputModel.InputTotalAxisX;
                            break;
                        default:
                            break;
                    }

                    CurrentDirecton = _characterModel.CharacterTransform.localEulerAngles.y;
                    TargetDirection = _cameraTransform.localEulerAngles.y + AdditionalDirection;
                    CurrentAngle = Mathf.SmoothDampAngle(CurrentDirecton, TargetDirection, ref _targetAngleVelocity,
                    _characterModel.CharacterCommonSettings.DirectionChangeLag);
                }
                else
                {
                    _targetAngleVelocity = Mathf.SmoothStep(_targetAngleVelocity, 0, ANGULAR_VELOCITY_FADE_SPEED);
                }
                
                _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle, 
                    -_targetAngleVelocity * Time.fixedDeltaTime);
            }
        }

        public void CountSpeedDefault()
        {
            if (_inputModel.IsInputMove)
            {
                if (_inputModel.IsInputRun)
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.RunSpeed;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.WalkSpeed;
                }
            }
            else
            {
                _targetSpeed = 0;
            }

            if (_characterModel.CurrentSpeed < _targetSpeed)
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.AccelerationLag;
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.DecelerationLag;
            }

            MovementSpeed = Mathf.SmoothDamp(_characterModel.CurrentSpeed, _targetSpeed, ref _currentVelocity,
                _speedChangeLag);
        }

        public void CountSpeedSneaking()
        {
            if (_inputModel.IsInputMove)
            {
                if (_inputModel.IsInputRun)
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.SneakRunSpeed;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.SneakWalkSpeed;
                }
            }
            else
            {
                _targetSpeed = 0;
            }

            if (_characterModel.CurrentSpeed < _targetSpeed)
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.AccelerationLag;
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.DecelerationLag;
            }

            MovementSpeed = Mathf.SmoothDamp(_characterModel.CurrentSpeed, _targetSpeed, ref _currentVelocity,
                _speedChangeLag);
        }

        #endregion


        #region GarbageToRefactor

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
        //private void SetLeftWeapon(WeaponItem newWeapon)
        //{
        //    if (newWeapon.WeaponHandType == WeaponHandType.OneHanded)
        //    {
        //        if (_characterModel.LeftWeaponBehavior != null)
        //        {
        //            _characterModel.LeftWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
        //            _characterModel.LeftWeaponBehavior.OnTriggerEnterHandler -= OnLeftHitBoxHit;
        //        }

        //        _services.InventoryService.SetWeaponInLeftHand(_characterModel, newWeapon, out WeaponHitBoxBehavior newBehaviour);
        //        _characterModel.LeftWeaponBehavior = newBehaviour;

        //        _characterModel.LeftWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
        //        _characterModel.LeftWeaponBehavior.OnTriggerEnterHandler += OnLeftHitBoxHit;
        //    }
        //    else
        //    {
        //        // if two handed
        //    }
        //}

        ////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
        //private void SetRightWeapon(WeaponItem newWeapon)
        //{
        //    if (newWeapon.WeaponHandType == WeaponHandType.OneHanded)
        //    {
        //        if (_characterModel.RightWeaponBehavior != null)
        //        {
        //            _characterModel.RightWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
        //            _characterModel.RightWeaponBehavior.OnTriggerEnterHandler -= OnRightHitBoxHit;
        //        }

        //        _services.InventoryService.SetWeaponInRightHand(_characterModel, newWeapon, out WeaponHitBoxBehavior newBehaviour);
        //        _characterModel.RightWeaponBehavior = newBehaviour;

        //        _characterModel.RightWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
        //        _characterModel.RightWeaponBehavior.OnTriggerEnterHandler += OnRightHitBoxHit;
        //    }
        //    else
        //    {
        //        if (_characterModel.RightWeaponBehavior != null)
        //        {
        //            _characterModel.RightWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
        //            _characterModel.RightWeaponBehavior.OnTriggerEnterHandler -= OnRightHitBoxHit;
        //        }
        //        if (_characterModel.LeftWeaponBehavior != null)
        //        {
        //            _characterModel.LeftWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
        //            _characterModel.LeftWeaponBehavior.OnTriggerEnterHandler -= OnLeftHitBoxHit;
        //        }

        //        _services.InventoryService.SetWeaponInRightHand(_characterModel, newWeapon, out WeaponHitBoxBehavior newBehaviour);
        //        _characterModel.RightWeaponBehavior = newBehaviour;
        //        _characterModel.LeftWeaponBehavior = newBehaviour;

        //        _characterModel.RightWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
        //        _characterModel.RightWeaponBehavior.OnTriggerEnterHandler += OnRightHitBoxHit;
        //        _characterModel.LeftWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
        //        _characterModel.LeftWeaponBehavior.OnTriggerEnterHandler += OnLeftHitBoxHit;
        //    }
        //}

        //public void SetAttackingStateLeft()
        //{
        //    if (_characterModel.IsGrounded)
        //    {
        //        if (_characterModel.IsWeaponInHands)
        //        {
        //            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.AttackingFromLeft]);
        //        }
        //        else
        //        {
        //            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingWeapon]);
        //        }
        //    }
        //}

        //public void SetAttackingStateRight()
        //{
        //    if (_characterModel.IsGrounded)
        //    {
        //        if (_characterModel.IsWeaponInHands)
        //        {
        //            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.AttackingFromRight]);
        //        }
        //        else
        //        {
        //            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingWeapon]);
        //        }
        //    }
        //}

        //public void GetWeapon()
        //{
        //    if (_characterModel.IsGrounded)
        //    {
        //        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingWeapon]);
        //    }
        //}

        //public void RemoveWeapon()
        //{
        //    if (_characterModel.IsGrounded)
        //    {
        //        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.RemovingWeapon]);
        //    }
        //}

        //private void CheckTrigger()
        //{
        //    _characterModel.IsEnemyNear = _characterModel.EnemiesInTrigger.Count > 0;

        //    if (_stateMachine.CurrentState.Type != StateType.Sneaking)
        //    {
        //        if (_characterModel.IsEnemyNear && _currentBattleIgnoreTime <= 0)
        //        {
        //            _characterModel.IsInBattleMode = true;
        //        }
        //        else
        //        {
        //            _characterModel.IsInBattleMode = false;
        //            _characterModel.ClosestEnemy = null;
        //        }
        //    }
        //}

        //private void GetClosestEnemy()
        //{
        //    if (_characterModel.IsEnemyNear &&
        //            _stateMachine.CurrentState != _stateMachine.CharacterStates[CharacterStatesEnum.RollingTarget])
        //    {
        //        Collider enemy = null;

        //        float minimalDistance = _characterModel.CharacterCommonSettings.SphereColliderRadius;
        //        float countDistance = minimalDistance;

        //        foreach (var collider in _characterModel.EnemiesInTrigger)
        //        {
        //            countDistance = Mathf.Sqrt((_characterModel.CharacterTransform.position -
        //                collider.transform.position).sqrMagnitude);

        //            if (countDistance < minimalDistance)
        //            {
        //                minimalDistance = countDistance;
        //                enemy = collider;
        //            }
        //        }

        //        _characterModel.ClosestEnemy = enemy;
        //    }
        //}

        //private bool OnFilterHandler(Collider tagObject)
        //{
        //    return !tagObject.isTrigger && tagObject.CompareTag(TagManager.ENEMY);
        //}

        //private void OnTriggerEnterHandler(ITrigger thisdObject, Collider enteredObject)
        //{
        //    if (thisdObject.Type == InteractableObjectType.Player && !_characterModel.EnemiesInTrigger.
        //        Contains(enteredObject))
        //    {
        //        _characterModel.EnemiesInTrigger.Add(enteredObject);
        //        CheckTrigger();
        //    }
        //}

        //private void OnTriggerExitHandler(ITrigger thisdObject, Collider exitedObject)
        //{
        //    if (thisdObject.Type == InteractableObjectType.Player && _characterModel.EnemiesInTrigger.
        //        Contains(exitedObject))
        //    {
        //        _characterModel.EnemiesInTrigger.Remove(exitedObject);
        //        CheckTrigger();
        //    }
        //}

        //private bool OnHitBoxFilter(Collider hitedObject)
        //{
        //    bool isEnemyColliderHit = hitedObject.CompareTag(TagManager.ENEMY) || hitedObject.CompareTag(TagManager.HITBOX);

        //    if (hitedObject.isTrigger || !_stateMachine.CurrentState.IsAttacking)
        //    {
        //        isEnemyColliderHit = false;
        //    }

        //    return isEnemyColliderHit;
        //}

        //private void OnLeftHitBoxHit(ITrigger hitBox, Collider enemy)
        //{
        //    if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.IsInteractable)
        //    {
        //        InteractableObjectBehavior enemyBehavior = enemy.transform.GetComponent<InteractableObjectBehavior>();

        //        if (enemyBehavior.Type == InteractableObjectType.WeakHitBox)
        //        {
        //            GlobalEventsModel.OnBossWeakPointHitted?.Invoke(enemy);
        //        }

        //        DealDamage(enemyBehavior, _services.AttackService.CountDamage(_characterModel.LeftHandWeapon,
        //            _characterModel.CharacterStatsSettings, _context.NpcModels[GetParent(enemyBehavior.transform).
        //                GetInstanceID()].GetStats().BaseStats));
        //        hitBox.IsInteractable = false;
        //    }
        //}

        //private void OnRightHitBoxHit(ITrigger hitBox, Collider enemy)
        //{
        //    if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.IsInteractable)
        //    {
        //        InteractableObjectBehavior enemyBehavior = enemy.transform.GetComponent<InteractableObjectBehavior>();

        //        if (enemyBehavior.Type == InteractableObjectType.WeakHitBox)
        //        {
        //            GlobalEventsModel.OnBossWeakPointHitted?.Invoke(enemy);
        //        }

        //        DealDamage(enemyBehavior, _services.AttackService.CountDamage(_characterModel.RightHandWeapon,
        //            _characterModel.CharacterStatsSettings, _context.NpcModels[GetParent(enemyBehavior.transform).
        //                GetInstanceID()].GetStats().BaseStats));
        //        hitBox.IsInteractable = false;
        //    }
        //}

        //private void ChangeTargetMode()
        //{
        //    if (_characterModel.IsInBattleMode)
        //    {
        //        if (_stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement])
        //        {
        //            if (_inputModel.IsInputMove)
        //            {
        //                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.BattleMovement]);
        //            }
        //            else
        //            {
        //                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.BattleIdle]);
        //            }
        //        }
        //        else
        //        {
        //            if (_characterModel.IsEnemyNear)
        //            {
        //                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement]);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (_characterModel.IsEnemyNear && _characterModel.ClosestEnemy != null)
        //        {
        //            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingWeapon],
        //                _stateMachine.CharacterStates[CharacterStatesEnum.BattleTargetMovement]);
        //        }
        //    }
        //}


        //private void SetDodgingState()
        //{
        //    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dodging]);
        //}

        //private void SetFallingState()
        //{
        //    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Falling]);
        //}

        //private void ChangeDancingState()
        //{
        //    if (_stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle])
        //    {
        //        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dancing]);
        //    }
        //    else if (_stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.Dancing])
        //    {
        //        _stateMachine.ReturnState();
        //    }
        //}

        //private void ExitBattle()
        //{
        //    _currentBattleIgnoreTime = _characterModel.CharacterCommonSettings.BattleIgnoreTime;

        //    if (_inputModel.IsInputMove)
        //    {
        //        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultMovement]);
        //    }
        //    else
        //    {
        //        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
        //    }

        //    _characterModel.IsInBattleMode = false;
        //}

        //private void SetTalkingState(Vector3 npcPosition)
        //{
        //    if (_stateMachine.CurrentState.Type == StateType.Default)
        //    {
        //        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Talking]);
        //    }
        //    else if (_stateMachine.CurrentState.Type == StateType.Battle)
        //    {
        //        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.RemovingWeapon],
        //            _stateMachine.CharacterStates[CharacterStatesEnum.Talking]);
        //    }

        //    _characterModel.PlayerBehavior.SetLookAtTarget(npcPosition);
        //    _services.CameraService.SetActiveCamera(_services.CameraService.CharacterDialogCamera);
        //}

        //private void ExitTalkingState(bool isStartsTalking)
        //{
        //    if (!isStartsTalking && _stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.Talking])
        //    {
        //        _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
        //        _services.CameraService.SetActiveCamera(_services.CameraService.CharacterFreelookCamera);
        //        _characterModel.PlayerBehavior.SetLookAtTarget(Vector3.zero);
        //    }
        //}

        //private void BattleIgnoreCheck()
        //{
        //    if (_currentBattleIgnoreTime > 0) _currentBattleIgnoreTime -= Time.deltaTime;
        //}

        //private void StopTarget()
        //{
        //    _characterModel.ClosestEnemy = null;
        //    _characterModel.IsEnemyNear = false;
        //}

        //private void PlaceTrap1()
        //{
        //    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.TrapPlace]);
        //    new InitializeTrapController(_context, Data.TrapData);
        //}

        //private void PlaceTrap2()
        //{
        //    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.TrapPlace]);
        //    new InitializeTrapController(_context, Data.TrapData2);
        //}

        //private void OpenCloseTimeSkip()
        //{
        //    if (!_characterModel.IsDead && _characterModel.IsGrounded)
        //    {
        //        if (_services.TimeSkipService.IsOpen)
        //        {
        //            _services.TimeSkipService.CloseTimeSkipMenu();
        //            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.DefaultIdle]);
        //        }
        //        else
        //        {
        //            _services.TimeSkipService.OpenTimeSkipMenu();
        //            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.SettingTimeSkip]);
        //        }
        //    }
        //}

        #endregion
    }
}

