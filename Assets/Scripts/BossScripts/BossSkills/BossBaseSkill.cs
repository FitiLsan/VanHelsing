using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public abstract class BossBaseSkill
    {
        #region Fields

        private int _skillId;
        private float _skillRangeMin;
        private float _skillRangeMax;
        private float _skillCooldown;
        private bool _isCooldownStart;
        private bool _isSkillReady;
        private bool _canInterrupt;
        private GameObject _prefab;

        protected BossModel _bossModel;
        protected WeaponHitBoxBehavior _currenTriggertHand;
        protected WeaponHitBoxBehavior _rightFingerTrigger;
        protected Collider _currenColliderHand;
        protected Dictionary<int, BossBaseSkill> _skillDictionary;
        protected BossStateMachine _stateMachine;

        #endregion

        #region ClassLifeCycle

        public BossBaseSkill(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _skillId = Id;
            _skillRangeMin = RangeMin;
            _skillRangeMax = RangeMax;
            _skillCooldown = Cooldown;
            _isSkillReady = IsReady;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }

        public BossBaseSkill ((int, float, float, float, bool, bool)skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _skillId = skillInfo.Item1;
            _skillRangeMin = skillInfo.Item2;
            _skillRangeMax = skillInfo.Item3;
            _skillCooldown = skillInfo.Item4;
            _isSkillReady = skillInfo.Item5;
            _canInterrupt = skillInfo.Item6;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }

        public BossBaseSkill((int, float, float, float, bool, bool, GameObject) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _skillId = skillInfo.Item1;
            _skillRangeMin = skillInfo.Item2;
            _skillRangeMax = skillInfo.Item3;
            _skillCooldown = skillInfo.Item4;
            _isSkillReady = skillInfo.Item5;
            _canInterrupt = skillInfo.Item6;
            _prefab = skillInfo.Item7;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }

        #endregion

        #region Properties

        public int SkillId => _skillId;

        public float SkillRangeMin => _skillRangeMin;

        public float SkillRangeMax => _skillRangeMax;

        public float SkillCooldown => _skillCooldown;

        public bool IsSkillReady
        {
            get
            {
                return _isSkillReady;
            }
            set
            {
                _isSkillReady = value;
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

        public bool CanInterrupt => _canInterrupt;

        #endregion

        public abstract void UseSkill(int id);

        public abstract void StopSkill();

        public virtual void StartCooldown(int skillId, float coolDownTime)
        {
            if (!_skillDictionary[skillId].IsCooldownStart & !_skillDictionary[skillId].IsSkillReady)
            {
                TimeRemaining currentSkill = new TimeRemaining(() => SetReadySkill(skillId), coolDownTime);
                currentSkill.AddTimeRemaining(coolDownTime);
                _skillDictionary[skillId].IsCooldownStart = true;
            }
        }

        private void SetReadySkill(int id)
        {
            _skillDictionary[id].IsSkillReady = true;
            _skillDictionary[id].IsCooldownStart = false;
        }

        protected void TurnOnHitBoxTrigger(WeaponHitBoxBehavior hitBox, float currentAttackTime, float delayTime)
        {
           // TimeRemaining enableHitBox = new TimeRemaining(() => hitBox.IsInteractable = true, currentAttackTime * delayTime);
           // enableHitBox.AddTimeRemaining(currentAttackTime * delayTime);
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

        internal virtual void SwitchAllowed(bool v)
        {
            
        }

        protected virtual void ReloadSkill(int id)
        {
            _skillDictionary[id].IsSkillReady = false;
            StartCooldown(id, _skillDictionary[id].SkillCooldown);
            _stateMachine.CurrentState.isAnimationPlay = true;
        }

        protected virtual void DelayCall(Action action, float delayTime, out TimeRemaining delayCall, bool isRepete=false)
        {
            TimeRemaining timeRemaining = new TimeRemaining(() => action(), delayTime, isRepete);
            timeRemaining.AddTimeRemaining(delayTime);
            delayCall = timeRemaining;
        }

        protected virtual void DelayCall(Action action, float delayTime, bool isRepete = false)
        {
            TimeRemaining timeRemaining = new TimeRemaining(() => action(), delayTime, isRepete);
            timeRemaining.AddTimeRemaining(delayTime);
        }

        protected virtual void RotateToTarget ()
        {
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform, 1, true);
        }
    }
}