using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using RootMotion.Dynamics;
using UniRx;

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

        public Action OnMove;
        public Action OnStop;
        public Action OnAttack;
        public Action OnJump;
        public Action OnSneak;
        public Action OnAim;
        public Action OnStartRun;
        public Action OnStopRun;

        public Action OnWeaponWheelOpen;
        public Action OnWeaponWheelClose;
        public Action OnWeaponChange;
        public Action OnTrapPlace;
        public Action OnTimeSkipOpenClose;
        public Action OnButtonsInfoMenuOpenClose;
        public Action OnUse;
        public Action OnHealthChange;

        private readonly GameContext _context;
        private readonly CharacterStateMachine _stateMachine;
        private readonly Services _services;
        private readonly CharacterModel _characterModel;
        private readonly InputModel _inputModel;
        private readonly PuppetMaster _puppetController;

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

        private float[] _speedArray;
        private float _currentHealth;
        private float _currentBattleIgnoreTime;
        private float _targetAngleVelocity;
        private float _targetSpeed;
        private float _speedChangeLag;
        private float _currentVelocity;
        private float _weaponWheelDistance;

        private bool _isWeaponWheelOpen;
        private bool _isCurrentWeaponWithProjectile;

        private PlayerHealthBarModel _playerHealthBarModel;
        private float _currentMaxHealthPercent;

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
            _context = context;
            _stateMachine = stateMachine;
            _services = Services.SharedInstance;
            _characterModel = _context.CharacterModel;
            _inputModel = _context.InputModel;
            _puppetController = _characterModel.PuppetMaster;
            _speedArray = new float[5] { 0f, 0f, 0f, 0f, 0f};

            _weaponWheelUI = GameObject.Instantiate(Data.UIElementsData.WeaponWheelPrefab);
            _weaponWheelTransform = _weaponWheelUI.transform.
                Find(WEAPON_WHEEL_PANEL_NAME).Find(WEAPON_WHEEL_CYCLE_NAME).transform;
            _weaponWheelItems = _weaponWheelUI.transform.Find(WEAPON_WHEEL_PANEL_NAME).
                GetComponentsInChildren<WeaponCircle>();
            _weaponWheelText = _weaponWheelUI.transform.GetComponentInChildren<Text>();
            InitAllWeaponItemsOnWheel();

            _buttonsInfoUI = GameObject.Instantiate(Data.UIElementsData.ButtonsInformationPrefab);

            _cameraTransform = _services.CameraService.CharacterCamera.transform;
            CloseWeaponWheel();

            GameObject playerHealthBar = GameObject.Instantiate(Data.PlayerHealthBarData.HealthBarPrefab);
            _playerHealthBarModel = new PlayerHealthBarModel(playerHealthBar, Data.PlayerHealthBarData);
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _characterModel.PlayerBehavior.SetTakeDamageEvent(TakeDamage);

            //LockCharAction.LockCharacterMovement += ExitTalkingState;
            //StartDialogueData.StartDialog += SetTalkingState;
            //GlobalEventsModel.OnBossDie += StopTarget; // TO REFACTOR

            //_services.TrapService.LoadTraps();

            _services.EventManager.StartListening(InputEventTypes.MoveStart, OnMoveHandler);
            _services.EventManager.StartListening(InputEventTypes.MoveStop, OnStopHandler);
            _services.EventManager.StartListening(InputEventTypes.Sneak, OnSneakHandler);
            _services.EventManager.StartListening(InputEventTypes.TimeSkipMenu, OnTimeSkipOpenCloseHandler);
            _services.EventManager.StartListening(InputEventTypes.WeaponWheelOpen, OnWeaponWheelOpenHandler);
            _services.EventManager.StartListening(InputEventTypes.WeaponWheelClose, OnWeaponWheelCloseHandler);
            _services.EventManager.StartListening(InputEventTypes.Attack, OnAttackHandler);
            _services.EventManager.StartListening(InputEventTypes.AimStart, OnAimHandler);
            _services.EventManager.StartListening(InputEventTypes.Jump, OnJumpHandler);
            _services.EventManager.StartListening(InputEventTypes.RunStart, OnStartRunHandler);
            _services.EventManager.StartListening(InputEventTypes.RunStop, OnStopRunHandler);
            _services.EventManager.StartListening(InputEventTypes.ButtonsInfoMenu, OnButtonsInfoOpenCloseHandler);
            _services.EventManager.StartListening(InputEventTypes.Use, OnUseHandler);
            _services.EventManager.StartListening(InputEventTypes.WeaponRemove, OnWeaponChangeHandler);

            OnWeaponWheelOpen += OpenWeaponWheel;
            OnWeaponWheelClose += CloseWeaponWheel;
            OnButtonsInfoMenuOpenClose += OpenCloseButtonsInfoMenu;
            OnWeaponChange += _services.TrapService.RemoveTrap;
            OnTrapPlace += _services.TrapService.PlaceTrap;
            OnUse += UseInteractiveObject;
            OnHealthChange += HealthBarUpdate;

            _characterModel.CurrentWeaponData.Subscribe(OnWeaponChangeHandler);

            MessageBroker.Default.Receive<OnPlayerReachHidePlaceEventClass>().Subscribe(EnableHiding);

            _characterModel.PlayerBehavior.OnFilterHandler += OnTriggerFilter;
            _characterModel.PlayerBehavior.OnTriggerEnterHandler += OnTriggerEnterSomething;
            _characterModel.PlayerBehavior.OnTriggerExitHandler += OnTriggerExitSomething;
            _characterModel.BehaviorFall.OnPostActivate += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.KnockedDown]);
            _characterModel.BehaviorPuppet.OnPostActivate += () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingUp]);
            _characterModel.BehaviorPuppet.onRegainBalance.unityEvent.AddListener(() => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]));
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            GroundCheck();
            MovementCheck();
            SpeedCheck();
            ControlWeaponWheel();

            //FOR DEBUG ONLY!
            if (Input.GetKeyDown(KeyCode.H)) TestingHealthRestoreToCurrentMaxHealthThreshold();
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _characterModel.PlayerBehavior.DeleteTakeDamageEvent(TakeDamage);

            //LockCharAction.LockCharacterMovement -= ExitTalkingState;
            //StartDialogueData.StartDialog -= SetTalkingState;
            //GlobalEventsModel.OnBossDie -= StopTarget;

            _services.EventManager.StopListening(InputEventTypes.MoveStart, OnMoveHandler);
            _services.EventManager.StopListening(InputEventTypes.MoveStop, OnStopHandler);
            _services.EventManager.StopListening(InputEventTypes.Sneak, OnSneakHandler);
            _services.EventManager.StopListening(InputEventTypes.TimeSkipMenu, OnTimeSkipOpenCloseHandler);
            _services.EventManager.StopListening(InputEventTypes.WeaponWheelOpen, OnWeaponWheelOpenHandler);
            _services.EventManager.StopListening(InputEventTypes.WeaponWheelClose, OnWeaponWheelCloseHandler);
            _services.EventManager.StopListening(InputEventTypes.Attack, OnAttackHandler);
            _services.EventManager.StopListening(InputEventTypes.AimStart, OnAimHandler);
            _services.EventManager.StopListening(InputEventTypes.Jump, OnJumpHandler);
            _services.EventManager.StopListening(InputEventTypes.RunStart, OnStartRunHandler);
            _services.EventManager.StopListening(InputEventTypes.RunStop, OnStopRunHandler);
            _services.EventManager.StopListening(InputEventTypes.ButtonsInfoMenu, OnButtonsInfoOpenCloseHandler);
            _services.EventManager.StopListening(InputEventTypes.Use, OnUseHandler);
            _services.EventManager.StopListening(InputEventTypes.WeaponRemove, OnWeaponChangeHandler);

            OnWeaponWheelOpen -= OpenWeaponWheel;
            OnWeaponWheelClose -= CloseWeaponWheel;
            OnButtonsInfoMenuOpenClose -= OpenCloseButtonsInfoMenu;
            OnWeaponChange -= _services.TrapService.RemoveTrap;
            OnTrapPlace -= _services.TrapService.PlaceTrap;
            OnUse -= UseInteractiveObject;
            OnHealthChange -= HealthBarUpdate;

            _characterModel.CurrentWeaponData.Dispose();

            _characterModel.PlayerBehavior.OnFilterHandler -= OnTriggerFilter;
            _characterModel.PlayerBehavior.OnTriggerEnterHandler -= OnTriggerEnterSomething;
            _characterModel.PlayerBehavior.OnTriggerExitHandler -= OnTriggerExitSomething;
            _characterModel.BehaviorFall.OnPostActivate -= () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.KnockedDown]);
            _characterModel.BehaviorPuppet.OnPostActivate -= () => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.GettingUp]);
            _characterModel.BehaviorPuppet.onRegainBalance.unityEvent.RemoveListener(() => _stateMachine.
                SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Movement]));
        }

        #endregion


        #region ITakeDamage

        public void TakeDamage(Damage damage)
        {
            if (!_characterModel.IsDead && !_characterModel.IsDodging)
            {
                _currentHealth -= damage.PhysicalDamage;
                _currentHealth -= damage.FireDamage;

                OnHealthChange?.Invoke();

                float stunProbability = UnityEngine.Random.Range(0f, 1f);

                if (_currentHealth <= 0)
                {
                    _stateMachine.SetState(_stateMachine.CharacterStates[CharacterStatesEnum.Dead]);
                }

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
                //enemy.TakeDamageEvent(damage); TO REFACTOR
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
            _currentMaxHealthPercent = _currentHealth * 100 / _characterModel.CharacterCommonSettings.HealthPoints;
            _currentBattleIgnoreTime = 0; // TO REFACTOR
        }


        public void OnExit() // TO REFACTOR
        {
            //TODO
        }

        #endregion


        #region ActionHandlers

        private void OnMoveHandler()
        {
            OnMove?.Invoke();
        }

        private void OnStopHandler()
        {
            OnStop?.Invoke();
        }

        private void OnJumpHandler()
        {
            OnJump?.Invoke();
        }

        private void OnSneakHandler()
        {
            OnSneak?.Invoke();
        }

        private void OnWeaponWheelOpenHandler()
        {
            OnWeaponWheelOpen?.Invoke();
        }

        private void OnWeaponWheelCloseHandler()
        {
            OnWeaponWheelClose?.Invoke();
        }

        private void OnTimeSkipOpenCloseHandler()
        {
            OnTimeSkipOpenClose?.Invoke();
        }

        private void OnButtonsInfoOpenCloseHandler()
        {
            OnButtonsInfoMenuOpenClose?.Invoke();
        }

        private void OnAttackHandler()
        {
            OnAttack?.Invoke();
        }

        private void OnAimHandler()
        {
            OnAim?.Invoke();
        }

        private void OnStartRunHandler()
        {
            OnStartRun?.Invoke();
        }

        private void OnStopRunHandler()
        {
            OnStopRun?.Invoke();
        }

        private void OnUseHandler()
        {
            OnUse?.Invoke();
        }

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
            if (hitBox.IsInteractable)
            {
                InteractableObjectBehavior enemyBehavior = enemy.transform.GetComponent<InteractableObjectBehavior>();

                if (enemyBehavior.Type == InteractableObjectType.WeakHitBox)
                {
                    MessageBroker.Default.Publish(new OnBossWeakPointHittedEventClass { WeakPointCollider = enemy });
                }

                DealDamage(enemyBehavior, _services.AttackService.CountDamage(_characterModel.CurrentWeaponData.Value,
                    _characterModel.CharacterStatsSettings, _context.NpcModels[GetParent(enemyBehavior.transform).
                        GetInstanceID()].GetStats().MainStats));
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

        private void AddSpeedToArray(float speed)
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
                AddSpeedToArray(Mathf.Sqrt((_currentPosition - _lastPosition).sqrMagnitude) / Time.deltaTime);
                _characterModel.CurrentSpeed = GetAverageSpeed();
                _lastPosition = _currentPosition;
            }
        }

        #endregion


        #region WeaponWheelControls

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
                else if(item.TrapData != null)
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
                    else if(item.TrapData != null)
                    {
                        _weaponWheelText.text = item.TrapData.TrapStruct.TrapName;
                    }
                }
                else if(item.IsNotEmpty)
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

        public void GetWeapon(WeaponData weaponData)
        {
            new InitializeWeaponController(_context, weaponData, OnHitBoxFilter, OnHitBoxHit, ref OnWeaponChange);

            if(weaponData is OneHandedShootingWeapon oneHandedWeapon)
            {
                _isCurrentWeaponWithProjectile = oneHandedWeapon.ProjectileData != null;
            }
            else if(weaponData is TwoHandedShootingWeapon twoHandedWeapon)
            {
                _isCurrentWeaponWithProjectile = twoHandedWeapon.ProjectileData != null;
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
            MovementSpeed = 0f;
            _targetAngleVelocity = 0f;
            _characterModel.CharacterTransform.localRotation = Quaternion.Euler(0, CurrentAngle, 0);
        }

        public void MoveCharacter(bool isStrafing)
        {
            if (_characterModel.IsGrounded)
            {
                if (isStrafing && _inputModel.IsInputMove)
                {
                    Vector3 moveDirection = (Vector3.forward * _inputModel.InputAxisY + Vector3.right * 
                        _inputModel.InputAxisX);

                    if(Math.Abs(_inputModel.InputAxisX) + Math.Abs(_inputModel.InputAxisY) == 2)
                    {
                        moveDirection *= ANGULAR_MOVE_SPEED_REDUCTION_INDEX;
                    }

                    _characterModel.CharacterData.Move(_characterModel.CharacterTransform, MovementSpeed, 
                        moveDirection);
                }
                else
                {
                    _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, MovementSpeed);
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
                        _characterModel.CharacterTransform?.LookAt(_characterModel.ClosestEnemy.transform.position);                         
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
                CurrentAngle = _characterModel.CharacterTransform.eulerAngles.y + _inputModel.MouseInputX * _characterModel.CharacterCommonSettings.AimingDirectionChangeSpeed * Time.deltaTime;
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

        public void CountSpeedStrafing()
        {
            if (_inputModel.IsInputMove)
            {
                if (_inputModel.IsInputRun)
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.InBattleRunSpeed;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.InBattleWalkSpeed;
                }
            }
            else
            {
                _targetSpeed = 0;
            }

            if (_characterModel.CurrentSpeed < _targetSpeed)
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.InBattleAccelerationLag;
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.InBattleDecelerationLag;
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
                _speedChangeLag = _characterModel.CharacterCommonSettings.SneakAccelerationLag;
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.SneakDecelerationLag;
            }

            MovementSpeed = Mathf.SmoothDamp(_characterModel.CurrentSpeed, _targetSpeed, ref _currentVelocity,
                _speedChangeLag);
        }

        public void CountSpeedAiming()
        {
            if (_inputModel.IsInputMove)
            {
                if (_inputModel.IsInputRun)
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.AimRunSpeed;
                }
                else
                {
                    _targetSpeed = _characterModel.CharacterCommonSettings.AimWalkSpeed;
                }
            }
            else
            {
                _targetSpeed = 0;
            }

            if (_characterModel.CurrentSpeed < _targetSpeed)
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.AimAccelerationLag;
            }
            else
            {
                _speedChangeLag = _characterModel.CharacterCommonSettings.AimDecelerationLag;
            }

            MovementSpeed = Mathf.SmoothDamp(_characterModel.CurrentSpeed, _targetSpeed, ref _currentVelocity,
                _speedChangeLag);
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
            foreach (BaseInteractiveObjectModel interactiveObjectModel in _context.InteractableObjectModels.Values)
            {
                if (interactiveObjectModel.IsInteractive)
                {
                    interactiveObjectModel.InteractiveObjectData.Interact(interactiveObjectModel);
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


        #region HealthBar

        private void HealthBarUpdate()
        {
            _currentMaxHealthPercent = _playerHealthBarModel.HealthFillUpdate(_currentHealth * 100 / _characterModel.CharacterCommonSettings.HealthPoints);
        }

        /// <summary>Example method of implementing health restoration to the current max health threshold</summary>
        private void TestingHealthRestoreToCurrentMaxHealthThreshold()
        {
            float restoredHP = 5.0f;
            _currentHealth = Mathf.Clamp(_currentHealth + restoredHP, 0, _currentMaxHealthPercent);
            Debug.Log(this + ": Player is healing by " + restoredHP + " HP. Current health = " + _currentHealth + " HP"
                + "\nNote: \"H\"-button assigned for testing healing up to the current max health threshold");
            OnHealthChange?.Invoke();
        }

        #endregion
    }
}

