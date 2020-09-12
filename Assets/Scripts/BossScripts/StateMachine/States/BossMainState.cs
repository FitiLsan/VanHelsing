using UnityEngine;
using System;


namespace BeastHunter
{
    public class BossMainState : BossBaseState
    {
        #region Constants

        public const float ANGLE_TARGET_RANGE = 20f;
        public const float DISTANCE_TO_START_ATTACK = 1.5f;

        private const float SPEED_COUNT_FRAME = 0.15f;
        private const float TIME_TO_NORMILIZE_WEAK_POINT = 5f;

        #endregion


        #region Fields

        public Quaternion TargetRotation;

        private Vector3 _lastPosition;
        private Vector3 _currentPosition;
        private Vector3 _targetDirection;

        private float _speedCountTime;

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

            GlobalEventsModel.OnPlayerDie += OnPlayerDieHandler;
            GlobalEventsModel.OnBossStunned += OnBossStunnedHandler;
            GlobalEventsModel.OnBossHitted += OnBossHittedHandler;
            GlobalEventsModel.OnBossWeakPointHitted += MakeWeakPointBurst;
            GlobalEventsModel.OnPlayerSneaking += OnPlayerSneakingHanlder;
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = false;
        }

        public override void Execute()
        {
            if (!_stateMachine._model.IsDead)
            {
                SpeedCheck();
                HealthCheck();
                CheckDirection();
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

            GlobalEventsModel.OnPlayerDie -= OnPlayerDieHandler;
            GlobalEventsModel.OnBossStunned -= OnBossStunnedHandler;
            GlobalEventsModel.OnBossHitted -= OnBossHittedHandler;
            GlobalEventsModel.OnBossWeakPointHitted -= MakeWeakPointBurst;
            GlobalEventsModel.OnPlayerSneaking -= OnPlayerSneakingHanlder;
        }

        private void OnBossHittedHandler()
        {
            if (!_stateMachine._model.IsDead)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Hitted);
            }
        }

        private void OnBossStunnedHandler()
        {
            if (!_stateMachine._model.IsDead)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Stunned);
            }         
        }

        private void OnPlayerDieHandler()
        {
            if (!_stateMachine._model.IsDead)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Patroling);
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
                _speedCountTime = SPEED_COUNT_FRAME;
                _currentPosition = _stateMachine._model.BossTransform.position;
                _stateMachine._model.CurrentSpeed = Mathf.Sqrt((_currentPosition - _lastPosition).sqrMagnitude) /
                    SPEED_COUNT_FRAME;
                _lastPosition = _currentPosition;
            }

            _stateMachine._model.BossAnimator.SetFloat("Speed", _stateMachine._model.CurrentSpeed);
        }

        private void HealthCheck()
        {
            if (_stateMachine._model.CurrentHealth <= 0)
            {
                _stateMachine.SetCurrentStateAnyway(BossStatesEnum.Dead);
            }
        }

        private bool OnFilterHandler(Collider tagObject)
        {
            return !tagObject.isTrigger && tagObject.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger thisdObject, Collider enteredObject)
        {
            if(thisdObject.Type == InteractableObjectType.Enemy)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
            }         
        }

        private void OnTriggerExitHandler(ITrigger thisdObject, Collider exitedObject)
        {
            if (thisdObject.Type == InteractableObjectType.Enemy)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Searching);
            }
        }

        private void MakeWeakPointBurst(Collider collider)
        {
            collider.gameObject.GetComponent<ParticleSystem>().Play();
            _stateMachine._model.TakeDamage(Services.SharedInstance.AttackService.CountDamage(
                collider.GetComponent<HitBoxBehavior>().AdditionalDamage, _stateMachine._model.GetStats().MainStats));
            collider.GetComponent<Light>().color = Color.red;
            collider.enabled = false;

            Action makeWeakPointNormalAction = () => MakeWeakPointNormal(collider);
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
            _targetDirection = (_stateMachine._context.CharacterModel.CharacterTransform.position -
                _stateMachine._model.BossTransform.position).normalized;
            TargetRotation = Quaternion.LookRotation(_targetDirection);
        }

        public int AngleDirection(Vector3 forward, Vector3 targetDirection, Vector3 up)
        {
            Vector3 perpendicular = Vector3.Cross(forward, targetDirection);
            float direction = Vector3.Dot(perpendicular, up);

            if (direction > 0f)
            {
                return 1;
            }
            else if (direction < 0f)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private void OnPlayerSneakingHanlder(bool isSneaking)
        {
            if (isSneaking)
            {
                _stateMachine._model.BossSphereCollider.radius /= 
                    _stateMachine._model.BossSettings.SphereColliderRadiusDecreace;
            }
            else
            {
                _stateMachine._model.BossSphereCollider.radius = _stateMachine._model.BossSettings.SphereColliderRadius;
            }
        }

        #endregion
    }
}

