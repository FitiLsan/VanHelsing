using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public abstract class BaseSkill
    {
        #region Fields

        private int _attackId;
        private float _attackRangeMin;
        private float _attackRangeMax;
        private float _attackCooldown;
        private bool _isCooldownStart;
        private bool _isAttackReady;
        protected BossModel _bossModel;
        protected WeaponHitBoxBehavior _currenTriggertHand;
        protected Collider _currenColliderHand;
        protected Dictionary<int, BaseSkill> _skillDictionary;
        protected BossStateMachine _stateMachine;

        #endregion

        #region ClassLifeCycle

        public BaseSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _attackId = Id;
            _attackRangeMin = RangeMin;
            _attackRangeMax = RangeMax;
            _attackCooldown = Cooldown;
            _isAttackReady = IsReady;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }

        #endregion

        #region Properties

        public int AttackId => _attackId;

        public float AttackRangeMin => _attackRangeMin;

        public float AttackRangeMax => _attackRangeMax;

        public float AttackCooldown => _attackCooldown;

        public bool IsAttackReady
        {
            get
            {
                return _isAttackReady;
            }
            set
            {
                _isAttackReady = value;
            }
        }

        public bool IsCooldownStart
        {
            get
            {
                return _isCooldownStart;
            }
            set
            {
                _isCooldownStart = value;
            }
        }
        #endregion

        public abstract void UseSkill(int id);

        public virtual void SkillCooldown(int skillId, float coolDownTime)
        {
            if (!_skillDictionary[skillId].IsCooldownStart & !_skillDictionary[skillId].IsAttackReady)
            {
                TimeRemaining currentSkill = new TimeRemaining(() => SetReadySkill(skillId), coolDownTime);
                currentSkill.AddTimeRemaining(coolDownTime);
                _skillDictionary[skillId].IsCooldownStart = true;
            }
        }

        private void SetReadySkill(int id)
        {
            _skillDictionary[id].IsAttackReady = true;
            _skillDictionary[id].IsCooldownStart = false;
        }

        protected void TurnOnHitBoxTrigger(WeaponHitBoxBehavior hitBox, float currentAttackTime, float delayTime)
        {
            //TimeRemaining enableHitBox = new TimeRemaining(() => hitBox.IsInteractable = true, currentAttackTime * delayTime);
            //enableHitBox.AddTimeRemaining(currentAttackTime * delayTime);
        }

        protected void TurnOnHitBoxCollider(Collider hitBox, float currentAttackTime, float delayTime, bool isOn = true)
        {
            TimeRemaining enableHitBox = new TimeRemaining(() => hitBox.enabled = isOn, currentAttackTime * delayTime);
            enableHitBox.AddTimeRemaining(currentAttackTime * delayTime);
        }

        protected void SetNavMeshAgent(Vector3 targetPosition, float speed)
        {
            _bossModel.BossNavAgent.SetDestination(targetPosition);
            _bossModel.BossNavAgent.speed = speed;
        }
    }
}