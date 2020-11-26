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
        private const float ANGLE_SPEED = 100f;
        private const float ANGLE_TARGET_RANGE_MIN = 10f;



        private const int DEFAULT_ATTACK_ID = 0;
        private const float DEFAULT_ATTACK_RANGE_MIN = 3f;
        private const float DEFAULT_ATTACK_RANGE_MAX = 5f;
        private const float DEFAULT_ATTACK_COOLDAWN = 3f;

        private const int HORIZONTAL_FIST_ATTACK_ID = 1;
        private const float HORIZONTAL_FIST_ATTACK_RANGE_MIN = 3f;
        private const float HORIZONTAL_FIST_ATTACK_RANGE_MAX = 5f;
        private const float HORIZONTAL_FIST_ATTACK_COOLDOWN = 20f;

        private const int STOMP_SPLASH_ATTACK_ID = 2;
        private const float STOMP_SPLASH_ATTACK_RANGE_MAX = 5f;
        private const float STOMP_SPLASH_ATTACK_COOLDOWN = 35f;

        private const int RAGE_OF_FOREST_ATTACK_ID = 3;
        private const float RAGE_OF_FOREST_ATTACK_DURATION = 120f;
        private const float RAGE_OF_FOREST_ATTACK_COOLDOWN = 120f;

        private const int POISON_SPORES_ATTACK_ID = 4;
        private const float POISON_SPORES_RANGE_MIN = 10f;
        private const float POISON_SPORES_RANGE_MAX = 20f;
        private const float POISON_SPORES_COOLDOWN = 20f;

        

        #endregion


        #region Fields

        private Vector3 _lookDirection;
        private Quaternion _toRotation;

        private bool _isCurrentAttackRight;

        private int _attackNumber;

        private float _currentAttackTime;
      //  private float _attackDelay;

        private bool _isDefaultAttackReady = true;
        private bool _isHorizontalFistAttackReady = true;
        private bool _isStompSplashAttackReady = true;
        private bool _isRageOfForestAttackReady = true;
        private bool _isPoisonSporesAttackReady = true;

        private Dictionary<int, bool> skillDictionary = new Dictionary<int, bool>();

        #endregion


        #region ClassLifeCycle

        public BossAttackingState(BossStateMachine stateMachine) : base(stateMachine)
        {
            skillDictionary.Add(DEFAULT_ATTACK_ID, _isDefaultAttackReady);
            skillDictionary.Add(HORIZONTAL_FIST_ATTACK_ID, _isHorizontalFistAttackReady);
            skillDictionary.Add(STOMP_SPLASH_ATTACK_ID, _isStompSplashAttackReady);
            skillDictionary.Add(RAGE_OF_FOREST_ATTACK_ID, _isRageOfForestAttackReady);
            skillDictionary.Add(POISON_SPORES_ATTACK_ID, _isPoisonSporesAttackReady);
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            _stateMachine._model.LeftHandBehavior.OnFilterHandler += OnHitBoxFilter;
            _stateMachine._model.RightHandBehavior.OnFilterHandler += OnHitBoxFilter;
            _stateMachine._model.LeftHandBehavior.OnTriggerEnterHandler += OnLeftHitBoxHit;
            _stateMachine._model.RightHandBehavior.OnTriggerEnterHandler += OnRightHitBoxHit;
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = true;


            _stateMachine._model.BossNavAgent.SetDestination(_stateMachine._model.BossTransform.position);
            _stateMachine._model.BossNavAgent.speed = 0f;
            ChoosingAttackSkill(true);
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
            _stateMachine._model.LeftHandBehavior.OnFilterHandler -= OnHitBoxFilter;
            _stateMachine._model.RightHandBehavior.OnFilterHandler -= OnHitBoxFilter;
            _stateMachine._model.LeftHandBehavior.OnTriggerEnterHandler -= OnLeftHitBoxHit;
            _stateMachine._model.RightHandBehavior.OnTriggerEnterHandler -= OnRightHitBoxHit;
        }

        private void ChoosingAttackSkill(bool isDefault = false)
        {
            _currentAttackTime = 1.5f;
            //_attackDelay = 3f;
            var dic = new Dictionary<int, int>();
            int j = 0;
            for (int i=0; i<skillDictionary.Count; i++)
            {
                if(skillDictionary[i])
                {
                    dic.Add(j, i);
                    j++;
                }
            }

            int skillId;
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
                    HorizontalAttackSkill();
                    break;
                case 2:
                    StompSplashAttackSkill();
                    break;
                case 3:
                    RageOfForestAttackSkill();
                    break;
                case 4:
                    PoisonSporesAttackSkill();
                    break;
                default:
                    DefaultAttackSkill();
                    break;
            }
        }

        private void DefaultAttackSkill()
        {
            Debug.Log("DefaultAttackSkill");
            _stateMachine._model.BossAnimator.Play("BossFeastsAttack_1", 0, 0f);
            TurnOnHitBox(_stateMachine._model.LeftHandBehavior, PART_OF_NONE_ATTACK_TIME_LEFT);
            skillDictionary[DEFAULT_ATTACK_ID] = false;
            SkillCooldown(DEFAULT_ATTACK_ID, DEFAULT_ATTACK_COOLDAWN);
        }

        private void HorizontalAttackSkill()
        {
            Debug.Log("HorizontalAttackSkill");
            _stateMachine._model.BossAnimator.Play("BossFeastsAttack_0", 0, 0f);
            TurnOnHitBox(_stateMachine._model.RightHandBehavior, PART_OF_NONE_ATTACK_TIME_RIGHT);
            skillDictionary[HORIZONTAL_FIST_ATTACK_ID] = false;
            SkillCooldown(HORIZONTAL_FIST_ATTACK_ID, HORIZONTAL_FIST_ATTACK_COOLDOWN);
            
        }

        private void StompSplashAttackSkill()
        {
            Debug.Log("StompAttackSkill");
            _stateMachine._model.BossAnimator.Play("BossStompAttack", 0, 0f);
            skillDictionary[STOMP_SPLASH_ATTACK_ID] = false;
            SkillCooldown(STOMP_SPLASH_ATTACK_ID, STOMP_SPLASH_ATTACK_COOLDOWN);

        }
        
        private void RageOfForestAttackSkill()
        {
            Debug.Log("RAGEAttackSkill");
            skillDictionary[RAGE_OF_FOREST_ATTACK_ID] = false;
            SkillCooldown(RAGE_OF_FOREST_ATTACK_ID, RAGE_OF_FOREST_ATTACK_COOLDOWN);
        }

        private void PoisonSporesAttackSkill()
        {
            Debug.Log("POISONAttackSkill");
            skillDictionary[POISON_SPORES_ATTACK_ID] = false;
            SkillCooldown(POISON_SPORES_ATTACK_ID, POISON_SPORES_COOLDOWN);
        }


        private void CheckNextMove()
        {
          //  _attackDelay -= Time.deltaTime;
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
            bool isNear = Quaternion.Angle(_stateMachine._model.BossTransform.rotation,
                _stateMachine._mainState.TargetRotation) <= BossMainState.ANGLE_TARGET_RANGE;

            if (!isNear)
            {
                CheckTargetDirection();
                TargetOnPlayer();
            }

            return isNear;
        }

        private bool CheckDistance()
        {
            bool isNear = Mathf.Sqrt((_stateMachine._model.BossTransform.position - _stateMachine.
                _context.CharacterModel.CharacterTransform.position).sqrMagnitude) <= BossMainState.DISTANCE_TO_START_ATTACK;

            if (!isNear)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
            }

            return isNear;
        }

        private void DecideNextMove()
        {
            _stateMachine._model.LeftHandBehavior.IsInteractable = false;
            _stateMachine._model.RightHandBehavior.IsInteractable = false;

            if (!_stateMachine._model.IsDead && CheckDirection() && CheckDistance())
            {
               // if (_attackDelay <= 0)
               // {
                    ChoosingAttackSkill(); // Initialise();
               // }
            }
        }

        private void TurnOnHitBox(WeaponHitBoxBehavior hitBox, float delayTime)
        {
           // TimeRemaining enableHitBox = new TimeRemaining(() => hitBox.IsInteractable = true, _currentAttackTime * delayTime);
         //   enableHitBox.AddTimeRemaining(_currentAttackTime * delayTime);
        }

        private void SkillCooldown(int skillId, float coolDownTime)
        {
            TimeRemaining currentSkill = new TimeRemaining(() => skillDictionary[skillId] = true, coolDownTime);
            currentSkill.AddTimeRemaining(coolDownTime);
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
                    CountDamage(_stateMachine._model.WeaponData, _stateMachine._model.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.PlayerBehavior.Stats));
                hitBox.IsInteractable = false;
            }
        }

        private void OnRightHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (hitBox.IsInteractable)
            {
                DealDamage(_stateMachine._context.CharacterModel.PlayerBehavior, Services.SharedInstance.AttackService.
                    CountDamage(_stateMachine._model.WeaponData, _stateMachine._model.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.PlayerBehavior.Stats));
                hitBox.IsInteractable = false;
            }
        }

        private void CheckTargetDirection()
        {
            Vector3 heading = _stateMachine._context.CharacterModel.CharacterTransform.position -
                _stateMachine._model.BossTransform.position;
            int directionNumber = _stateMachine._mainState.AngleDirection(
                _stateMachine._model.BossTransform.forward, heading, _stateMachine._model.BossTransform.up);

            switch (directionNumber)
            {
                case -1:
                    _stateMachine._model.BossAnimator.Play("TurningLeftState", 0, 0f);
                    break;
                case 0:
                    _stateMachine._model.BossAnimator.Play("IdleState", 0, 0f);
                    break;
                case 1:
                    _stateMachine._model.BossAnimator.Play("TurningRightState", 0, 0f);
                    break;
                default:
                    _stateMachine._model.BossAnimator.Play("IdleState", 0, 0f);
                    break;
            }
        }


        private void TargetOnPlayer()
        {
            if (Quaternion.Angle(_stateMachine._model.BossTransform.rotation,
                _stateMachine._mainState.TargetRotation) > ANGLE_TARGET_RANGE_MIN)
            {
                _stateMachine._model.BossTransform.rotation = Quaternion.RotateTowards(_stateMachine.
                    _model.BossTransform.rotation, _stateMachine._mainState.TargetRotation, ANGLE_SPEED * Time.deltaTime);
            }
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

