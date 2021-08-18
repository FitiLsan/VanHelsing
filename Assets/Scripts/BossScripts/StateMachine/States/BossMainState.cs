using UnityEngine;
using UniRx;
using System;
using Extensions;
using DG.Tweening;
using Photon.Pun;

namespace BeastHunter
{
    public class BossMainState : BossBaseState
    {
        #region Constants

        public const float ANGLE_TARGET_RANGE = 20f;
        public const float DISTANCE_TO_START_ATTACK = 5f;
        private const float TRIGGER_VIEW_INCREASE = 70f;
        private const float SPEED_COUNT_FRAME = 0.15f;
        private const float TIME_TO_NORMILIZE_WEAK_POINT = 5f;

        private const float HUNGER_TIME = 5f;

        private const float CAMERA_LOW_SIDE_ANGLE = 45f;
        private const float CAMERA_HALF_SIDE_ANGLE = 90f;
        private const float CAMERA_BACK_SIDE_ANGLE = 225f;
        private const float CAMERA_BACK_ANGLE = 180f;

        private const float ANGULAR_VELOCITY_FADE_SPEED = 0.2f;
        private const float ANGULAR_MOVE_SPEED_REDUCTION_INDEX = 0.7f;
        private const float DIRECTION_CHANGE_LAG = 0.2f;

        #endregion


        #region Fields

        public Quaternion TargetRotation;

        private Vector3 _lastPosition;
        private Vector3 _currentPosition;
        private Vector3 _targetDirection;

        private float _speedCountTime;
        private float _hungerCountTime = HUNGER_TIME;

        private float _timer = 7f;
        private int _hitPerTime = 0;
        private float _damagePerTime;
        private float _realNavAgentSpeed;
        private float _lastSpeedModifier = -1;

        private InputModel _inputModel;
        private GameContext _context;
        private Services _services;
        private PhotonView _photonView;
        private float _targetAngleVelocity;
        private Transform _cameraTransform;

        #endregion

        #region Properties

        public bool IsHunger { get; private set; }

        private float TargetDirection { get; set; }
        private float CurrentDirecton { get; set; }
        private float AdditionalDirection { get; set; }
        private float CurrentAngle { get; set; }

        #endregion


        #region ClassLifeCycle

        public BossMainState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            _stateMachine._model.BossBehavior.OnFilterHandler += OnFilterHandler;
            _stateMachine._model.BossBehavior.OnTriggerEnterHandler += OnTriggerEnterHandler;
            _stateMachine._model.BossBehavior.OnTriggerExitHandler += OnTriggerExitHandler;
            MessageBroker.Default.Receive<OnPlayerDieEventCLass>().Subscribe(OnPlayerDieHandler);
            MessageBroker.Default.Receive<OnBossStunnedEventClass>().Subscribe(OnBossStunnedHandler);
            MessageBroker.Default.Receive<OnBossHittedEventClass>().Subscribe(OnBossHittedHandler);
            MessageBroker.Default.Receive<OnBossWeakPointHittedEventClass>().Subscribe(MakeWeakPointBurst);
            _bossModel.CurrentStats.BaseStats.SpeedUpdate += OnSpeedUpdate;
            // MessageBroker.Default.Receive<OnPlayerSneakingEventClass>().Subscribe(OnPlayerSneakingHandler);

            _services = Services.SharedInstance;
            _context = _services.Context;
            _inputModel = _context.InputModel;
            _photonView = _bossModel.ObjectOnScene.GetComponent<PhotonView>();
            _cameraTransform = _services.CameraService.BossCamera.transform;

            _inputModel.OnAttack += BossAttack;
            _inputModel.OnPressNumberOne += BossAttack;
            _inputModel.OnPressNumberTwo += BossAttack;
            _inputModel.OnPressNumberThree += BossAttack;
            _inputModel.OnPressNumberFour += BossAttack;

        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = false;
        }






        /// <summary>
        /// //////////////////////////////////////////////
        /// </summary>


        public void MoveBoss(bool isStrafing, bool onlyStrafing = false)
        {
            if (!_bossModel.IsGrounded)
            {
                if (_inputModel.IsInputMove)
                {
                    _stateMachine.SetCurrentState(BossStatesEnum.Moving);
                    _bossModel.BossData.MoveForward(_bossModel.BossTransform, _bossModel.CurrentSpeed);
                }

                else
                {
                    _stateMachine.SetCurrentState(BossStatesEnum.Idle);
                }
            }
        }

        public void RotateBoss(bool hasCameraControl, bool isStrafing = false)
        {
            if (!_bossModel.IsGrounded)
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

                    CurrentDirecton = _bossModel.BossTransform.localEulerAngles.y;
                    TargetDirection = _cameraTransform.localEulerAngles.y + AdditionalDirection;
                    CurrentAngle = Mathf.SmoothDampAngle(CurrentDirecton, TargetDirection, ref _targetAngleVelocity,
                        DIRECTION_CHANGE_LAG);
                }
                else
                {
                    _targetAngleVelocity = Mathf.SmoothStep(_targetAngleVelocity, 0, ANGULAR_VELOCITY_FADE_SPEED);

                    if (isStrafing)
                    {
                       // _bossModel.BossTransform?.LookAt(_bossModel.ClosestEnemy.Value.transform.position);
                        CurrentAngle = _bossModel.BossTransform.eulerAngles.y;
                    }
                }

                _bossModel.BossTransform.localRotation = Quaternion.Euler(0, CurrentAngle,
                    -_targetAngleVelocity * Time.fixedDeltaTime);
            }
        }


        private void BossAttack()
        {
            if (_stateMachine.CurrentStateType != BossStatesEnum.Attacking)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
            }
        }



        /// <summary>
        /// /////////////////////////////////////
        /// </summary>

        public override void Execute()
        {
            if (!_bossModel.CurrentStats.BaseStats.IsDead)
            {
                SpeedCheck();
                HealthCheck();
                MoveBoss(false);
                RotateBoss(true);
                //  CheckDirection(); //disable for Photon
                //   HungerCheck();//disable for Photon
                //  CheckCurrentFieldOfView();
                HitCounter();
                InteractionTriggerUpdate();
            }
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
            _stateMachine._model.BossBehavior.OnFilterHandler -= OnFilterHandler;
            _stateMachine._model.BossBehavior.OnTriggerEnterHandler -= OnTriggerEnterHandler;
            _stateMachine._model.BossBehavior.OnTriggerExitHandler -= OnTriggerExitHandler;
            _bossModel.BossBehavior.OnTriggerStayHandler -= OnTriggerStayHandler;
        }

        private void OnBossHittedHandler(OnBossHittedEventClass eventClass)
        {
            if (!_bossModel.CurrentStats.BaseStats.IsDead)
            {
                _hitPerTime++;
              //  _stateMachine.SetCurrentStateOverride(BossStatesEnum.Hitted);
            }
        }

        private void OnBossStunnedHandler(OnBossStunnedEventClass eventClass)
        {
            if (!_bossModel.CurrentStats.BaseStats.IsDead)
            {
             //   _stateMachine.SetCurrentStateOverride(BossStatesEnum.Stunned);
            }         
        }

        private void OnPlayerDieHandler(OnPlayerDieEventCLass eventClass)
        {
            if (!_bossModel.CurrentStats.BaseStats.IsDead)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Patroling);
            }
        }

        private void HealthCheck()
        {
            if ( _bossModel.CurrentStats.BaseStats.CurrentHealthPart < 1)
            {
                var rate = (1 - _bossModel.CurrentStats.BaseStats.CurrentHealthPart) * 10;
                Regeneration(rate);
                var wispCount = (int)rate * 3;
                _bossModel.Wisps.maxParticles = wispCount;
            }
        }

        private void Regeneration(float rate)
        {
            var _healPower = _bossModel.CurrentStats.BaseStats.HealthRegenPerSecond;
            _bossModel.CurrentStats.BaseStats.CurrentHealthPoints += _healPower * rate * Time.deltaTime;
        }

        private void HitCounter()
        {
            if (_hitPerTime > 0)
            {
                _timer -= Time.deltaTime;
            }

            if (_timer <= 0)
            {
                if (_hitPerTime >= 3)
                {
                    _stateMachine.BossSkills.HardBarkSkill.SwitchAllowed(true);
                    if (!_stateMachine.CurrentState.IsAnimationPlay)
                    {
                      //  _stateMachine.BossSkills.ForceUseSkill(_stateMachine.BossSkills.MainSkillDictionary[SkillDictionaryEnum.DefenceStateSkillDictionary], 2);
                    }
                }

                DamageCounterReset();
            }
        }

        private void DamageCounterReset()
        {
            _timer = 7f;
            _hitPerTime = 0;
        }

        public void DamageCounter(Damage damage)
        {
            _damagePerTime += damage.GetTotalDamage();

            if (_damagePerTime >= 20)
            {
                if (_stateMachine.CurrentState.IsAnySkillUsed && _stateMachine.CurrentState.CurrentSkill.CanInterrupt)
                {
                    CurrentSkillStop();
                }
                _damagePerTime = 0;
            }
        }

        private void SpeedCheck()
        {
            var realSpeed = 0f;

            if (_bossModel.CurrentSpeed != _bossModel.BossNavAgent.speed)
            {
               realSpeed =  DOVirtual.EasedValue(_bossModel.CurrentSpeed, _bossModel.BossNavAgent.speed, 0.2f, Ease.InCirc);
            }
            _bossModel.CurrentSpeed = realSpeed;//Mathf.Clamp(realSpeed - _bossModel.CurrentStats.BaseStats.SpeedModifier, 0, float.PositiveInfinity);

            _bossModel.BossAnimator.SetFloat("Speed", _bossModel.CurrentSpeed);

        }

        public void CheckCurrentFieldOfView()
        {
            if(_stateMachine.CurrentState.IsBattleState)
            {
                _stateMachine._model.BossSphereCollider.radius = TRIGGER_VIEW_INCREASE;
                _bossModel.BossBehavior.OnTriggerStayHandler += OnTriggerStayHandler;
            }
            else
            {
                _stateMachine._model.BossSphereCollider.radius = _bossData._bossSettings.SphereColliderRadius;
            }
        }

        private void HungerCheck()
        {
            if (!IsHunger)
            {
                _hungerCountTime -= Time.deltaTime;
                if(_hungerCountTime<=0)
                {
                    IsHunger = true;
                    _hungerCountTime = HUNGER_TIME;
                }
            }
        }

        private bool OnFilterHandler(Collider tagObject)
        {
            bool isEnemyColliderHit = false;
            InteractableObjectBehavior interacterBehavior = tagObject.GetComponent<InteractableObjectBehavior>();

            if (interacterBehavior != null)
            {
                if( interacterBehavior.Type == InteractableObjectType.Player || interacterBehavior.Type == InteractableObjectType.Food)
                {
                    isEnemyColliderHit = true;
                }
            }
            return isEnemyColliderHit;
        }

        private void OnTriggerEnterHandler(ITrigger thisdObject, Collider enteredObject)
        {
            return;//
            var interactableObject = enteredObject.GetComponent<InteractableObjectBehavior>().Type;

            if (interactableObject == InteractableObjectType.Player && !enteredObject.isTrigger && !_stateMachine.CurrentState.IsBattleState)
            {
                _bossModel.BossCurrentTarget = enteredObject.gameObject;
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
            }

            if (interactableObject == InteractableObjectType.Food)
            {
                if (!_stateMachine._model.FoodListInSight.Contains(enteredObject.gameObject))
                {
                    _stateMachine._model.FoodListInSight.Add(enteredObject.gameObject);
                }
            }
        }

        private void OnTriggerExitHandler(ITrigger thisdObject, Collider enteredObject)
        {
            return;//
            var interactableObject = enteredObject.GetComponent<InteractableObjectBehavior>().Type;

            if (interactableObject == InteractableObjectType.Food)
            {
                if (_stateMachine._model.FoodListInSight.Contains(enteredObject.gameObject))
                {
                    _stateMachine._model.FoodListInSight.Remove(enteredObject.gameObject);
                }
            }
            if (interactableObject == InteractableObjectType.Player & !enteredObject.isTrigger)
            {
           //     _stateMachine.SetCurrentStateOverride(BossStatesEnum.Searching); TODO
            }
        }

        private void OnTriggerStayHandler(ITrigger thisdObject, Collider enteredObject)
        {
            return;//
            var interactableObject = enteredObject.GetComponent<InteractableObjectBehavior>().Type;
            if (interactableObject == InteractableObjectType.Player && !enteredObject.isTrigger)
            {
                _bossModel.BossCurrentTarget = enteredObject.gameObject;
                _bossModel.BossBehavior.OnTriggerStayHandler -= OnTriggerStayHandler;
            }
        }

        private void MakeWeakPointBurst(OnBossWeakPointHittedEventClass eventClass)
        {
            eventClass.WeakPointCollider.gameObject.GetComponent<ParticleSystem>().Play();
            Services.SharedInstance.AttackService.CountAndDealDamage(eventClass.WeakPointCollider.
                GetComponent<HitBoxBehavior>().AdditionalDamage, _bossModel.BossTransform.root.
                    gameObject.GetInstanceID());
            eventClass.WeakPointCollider.GetComponent<Light>().color = Color.red;
            eventClass.WeakPointCollider.enabled = false;

            Action makeWeakPointNormalAction = () => MakeWeakPointNormal(eventClass.WeakPointCollider);
            TimeRemaining makeWeakPointNormal = new TimeRemaining(makeWeakPointNormalAction, TIME_TO_NORMILIZE_WEAK_POINT);
            makeWeakPointNormal.AddTimeRemaining(TIME_TO_NORMILIZE_WEAK_POINT);
        }

        private void MakeWeakPointNormal(Collider collider)
        {
            collider.enabled = true;
            collider.GetComponent<Light>().color = Color.yellow;
        }

        private void CheckDirection()
        {
            _targetDirection = _stateMachine._context.CharacterModel != null ? (_stateMachine._context.CharacterModel.
                CharacterTransform.position - _stateMachine._model.BossTransform.position).normalized : Vector3.zero;
            TargetRotation = Quaternion.LookRotation(_targetDirection);
        }
        
        public Vector3 GetTargetCurrentPosition()
        {     if (_bossModel.BossCurrentTarget != null)
            {
                return _bossModel.BossCurrentTarget.transform.position;
            }
            else
            {
                return _bossModel.BossTransform.position;
            }
        }


        private void InteractionTriggerUpdate()
        {
          _bossModel.ClosestTriggerIndex = _bossModel.InteractionSystem.GetClosestTriggerIndex();
            _bossModel.InteractionTarget = _bossModel.InteractionSystem.GetClosestInteractionObjectInRange();
            //if(_bossModel.BossCurrentTarget !=null)
            // {
            //     _bossModel.InteractionTarget = _bossModel.BossCurrentTarget.GetComponentInChildren<InteractionObject>();
            // }
        }

        private void OnSpeedUpdate()
        {
            var speed = _stateMachine.CurrentState.IsBattleState ? _bossModel.BossSettings.RunSpeed : _bossModel.BossSettings.WalkSpeed;
            _bossData.SetNavMeshAgentSpeed(_bossModel, _bossModel.BossNavAgent, speed);
        }

        #endregion
    }
}

