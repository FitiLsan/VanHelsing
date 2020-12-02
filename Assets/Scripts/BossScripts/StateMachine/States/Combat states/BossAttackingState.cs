using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class BossAttackingState : BossBaseState, IDealDamage
    {
        #region Constants

        private const float LOOK_TO_TARGET_SPEED = 1f;
        private const float PART_OF_NONE_ATTACK_TIME_LEFT = 0.15f;
        private const float PART_OF_NONE_ATTACK_TIME_RIGHT = 0.3f;
        private const float ANGLE_SPEED = 150f;
        private const float ANGLE_TARGET_RANGE_MIN = 20f;
        private const float DISTANCE_TO_START_ATTACK = 4f;



        private const int DEFAULT_ATTACK_ID = 0;
        private const float DEFAULT_ATTACK_RANGE_MIN = 3f;
        private const float DEFAULT_ATTACK_RANGE_MAX = 5f;
        private const float DEFAULT_ATTACK_COOLDOWN = 3f; //3

        private const int HORIZONTAL_FIST_ATTACK_ID = 1;
        private const float HORIZONTAL_FIST_ATTACK_RANGE_MIN = 3f;
        private const float HORIZONTAL_FIST_ATTACK_RANGE_MAX = 5f;
        private const float HORIZONTAL_FIST_ATTACK_COOLDOWN = 7f; //20f

        private const int STOMP_SPLASH_ATTACK_ID = 2;
        private const float STOMP_SPLASH_ATTACK_RANGE_MIN = 0f;
        private const float STOMP_SPLASH_ATTACK_RANGE_MAX = 5f;
        private const float STOMP_SPLASH_ATTACK_COOLDOWN = 5f;

        private const int RAGE_OF_FOREST_ATTACK_ID = 3;
        private const float RAGE_OF_FOREST_ATTACK_RANGE_MIN = -1f;
        private const float RAGE_OF_FOREST_ATTACK_RANGE_MAX = -1f;
        private const float RAGE_OF_FOREST_ATTACK_DURATION = 10f;
        private const float RAGE_OF_FOREST_ATTACK_COOLDOWN = 30f; //120

        private const int POISON_SPORES_ATTACK_ID = 4;
        private const float POISON_SPORES_ATTACK_RANGE_MIN = 10f;
        private const float POISON_SPORES_ATTACK_RANGE_MAX = 20f;
        private const float POISON_SPORES_ATTACK_COOLDOWN = 15f; //20


        SkillInfoStruct _defaultSkill;
        SkillInfoStruct _horizontalFirstSkill;
        SkillInfoStruct _stompSplashSkill;
        SkillInfoStruct _rageOfForestSkill;
        SkillInfoStruct _poisonSporesSkill;

        #endregion


        #region Fields

        private Vector3 _lookDirection;
        private Quaternion _toRotation;

        private bool _isCurrentAttackRight;

        private int _attackNumber;

        private float _currentAttackTime;

        private bool _isDefaultAttackReady = false;
        private bool _isHorizontalFistAttackReady = false;
        private bool _isStompSplashAttackReady = false;
        private bool _isRageOfForestAttackReady = false;
        private bool _isPoisonSporesAttackReady = true;

        private Dictionary<int, SkillInfoStruct> TestSkillDictionary = new Dictionary<int, SkillInfoStruct>();

        private bool isAnimationPlay;
        #endregion


        #region ClassLifeCycle

        public BossAttackingState(BossStateMachine stateMachine) : base(stateMachine)
        {
            _defaultSkill = new SkillInfoStruct(DEFAULT_ATTACK_ID, DEFAULT_ATTACK_RANGE_MIN, DEFAULT_ATTACK_RANGE_MAX, DEFAULT_ATTACK_COOLDOWN, _isDefaultAttackReady);
            _horizontalFirstSkill = new SkillInfoStruct(HORIZONTAL_FIST_ATTACK_ID, HORIZONTAL_FIST_ATTACK_RANGE_MIN, HORIZONTAL_FIST_ATTACK_RANGE_MAX, HORIZONTAL_FIST_ATTACK_COOLDOWN, _isHorizontalFistAttackReady);
            _stompSplashSkill = new SkillInfoStruct(STOMP_SPLASH_ATTACK_ID, STOMP_SPLASH_ATTACK_RANGE_MIN, STOMP_SPLASH_ATTACK_RANGE_MAX, STOMP_SPLASH_ATTACK_COOLDOWN, _isStompSplashAttackReady);
            _rageOfForestSkill = new SkillInfoStruct(RAGE_OF_FOREST_ATTACK_ID, RAGE_OF_FOREST_ATTACK_RANGE_MIN, RAGE_OF_FOREST_ATTACK_RANGE_MAX, RAGE_OF_FOREST_ATTACK_COOLDOWN, _isRageOfForestAttackReady);
            _poisonSporesSkill = new SkillInfoStruct(POISON_SPORES_ATTACK_ID, POISON_SPORES_ATTACK_RANGE_MIN, POISON_SPORES_ATTACK_RANGE_MAX, POISON_SPORES_ATTACK_COOLDOWN, _isPoisonSporesAttackReady);

            TestSkillDictionary.Add(_defaultSkill.AttackId, _defaultSkill);
            TestSkillDictionary.Add(_horizontalFirstSkill.AttackId, _horizontalFirstSkill);
            TestSkillDictionary.Add(_stompSplashSkill.AttackId, _stompSplashSkill);
            TestSkillDictionary.Add(_rageOfForestSkill.AttackId, _rageOfForestSkill);
            TestSkillDictionary.Add(_poisonSporesSkill.AttackId, _poisonSporesSkill);
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            _bossModel.LeftHandBehavior.OnFilterHandler += OnHitBoxFilter;
            _bossModel.RightHandBehavior.OnFilterHandler += OnHitBoxFilter;
            _bossModel.LeftHandBehavior.OnTriggerEnterHandler += OnLeftHitBoxHit;
            _bossModel.RightHandBehavior.OnTriggerEnterHandler += OnRightHitBoxHit;
        }

        public override void Initialise()
        {
            Debug.Log($"current State ATTACK initialise");
            CanExit = false;
            CanBeOverriden = true;
            IsBattleState = true;

            SetNavMeshAgent(_bossModel.BossTransform.position, 0);

            for (var i = 0; i < TestSkillDictionary.Count; i++)
            {
                SkillCooldown(TestSkillDictionary[i].AttackId, TestSkillDictionary[i].AttackCooldown);
            }
            ChoosingAttackSkill();
        }

        public override void Execute()
        {
            CheckNextMove();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
            _bossModel.LeftHandBehavior.OnFilterHandler -= OnHitBoxFilter;
            _bossModel.RightHandBehavior.OnFilterHandler -= OnHitBoxFilter;
            _bossModel.LeftHandBehavior.OnTriggerEnterHandler -= OnLeftHitBoxHit;
            _bossModel.RightHandBehavior.OnTriggerEnterHandler -= OnRightHitBoxHit;
        }

        private void ChoosingAttackSkill(bool isDefault = false)
        {
            var dic = new Dictionary<int, int>();
            var j = 0;


            for (var i = 0; i < TestSkillDictionary.Count; i++)
            {
                if (TestSkillDictionary[i].IsAttackReady)
                {
                    if (CheckDistance(TestSkillDictionary[i].AttackRangeMin, TestSkillDictionary[i].AttackRangeMax))
                    {
                        dic.Add(j, i);
                        j++;
                    }
                }
            }

            int skillId;

            if(dic.Count==0 & _bossData.GetTargetDistance(_bossModel.BossTransform.position, _bossModel.BossCurrentTarget.transform.position)>=DISTANCE_TO_START_ATTACK)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
                return;
            }

            if (!isDefault & dic.Count!=0)
            {
                var readyId = UnityEngine.Random.Range(0, dic.Count);
                skillId = dic[readyId];
            }
            else
            {
                skillId = DEFAULT_ATTACK_ID;
            }
            switch (skillId)
            {
                case 1:
                    HorizontalAttackSkill(skillId);
                    break;
                case 2:
                    StompSplashAttackSkill(skillId);
                    break;
                case 3:
                    RageOfForestAttackSkill(skillId);
                    break;
                case 4:
                    PoisonSporesAttackSkill(skillId);
                    break;
                default:
                    DefaultAttackSkill(skillId);
                    break;
            }
        }
        #region Skills

        private void DefaultAttackSkill(int id)
        {
            Debug.Log("DefaultAttackSkill");
            
            _bossModel.BossAnimator.Play("BossFeastsAttack_1", 0, 0f);
            TurnOnHitBoxTrigger(_bossModel.LeftHandBehavior, PART_OF_NONE_ATTACK_TIME_LEFT);
            TestSkillDictionary[id].IsAttackReady = false;
            SkillCooldown(id, TestSkillDictionary[id].AttackCooldown);
            isAnimationPlay = true;
        }

        private void HorizontalAttackSkill(int id)
        {
            Debug.Log("HorizontalAttackSkill");
            _bossModel.BossAnimator.Play("BossFeastsAttack_0", 0, 0f);
            TurnOnHitBoxTrigger(_bossModel.RightHandBehavior, PART_OF_NONE_ATTACK_TIME_RIGHT);
            TurnOnHitBoxCollider(_bossModel.RightHandCollider, PART_OF_NONE_ATTACK_TIME_RIGHT);
            TestSkillDictionary[id].IsAttackReady = false;
            SkillCooldown(id, TestSkillDictionary[id].AttackCooldown);
            isAnimationPlay = true;
        }

        private void StompSplashAttackSkill(int id)
        {
            Debug.Log("StompAttackSkill");
            _bossModel.BossAnimator.Play("BossStompAttack", 0, 0f);
            var TimeRem = new TimeRemaining(() => _bossModel.leftStompEffect.Play(true), 0.65f);
            TimeRem.AddTimeRemaining(0.65f);

            TestSkillDictionary[id].IsAttackReady = false;
            SkillCooldown(id, TestSkillDictionary[id].AttackCooldown);
            isAnimationPlay = true;
        }

        private void RageOfForestAttackSkill(int id)
        {
            Debug.Log("RAGEAttackSkill");
            SetNavMeshAgent(_bossModel.BossCurrentTarget.transform.position, 5f);
            _bossModel.BossAnimator.Play("BossRageAttack", 0, 0f);

            TurnOnHitBoxTrigger(_bossModel.RightHandBehavior, PART_OF_NONE_ATTACK_TIME_RIGHT);
            TurnOnHitBoxCollider(_bossModel.RightHandCollider, PART_OF_NONE_ATTACK_TIME_RIGHT);
            TurnOnHitBoxTrigger(_bossModel.LeftHandBehavior, PART_OF_NONE_ATTACK_TIME_RIGHT);
            TurnOnHitBoxCollider(_bossModel.LeftHandCollider, PART_OF_NONE_ATTACK_TIME_RIGHT);

            TestSkillDictionary[id].IsAttackReady = false;
            SkillCooldown(id, TestSkillDictionary[id].AttackCooldown);
            _currentAttackTime = RAGE_OF_FOREST_ATTACK_DURATION;
          //  isAnimationPlay = true;
        }

        private void PoisonSporesAttackSkill(int id)
        {
            Debug.Log("POISONAttackSkill");
            SetNavMeshAgent(_bossModel.BossCurrentTarget.transform.position, 0f);
            _bossModel.BossAnimator.Play("PoisonAttack", 0, 0f);
            TestSkillDictionary[id].IsAttackReady = false;
            SkillCooldown(id, TestSkillDictionary[id].AttackCooldown);
            isAnimationPlay = true;
            CreateSpores();
        }


        //poisonlogic
        private void CreateSpores()
        {
            var bossPos = _bossModel.BossTransform.position;
            var targetPos = _bossModel.BossCurrentTarget.transform.position;
            var distance = _bossData.GetTargetDistance(bossPos, targetPos);
            var shortDistance = (int)distance / 2;
            var vector = targetPos - bossPos;
            var shortVector = vector / shortDistance;
            for (var j = 1; j <= shortDistance + 3; j++)
            {
                float horizontalOffset = UnityEngine.Random.Range(-2f, 2f);
                if (j%2==0)
                {
                    horizontalOffset *= -1;
                }
                var multPos = shortVector * j + new Vector3(horizontalOffset, 0, 0);
                var groundedPosition = Services.SharedInstance.PhysicsService.GetGroundedPosition(bossPos + multPos, 20f);
                var TimeRem = new TimeRemaining(() =>GameObject.Destroy(GameObject.Instantiate(_bossModel.SporePrefab, groundedPosition, Quaternion.identity), 5f), j * 0.1f);
                TimeRem.AddTimeRemaining(j * 0.1f);
            }
        }

        #endregion

        private void CheckNextMove()
        {
            if (isAnimationPlay)
            {
                _currentAttackTime = _bossModel.BossAnimator.GetCurrentAnimatorStateInfo(0).length + 0.2f;
                isAnimationPlay = false;
            }

            if (_currentAttackTime > 0)
            {
                _currentAttackTime -= Time.deltaTime;
                
            }
            if (_currentAttackTime <= 0)
            {
                DecideNextMove();
            }
        }

        private bool CheckDirection()
        {
            var isNear = _bossData.CheckIsLookAtTarget(_bossModel.BossTransform.rotation, _mainState.TargetRotation, ANGLE_TARGET_RANGE_MIN);

            if (!isNear)
            {
                CheckTargetDirection();
                TargetOnPlayer();
            }

            return isNear;
        }

        private bool CheckDistance(float distanceRangeMin, float distanceRangeMax)
        {
            if(distanceRangeMin == -1)
            {
                return true;
            }

            bool isNear = _bossData.CheckIsNearTarget(_bossModel.BossTransform.position, _bossModel.BossCurrentTarget.transform.position, distanceRangeMin, distanceRangeMax);
            return isNear;
        }

        private void DecideNextMove()
        {
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);
            _bossModel.LeftHandBehavior.IsInteractable = false;
            _bossModel.RightHandBehavior.IsInteractable = false;
            _bossModel.LeftHandCollider.enabled = false;
            _bossModel.RightHandCollider.enabled = false;

            if (!_bossModel.IsDead && CheckDirection()) //&& CheckDistance())
            {
                ChoosingAttackSkill();
            }
        }

        private void SetNavMeshAgent(Vector3 targetPosition, float speed)
        {
            _bossModel.BossNavAgent.SetDestination(targetPosition);
            _bossModel.BossNavAgent.speed = speed;
        }

        private void TurnOnHitBoxTrigger(WeaponHitBoxBehavior hitBox, float delayTime)
        {
            //TimeRemaining enableHitBox = new TimeRemaining(() => hitBox.IsInteractable = true, _currentAttackTime * delayTime);
            //enableHitBox.AddTimeRemaining(_currentAttackTime * delayTime);
        }

        private void TurnOnHitBoxCollider(Collider hitBox, float delayTime, bool isOn = true)
        {
            TimeRemaining enableHitBox = new TimeRemaining(() => hitBox.enabled = isOn, _currentAttackTime * delayTime);
            enableHitBox.AddTimeRemaining(_currentAttackTime * delayTime);
        }

        private void SkillCooldown(int skillId, float coolDownTime)
        {
            if (!TestSkillDictionary[skillId].IsCooldownStart & !TestSkillDictionary[skillId].IsAttackReady)
            {
                TimeRemaining currentSkill = new TimeRemaining(() => SetReadySkill(skillId), coolDownTime);
                currentSkill.AddTimeRemaining(coolDownTime);
                TestSkillDictionary[skillId].IsCooldownStart = true;
            }
        }

        private void SetReadySkill(int id)
        {
            TestSkillDictionary[id].IsAttackReady = true;
            TestSkillDictionary[id].IsCooldownStart = false;
        }

        private bool OnHitBoxFilter(Collider hitedObject)
        {         
            bool isEnemyColliderHit = hitedObject.CompareTag(TagManager.PLAYER);

            if (hitedObject.isTrigger || _stateMachine.CurrentState != _stateMachine.States[BossStatesEnum.Attacking])
            {
                isEnemyColliderHit = false;
            }

            return isEnemyColliderHit;
        }

        private void OnLeftHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (hitBox.IsInteractable)
            {
                DealDamage(_stateMachine._context.CharacterModel.PlayerBehavior, Services.SharedInstance.AttackService.
                    CountDamage(_bossModel.WeaponData, _bossModel.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.PlayerBehavior.Stats));
                hitBox.IsInteractable = false;
               // TurnOnHitBoxCollider(_bossModel.LeftHandCollider, 0.2f, false);
            }
        }

        private void OnRightHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (hitBox.IsInteractable)
            {
                DealDamage(_stateMachine._context.CharacterModel.PlayerBehavior, Services.SharedInstance.AttackService.
                    CountDamage(_bossModel.WeaponData, _bossModel.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.PlayerBehavior.Stats));
                hitBox.IsInteractable = false;
               // TurnOnHitBoxCollider(_bossModel.RightHandCollider, 0.2f, false);
            }
        }

        private void CheckTargetDirection()
        {
            Vector3 heading = _bossModel.BossCurrentTarget.transform.position -
                _bossModel.BossTransform.position;

            int directionNumber = _bossData.AngleDirection(
                _bossModel.BossTransform.forward, heading, _bossModel.BossTransform.up);

            switch (directionNumber)
            {
                case -1:
                    _bossModel.BossAnimator.Play("TurningLeftState", 0, 0f);
                    break;
                case 0:
                    _bossModel.BossAnimator.Play("IdleState", 0, 0f);
                    break;
                case 1:
                    _bossModel.BossAnimator.Play("TurningRightState", 0, 0f);
                    break;
                default:
                    _bossModel.BossAnimator.Play("IdleState", 0, 0f);
                    break;
            }
        }


        private void TargetOnPlayer()
        {
            var targetRotation = _bossData.GetTargetRotation(_bossModel.BossTransform.position, _bossModel.BossCurrentTarget.transform.position);
           _bossModel.BossTransform.rotation = _bossData.RotateTo(_bossModel.BossTransform.rotation, targetRotation, ANGLE_SPEED);
        }

        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            if (enemy != null && damage != null)
            {
                enemy.TakeDamageEvent(damage);
            }
        }

        #endregion

        #endregion
    }
}

