using UnityEngine;
using UnityEngine.UI;
using System;
using RootMotion.Dynamics;
using UniRx;
using Extensions;


namespace BeastHunter
{
    public sealed class BackState : IAwake, IUpdate, ITearDown
    {
        #region Constants

        private const float CAMERA_LOW_SIDE_ANGLE = 45f;
        private const float CAMERA_HALF_SIDE_ANGLE = 90f;
        private const float CAMERA_BACK_SIDE_ANGLE = 225f;
        private const float CAMERA_BACK_ANGLE = 180f;

        private const float ANGULAR_VELOCITY_FADE_SPEED = 0.2f;
        private const float ANGULAR_MOVE_SPEED_REDUCTION_INDEX = 0.7f;

        private const float WEAPON_WHEEL_MAX_CYCLE_DISTANCE = 15f;
        private const float WEAPON_WHEEL_DISTANCE_TO_SET_WEAPON = 7.5f;
        private const float WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_ALFA = 0.3f;
        private const float WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_ALFA = 0.4f;
        private const float WEAPON_WHEEL_PARENT_IMAGE_DEDICATED_ALFA = 0.6f;
        private const float WEAPON_WHEEL_CHILD_IMAGE_DEDICATED_ALFA = 0.7f;
        private const float WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE = 0.7f;
        private const float WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_SCALE = 0.75f;
        private const float WEAPON_WHEEL_IMAGE_DEDICATED_SCALE = 0.85f;

        private const string WEAPON_WHEEL_PANEL_NAME = "Panel";
        private const string WEAPON_WHEEL_CYCLE_NAME = "Cycle";

        #endregion


        #region Fields

        public Action OnWeaponChange;
        public Action OnTrapPlace;
        public Action OnHealthChange;

        private readonly GameContext _context;
        private readonly CharacterStateMachine _stateMachine;
        private readonly Services _services;
        private readonly CharacterModel _characterModel;
        private readonly InputModel _inputModel;
        private readonly PuppetMaster _puppetController;
        private readonly CharacterSpeedCounter _standartSpeedCounter;
        private readonly CharacterSpeedCounter _sneakingSpeedCounter;
        private readonly CharacterSpeedCounter _battleSpeedCounter;
        private readonly CharacterSpeedCounter _aimingSpeedCounter;

        private GameObject _weaponWheelUI;
        private GameObject _buttonsInfoUI;

        private WeaponCircle[] _weaponWheelItems;
        private WeaponCircle _closestWeaponOnWheel;

        private Transform _cameraTransform;
        private Transform _weaponWheelTransform;

        private Vector3 _groundHit;
        private Vector3 _lastPosition;
        private Vector3 _currentPosition;

        private Text _weaponWheelText;
        private CharacterSpeedCounter _activeSpeedCounter;
        private PlayerHealthBarModel _playerHealthBarModel;

        private CharacterAnimationEventTypes _lastAnimationEventType;

        private float _curretSpeed;
        private float _targetAngleVelocity;
        private float _targetSpeed;
        private float _speedChangeLag;
        private float _currentVelocity;
        private float _weaponWheelDistance;

        private bool _isWeaponWheelOpen;
        private bool _isCurrentWeaponWithProjectile;


        private EnemyHealthBarModel _enemyHealthBarModel;
        private EnemyModel _targetEnemy;

        #endregion


        #region Properties

        private float TargetDirection { get; set; }
        private float CurrentDirecton { get; set; }
        private float AdditionalDirection { get; set; }
        private float CurrentAngle { get; set; }

        #endregion


        #region ClassLifeCycle

        public BackState(GameContext context, CharacterStateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
            _services = Services.SharedInstance;
            _characterModel = _context.CharacterModel;
            _inputModel = _context.InputModel;
            _puppetController = _characterModel.PuppetMaster;
            
            _weaponWheelUI = GameObject.Instantiate(Data.UIElementsData.WeaponWheelPrefab);
            _weaponWheelTransform = _weaponWheelUI.transform.
                Find(WEAPON_WHEEL_PANEL_NAME).Find(WEAPON_WHEEL_CYCLE_NAME).transform;
            _weaponWheelItems = _weaponWheelUI.transform.Find(WEAPON_WHEEL_PANEL_NAME).
                GetComponentsInChildren<WeaponCircle>();
            _weaponWheelText = _weaponWheelUI.transform.GetComponentInChildren<Text>();
            InitAllWeaponItemsOnWheel();

            _standartSpeedCounter = new CharacterSpeedCounter(_characterModel.CharacterCommonSettings.WalkSpeed,
                _characterModel.CharacterCommonSettings.RunSpeed, _characterModel.CharacterCommonSettings.AccelerationLag,
                    _characterModel.CharacterCommonSettings.DecelerationLag,
                        _characterModel.CharacterCommonSettings.MinimalSpeed);
            _sneakingSpeedCounter = new CharacterSpeedCounter(_characterModel.CharacterCommonSettings.SneakWalkSpeed,
                _characterModel.CharacterCommonSettings.SneakRunSpeed, _characterModel.CharacterCommonSettings.
                    SneakAccelerationLag, _characterModel.CharacterCommonSettings.SneakDecelerationLag,
                        _characterModel.CharacterCommonSettings.MinimalSpeed);
            _battleSpeedCounter = new CharacterSpeedCounter(_characterModel.CharacterCommonSettings.InBattleWalkSpeed,
                _characterModel.CharacterCommonSettings.InBattleRunSpeed, _characterModel.CharacterCommonSettings.
                    InBattleAccelerationLag, _characterModel.CharacterCommonSettings.InBattleDecelerationLag,
                        _characterModel.CharacterCommonSettings.MinimalSpeed);
            _aimingSpeedCounter = new CharacterSpeedCounter(_characterModel.CharacterCommonSettings.AimWalkSpeed,
                _characterModel.CharacterCommonSettings.AimRunSpeed, _characterModel.CharacterCommonSettings.
                    AimAccelerationLag, _characterModel.CharacterCommonSettings.AimDecelerationLag,
                        _characterModel.CharacterCommonSettings.MinimalSpeed);

            _buttonsInfoUI = GameObject.Instantiate(Data.UIElementsData.ButtonsInformationPrefab);

            _cameraTransform = _services.CameraService.CharacterCamera.transform;
            CloseWeaponWheel();

            GameObject playerHealthBar = GameObject.Instantiate(Data.PlayerHealthBarData.HealthBarPrefab);
            _playerHealthBarModel = new PlayerHealthBarModel(playerHealthBar, Data.PlayerHealthBarData);
            _enemyHealthBarModel = new EnemyHealthBarModel(playerHealthBar, Data.EnemyHealthBarData);
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _characterModel.PlayerBehavior.SetTakeDamageEvent(TakeDamage);
            _inputModel.OnWeaponWheel += ControlWeaponWheelOpen;
            _inputModel.OnButtonsInfo += OpenCloseButtonsInfoMenu;
            _inputModel.OnUse += UseInteractiveObject;
            _inputModel.OnRemoveWeapon += () => OnWeaponChange?.Invoke();
            _inputModel.OnPressNumberOne += () => GetThrowableWeapon(Data.LureMeatData);
            _inputModel.OnPressNumberTwo += () => GetThrowableWeapon(Data.LureCharcoalData);
            OnWeaponChange += _services.TrapService.RemoveTrap;
            OnTrapPlace += _services.TrapService.PlaceTrap;
            OnHealthChange += HealthBarUpdate;

            _characterModel.CurrentWeaponData.Subscribe(OnWeaponChangeHandler);
            _characterModel.CurrentCharacterState.Subscribe(UpdateSpeedCounterByState);
            _characterModel.EnemiesInTrigger.ObserveCountChanged(true).Subscribe(OnEnemiesNear);

            MessageBroker.Default.Receive<OnPlayerReachHidePlaceEventClass>().Subscribe(EnableHiding);
            MessageBroker.Default.Receive<CharacterAnimationEvent>().Subscribe(PlaySoundFromAnimationEvent);

            _characterModel.PlayerBehavior.OnFilterHandler += OnTriggerFilter;
            _characterModel.PlayerBehavior.OnTriggerEnterHandler += OnTriggerEnterSomething;
            _characterModel.PlayerBehavior.OnTriggerExitHandler += OnTriggerExitSomething;
            _characterModel.BehaviorFall.OnPostActivate += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.KnockedDown]);
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            GroundCheck();
            MovementCheck();
            ControlWeaponWheel();
            EnemyHealthBarUpdate();
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _characterModel.PlayerBehavior.DeleteTakeDamageEvent(TakeDamage);
            _inputModel.OnWeaponWheel = null;
            _inputModel.OnButtonsInfo = null;
            _inputModel.OnUse = null;
            _inputModel.OnRemoveWeapon = null;
            _inputModel.OnPressNumberOne = null;
            _inputModel.OnPressNumberTwo = null;
            OnWeaponChange -= _services.TrapService.RemoveTrap;
            OnTrapPlace -= _services.TrapService.PlaceTrap;
            OnHealthChange -= HealthBarUpdate;

            _characterModel.CurrentWeaponData.Dispose();
            _characterModel.CurrentCharacterState.Dispose();
            _characterModel.EnemiesInTrigger.Dispose();

            _characterModel.PlayerBehavior.OnFilterHandler -= OnTriggerFilter;
            _characterModel.PlayerBehavior.OnTriggerEnterHandler -= OnTriggerEnterSomething;
            _characterModel.PlayerBehavior.OnTriggerExitHandler -= OnTriggerExitSomething;
            _characterModel.BehaviorFall.OnPostActivate -= () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.KnockedDown]);
        }

        #endregion


        #region ITakeDamage

        public void TakeDamage(Damage damage)
        {
            if (!_characterModel.CurrentStats.BaseStats.IsDead && !_characterModel.IsDodging)
            {
                _characterModel.CurrentStats.BaseStats.CurrentHealthPoints -= damage.GetTotalDamage();

                OnHealthChange?.Invoke();

                if (_characterModel.CurrentStats.BaseStats.CurrentHealthPoints <= 0)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dead]);
                }

                Debug.LogError("Player has: " + _characterModel.CurrentStats.BaseStats.CurrentHealthPoints + " of HP");

                //if (_stateMachine.CurrentState != _stateMachine.CharacterStates[CharacterStatesEnum.MidAir] &&
                //    _stateMachine.CurrentState != _stateMachine.CharacterStates[CharacterStatesEnum.KnockedDown])
                //{
                //    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Hitted]);                              
                //}
            }
        }

        private void TakeDamage(int id, Damage damage) => TakeDamage(damage);

        #endregion


        #region EnterExitMethods

        public void Initialize()
        {
            _lastPosition = _characterModel.CharacterTransform.position;
            _currentPosition = _lastPosition;
            HealthBarUpdate();
        }


        public void OnExit() // TO REFACTOR
        {
            //TODO
        }

        #endregion


        #region ActionHandlers

        private void OnWeaponChangeHandler()
        {
            OnWeaponChange?.Invoke();
        }

        private void OnWeaponChangeHandler(WeaponData newWeapon)
        {
            if ((_stateMachine.CurrentState == _stateMachine.CharacterStates[CharacterStatesEnum.Aiming] &&
                newWeapon?.Type != WeaponType.Shooting) || (_stateMachine.CurrentState == _stateMachine.
                    CharacterStates[CharacterStatesEnum.Battle] && newWeapon?.Type != WeaponType.Melee))
            {
                _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]);
            }

            newWeapon?.Init(_context);
        }

        #endregion


        #region OnTriggerHandlers

        private bool OnTriggerFilter(Collider interactedObject)
        {
            return interactedObject.GetComponentInChildren<InteractableObjectBehavior>() != null &&
                !interactedObject.isTrigger;
        }

        private void OnTriggerEnterSomething(ITrigger interactedItrigger, Collider interactedObject)
        {
            InteractableObjectBehavior interactedBehavior = interactedObject.
                GetComponentInChildren<InteractableObjectBehavior>();

            switch (interactedBehavior.Type)
            {
                case InteractableObjectType.Enemy:
                    if (!_characterModel.EnemiesInTrigger.Contains(interactedObject))
                    {
                        _characterModel.EnemiesInTrigger.Add(interactedObject);
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnTriggerExitSomething(ITrigger interactedItrigger, Collider interactedObject)
        {
            InteractableObjectBehavior interactedBehavior = interactedObject.
                GetComponentInChildren<InteractableObjectBehavior>();

            switch (interactedBehavior.Type)
            {
                case InteractableObjectType.Enemy:
                    if (_characterModel.EnemiesInTrigger.Contains(interactedObject))
                    {
                        _characterModel.EnemiesInTrigger.Remove(interactedObject);
                    }
                    break;
                default:
                    break;
            }
        }

        private bool OnHitBoxFilter(Collider hitedObject)
        {
            bool isEnemyColliderHit = false;
            InteractableObjectBehavior hitedBehavior = hitedObject.GetComponent<InteractableObjectBehavior>();

            if (hitedBehavior != null)
            {
                isEnemyColliderHit = hitedBehavior.Type == InteractableObjectType.Enemy || hitedBehavior.
                    Type == InteractableObjectType.WeakHitBox;
            }

            return isEnemyColliderHit;
        }

        private void OnHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (hitBox.IsInteractable && !enemy.isTrigger)
            {
                InteractableObjectBehavior enemyBehavior = enemy.transform.GetComponent<InteractableObjectBehavior>();

                if (enemyBehavior!=null && enemyBehavior.Type == InteractableObjectType.WeakHitBox)
                {
                    MessageBroker.Default.Publish(new OnBossWeakPointHittedEventClass { WeakPointCollider = enemy });
                }

                _services.AttackService.CountAndDealDamage(_characterModel.CurrentWeaponData.Value.WeaponDamage,
                    enemy.transform.root.gameObject.GetInstanceID(), _characterModel.CurrentStats, _characterModel.
                        CurrentWeaponData.Value);
                hitBox.IsInteractable = false;
            }
        }

        #endregion


        #region InFrameMethods

        private void GroundCheck()
        {
            _characterModel.IsGrounded = _services.PhysicsService.CheckGround(_characterModel.CharacterCapsuleCollider.
                transform.position + Vector3.up, _characterModel.CharacterCommonSettings.GroundCheckHeight, out _groundHit);
        }

        private void MovementCheck()
        {
            _inputModel.IsInputMove = _context.InputModel.IsInputMove;
        }

        #endregion


        #region WeaponWheelControls

        private void ControlWeaponWheelOpen(bool doOpen)
        {
            if (doOpen)
            {
                OpenWeaponWheel();
            }
            else
            {
                CloseWeaponWheel();
            }
        }

        private void OpenWeaponWheel()
        {
            _weaponWheelUI.SetActive(true);
            _services.CameraService.LockFreeLookCamera();
            _weaponWheelTransform.localPosition = Vector3.zero;
            _closestWeaponOnWheel = null;
            _isWeaponWheelOpen = true;
        }

        private void CloseWeaponWheel()
        {
            _weaponWheelUI.SetActive(false);
            _services.CameraService.UnlockFreeLookCamera();
            _isWeaponWheelOpen = false;

            if (_closestWeaponOnWheel != null)
            {
                if (_closestWeaponOnWheel.WeaponData != null)
                {
                    if (_closestWeaponOnWheel.WeaponData != _characterModel.CurrentWeaponData.Value)
                    {
                        OnWeaponChange?.Invoke();
                        GetWeapon(_closestWeaponOnWheel.WeaponData);
                    }
                }
                else if (_closestWeaponOnWheel.TrapData != null)
                {
                    if (_closestWeaponOnWheel.TrapData != _characterModel.CurrentPlacingTrapModel.Value?.TrapData)
                    {
                        OnWeaponChange?.Invoke();
                        GetTrap(_closestWeaponOnWheel.TrapData);
                    }
                }
                else
                {
                    OnWeaponChange?.Invoke();
                }
            }
        }

        private void ControlWeaponWheel()
        {
            if (_isWeaponWheelOpen)
            {
                _weaponWheelTransform.localPosition = new Vector3(
                    Mathf.Clamp(_weaponWheelTransform.localPosition.x + _inputModel.MouseInputX,
                    -WEAPON_WHEEL_MAX_CYCLE_DISTANCE, WEAPON_WHEEL_MAX_CYCLE_DISTANCE),
                    Mathf.Clamp(_weaponWheelTransform.localPosition.y + _inputModel.MouseInputY,
                    -WEAPON_WHEEL_MAX_CYCLE_DISTANCE, WEAPON_WHEEL_MAX_CYCLE_DISTANCE),
                    _weaponWheelTransform.localPosition.z);

                float distanceFromCenter = (_weaponWheelTransform.localPosition - Vector3.zero).sqrMagnitude;

                if (distanceFromCenter > WEAPON_WHEEL_MAX_CYCLE_DISTANCE)
                {
                    _weaponWheelTransform.localPosition *= WEAPON_WHEEL_MAX_CYCLE_DISTANCE / distanceFromCenter;
                }

                if (distanceFromCenter > WEAPON_WHEEL_DISTANCE_TO_SET_WEAPON)
                {
                    _closestWeaponOnWheel = GetClosestWeaponItemInWheel();
                }
                else
                {
                    DisableAllWeaponItemsInWheel();
                    _closestWeaponOnWheel = null;
                }
            }
        }

        private WeaponCircle GetClosestWeaponItemInWheel()
        {
            float minimalDistance = Mathf.Infinity;
            float currentDistance = minimalDistance;
            WeaponCircle chosenWeapon = null;

            for (int item = 0; item < _weaponWheelItems.Length; item++)
            {
                currentDistance = (_weaponWheelItems[item].AnchorPosition - _weaponWheelTransform.localPosition).
                    sqrMagnitude;

                if (currentDistance < minimalDistance)
                {
                    minimalDistance = currentDistance;
                    chosenWeapon = _weaponWheelItems[item];
                }
            }

            if (_closestWeaponOnWheel != chosenWeapon)
            {
                SetActivityOfElementOnWeaponWheel(_closestWeaponOnWheel, false);
                SetActivityOfElementOnWeaponWheel(chosenWeapon, true);
            }

            return chosenWeapon;
        }

        private void DisableAllWeaponItemsInWheel()
        {
            foreach (var item in _weaponWheelItems)
            {
                SetActivityOfElementOnWeaponWheel(item, false);
            }
        }

        private void InitAllWeaponItemsOnWheel()
        {
            foreach (var item in _weaponWheelItems)
            {
                Image[] images = item.GetComponentsInChildren<Image>();
                RectTransform[] rects = item.GetComponentsInChildren<RectTransform>();

                if (item.WeaponData != null)
                {
                    item.GetComponentsInChildren<Image>()[1].sprite = item.WeaponData.WeaponImage;
                    images[1].color = new Color(1f, 1f, 1f, WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_ALFA);
                }
                else if (item.TrapData != null)
                {
                    item.GetComponentsInChildren<Image>()[1].sprite = item.TrapData.TrapImage;
                    images[1].color = new Color(1f, 1f, 1f, WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_ALFA);
                }
                else
                {
                    images[1].color = new Color(1f, 1f, 1f, 0f);
                }

                images[0].color = new Color(1f, 1f, 1f, WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_ALFA);
                rects[0].localScale = new Vector3(WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE,
                    WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE, 1f);
                rects[1].localScale = new Vector3(WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_SCALE,
                    WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE, 1f);
            }
        }

        private void SetActivityOfElementOnWeaponWheel(WeaponCircle item, bool doActivate)
        {
            if (item != null)
            {
                Image[] images = item.GetComponentsInChildren<Image>();
                RectTransform[] rects = item.GetComponentsInChildren<RectTransform>();

                if (doActivate && item.IsNotEmpty)
                {
                    images[0].color = new Color(1f, 1f, 1f, WEAPON_WHEEL_PARENT_IMAGE_DEDICATED_ALFA);
                    images[1].color = new Color(1f, 1f, 1f, WEAPON_WHEEL_CHILD_IMAGE_DEDICATED_ALFA);

                    rects[0].localScale = new Vector3(WEAPON_WHEEL_IMAGE_DEDICATED_SCALE,
                        WEAPON_WHEEL_IMAGE_DEDICATED_SCALE, 1f);
                    rects[1].localScale = new Vector3(WEAPON_WHEEL_IMAGE_DEDICATED_SCALE,
                        WEAPON_WHEEL_IMAGE_DEDICATED_SCALE, 1f);

                    if (item.WeaponData != null)
                    {
                        _weaponWheelText.text = item.WeaponData.WeaponName;
                    }
                    else if (item.TrapData != null)
                    {
                        _weaponWheelText.text = item.TrapData.TrapStruct.TrapName;
                    }
                }
                else if (item.IsNotEmpty)
                {
                    images[0].color = new Color(1f, 1f, 1f, WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_ALFA);
                    images[1].color = new Color(1f, 1f, 1f, WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_ALFA);

                    rects[0].localScale = new Vector3(WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE,
                        WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE, 1f);
                    rects[1].localScale = new Vector3(WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_SCALE,
                        WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE, 1f);

                    _weaponWheelText.text = string.Empty;
                }
            }
        }

        #endregion


        #region WeaponControl

        private void GetThrowableWeapon(OneHandedThrowableWeapon weaponData)
        {
            OnWeaponChange?.Invoke();
            GetWeapon(weaponData);
        }

        public void GetWeapon(WeaponData weaponData)
        {
            new InitializeWeaponController(_context, weaponData, OnHitBoxFilter, OnHitBoxHit, ref OnWeaponChange);

            if (weaponData is OneHandedShootingWeapon oneHandedWeapon)
            {
                _isCurrentWeaponWithProjectile = oneHandedWeapon.ProjectileData != null;
            }
            else if (weaponData is TwoHandedShootingWeapon twoHandedWeapon)
            {
                _isCurrentWeaponWithProjectile = twoHandedWeapon.ProjectileData != null;
            }
            else if (weaponData is OneHandedThrowableWeapon)
            {
                _isCurrentWeaponWithProjectile = true;
            }

            _services.CameraService.UpdateWeaponProjectileExistence(_isCurrentWeaponWithProjectile);
        }

        public void GetTrap(TrapData trapData)
        {
            _services.TrapService.GetTrap(trapData);
        }

        #endregion


        #region MovementControlMethods

        public void SetAnimatorSpeed(float speed)
        {
            _characterModel.AnimationSpeed = speed;
        }

        public void StopCharacter()
        {
            _characterModel.CurrentSpeed = 0f;
            _targetAngleVelocity = 0f;
            _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0f, CurrentAngle, 0f);
            _characterModel.CharacterRigitbody.velocity = Vector3.zero;
        }

        public void MoveCharacter(bool isStrafing, bool onlyStrafing = false)
        {
            if (_characterModel.IsGrounded)
            {
                if (isStrafing && _inputModel.IsInputMove && !onlyStrafing)
                {
                    Vector3 moveDirection = (Vector3.forward * _inputModel.InputAxisY + Vector3.right *
                        _inputModel.InputAxisX);

                    if (Math.Abs(_inputModel.InputAxisX) + Math.Abs(_inputModel.InputAxisY) == 2)
                    {
                        moveDirection *= ANGULAR_MOVE_SPEED_REDUCTION_INDEX;
                    }

                    _characterModel.CharacterData.Move(_characterModel.CharacterTransform, _characterModel.CurrentSpeed,
                        moveDirection);
                }
                else if(!onlyStrafing)
                {
                    _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, _characterModel.CurrentSpeed);
                }
                else
                {
                    Vector3 moveDirection = (Vector3.forward * 0 + Vector3.right *
                                          _inputModel.InputAxisX);

                    if (Math.Abs(_inputModel.InputAxisX) + Math.Abs(_inputModel.InputAxisY) == 2)
                    {
                        moveDirection *= ANGULAR_MOVE_SPEED_REDUCTION_INDEX;
                    }

                    _characterModel.CharacterData.Move(_characterModel.CharacterTransform, _characterModel.CurrentSpeed,
                        moveDirection);
                }
            }
        }


        public void RotateCharacter(bool hasCameraControl, bool isStrafing = false)
        {
            if (_characterModel.IsGrounded)
            {
                if (hasCameraControl && !_services.CameraService.CameraCinemachineBrain.IsBlending)
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

                    if (isStrafing)
                    {
                        _characterModel.CharacterTransform?.LookAt(_characterModel.ClosestEnemy.Value.transform.position);
                        CurrentAngle = _characterModel.CharacterTransform.eulerAngles.y;
                    }
                }

                _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle,
                    -_targetAngleVelocity * Time.fixedDeltaTime);
            }
        }

        public void RotateCharacterAimimng()
        {
            if (_characterModel.IsGrounded)
            {
                _targetAngleVelocity = Mathf.SmoothStep(_targetAngleVelocity, 0, ANGULAR_VELOCITY_FADE_SPEED);
                CurrentAngle = _characterModel.CharacterTransform.eulerAngles.y + _inputModel.MouseInputX *
                    _characterModel.CharacterCommonSettings.AimingDirectionChangeSpeed * Time.deltaTime;
                _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle,
                    -_targetAngleVelocity * Time.fixedDeltaTime);
            }
        }

        private void UpdateSpeedCounterByState(CharacterBaseState currentState)
        {
            switch (currentState?.StateName)
            {
                case CharacterStatesEnum.Aiming:
                    _activeSpeedCounter = _aimingSpeedCounter;
                    break;
                case CharacterStatesEnum.Idle:
                    _activeSpeedCounter = _standartSpeedCounter;
                    break;
                case CharacterStatesEnum.Movement:
                    _activeSpeedCounter = _standartSpeedCounter;
                    break;
                case CharacterStatesEnum.Sneaking:
                    _activeSpeedCounter = _sneakingSpeedCounter;
                    break;
                case CharacterStatesEnum.Battle:
                    _activeSpeedCounter = _battleSpeedCounter;
                    break;
                case CharacterStatesEnum.ControlTransferring:
                    _activeSpeedCounter = _sneakingSpeedCounter;
                    break;
                default:
                    break;
            }
        }

        public void CountSpeed()
        {
            _activeSpeedCounter.CountSpeed(_inputModel.IsInputMove, _inputModel.IsInputRun,
                ref _curretSpeed, ref _currentVelocity);
            _characterModel.CurrentSpeed = _curretSpeed;
        }

        #endregion


        #region ButtonsInformationMenu

        private void OpenCloseButtonsInfoMenu()
        {
            _buttonsInfoUI.SetActive(!_buttonsInfoUI.activeSelf);
        }

        #endregion


        #region UsingMethods

        private void UseInteractiveObject()
        {
            foreach (BaseInteractiveObjectModel interactiveObjectModel in _context.InteractiveObjectModels.Values)
            {
                if (interactiveObjectModel.IsInteractive)
                {
                    interactiveObjectModel.InteractiveObjectData.Interact(interactiveObjectModel);

                    if (interactiveObjectModel.IsNeedControl)
                    {
                        if (interactiveObjectModel.IsActivated)
                        {
                            _characterModel.ClosestEnemy.Value = interactiveObjectModel.Prefab.GetComponent<Collider>();
                            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.ControlTransferring]);
                        }
                        else
                        {
                            _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Idle]);
                        }
                    }
                }
            }
        }

        private void EnableHiding(OnPlayerReachHidePlaceEventClass onPlayerHideEvent)
        {
            if (onPlayerHideEvent.CanHide && _stateMachine.CurrentState == _stateMachine.
                CharacterStates[CharacterStatesEnum.Sneaking])
            {
                _characterModel.PlayerBehavior.EnableHiding(true);
            }
            else
            {
                _characterModel.PlayerBehavior.EnableHiding(false);
            }

            _characterModel.IsInHidingPlace = onPlayerHideEvent.CanHide;
        }

        #endregion


        #region WeaponAiming

        public void UpdateAimingDotsForProjectile()
        {
            if (_isCurrentWeaponWithProjectile)
            {
                _services.CameraService.DrawAimLine();
            }
        }

        #endregion


        #region AudioMethods

        private void OnEnemiesNear(int quantity)
        {
            if (quantity > 0)
            {
                _services.AudioService.ChangeAmbientMusic(_services.AudioService.AudioData.AmbientMusicArray[1], 1f, true);
            }
            else
            {
                _services.AudioService.ChangeAmbientMusic(_services.AudioService.AudioData.AmbientMusicArray[0], 5f, true);
            }
        }

        private void PlaySoundFromAnimationEvent(CharacterAnimationEvent animationEvent)
        {
            int soundNum = 2;

            switch (animationEvent.AnimationEventType)
            {
                case CharacterAnimationEventTypes.None:
                    break;
                case CharacterAnimationEventTypes.LeftStep:
                    if (_lastAnimationEventType != CharacterAnimationEventTypes.LeftStep)
                    {
                        _characterModel.MovementAudioSource.PlayOneShot(_characterModel.CharacterCommonSettings.
                            StepSounds[soundNum]);
                    }
                    break;
                case CharacterAnimationEventTypes.RightStep:
                    if (_lastAnimationEventType != CharacterAnimationEventTypes.RightStep)
                    {
                        _characterModel.MovementAudioSource.PlayOneShot(_characterModel.CharacterCommonSettings.
                            StepSounds[soundNum]);
                    }
                    break;
                default:
                    break;
            }
            _characterModel.MovementAudioSource.pitch = 1 + UnityEngine.Random.Range(-0.1f, 0.1f);
            _lastAnimationEventType = animationEvent.AnimationEventType;
        }

        #endregion


        #region HealthBar

        private void HealthBarUpdate()
        {
            _playerHealthBarModel.HealthFillUpdate(_characterModel.CurrentStats.BaseStats.CurrentHealthPart);
        }

        /// <summary>Example method of implementing health restoration to the current max health threshold</summary>
        private void TestingHealthRestoreToCurrentMaxHealthThreshold()
        {
            float restoredHP = 5.0f;
            _characterModel.CurrentStats.BaseStats.CurrentHealthPoints += restoredHP;
            Debug.Log(this + ": Player is healing by " + restoredHP + " HP. Current health = " + _characterModel.
                CurrentStats.BaseStats.CurrentHealthPoints + " HP"
                    + "\nNote: \"H\"-button assigned for testing healing up to the current max health threshold");
            OnHealthChange?.Invoke();
        }

        #endregion


        #region EnemyHealthBar

        private void EnemyHealthBarUpdate()
        {
            if (_targetEnemy != null)
            {

                if (!_targetEnemy.CurrentStats.BaseStats.IsDead)
                {
                    _enemyHealthBarModel.CanvasHPImage.fillAmount = _targetEnemy.CurrentStats.BaseStats.CurrentHealthPart;
                }
                else
                {
                    _targetEnemy = null;
                    _enemyHealthBarModel.CanvasHPImage.fillAmount = 0f;
                }
            }
        }

        public void OnEnemyHealthBar(bool onEnemyBar)
        {
            if (onEnemyBar)
            {
                if (_characterModel.ClosestEnemy.Value != null)
                {
                    _targetEnemy = _context.NpcModels[_characterModel.ClosestEnemy.Value.transform.root.
                        gameObject.GetInstanceID()];

                    if (!_targetEnemy.CurrentStats.BaseStats.IsDead)
                    {
                        _enemyHealthBarModel.EnemyHealthBarObject.SetActive(onEnemyBar);
                    }
                    else
                    {
                        _enemyHealthBarModel.EnemyHealthBarObject.SetActive(false);
                    }
                }
            }
            else
            {
                _targetEnemy = null;
                _enemyHealthBarModel.EnemyHealthBarObject.SetActive(onEnemyBar);
            }
        }

        #endregion
    }
}

