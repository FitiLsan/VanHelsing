using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public abstract class BossBaseSkill
    {
        #region Fields

        private bool _isEnable;
        private int _skillId;
        private float _skillRangeMin;
        private float _skillRangeMax;
        private float _skillCooldown;
        private bool _isCooldownStart;
        private bool _isSkillReady;
        private bool _canInterrupt;
        private bool _isNeedRage;
        private GameObject _prefab;
        private float _damage;
        private int _count;

        protected BossModel _bossModel;
        protected BossData _bossData;
        protected WeaponHitBoxBehavior _currenTriggertHand;
        protected WeaponHitBoxBehavior _rightFingerTrigger;
        protected Collider _currenColliderHand;
        protected Dictionary<int, BossBaseSkill> _skillDictionary;
        protected BossStateMachine _stateMachine;

        #endregion

        #region ClassLifeCycle

        public BossBaseSkill(bool isEnable, int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _isEnable = isEnable;
            _skillId = Id;
            _skillRangeMin = RangeMin;
            _skillRangeMax = RangeMax;
            _skillCooldown = Cooldown;
            _isSkillReady = IsReady;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _bossData = _bossModel.BossData;
            _skillDictionary = skillDictionary;
        }

        public BossBaseSkill ((bool, int, float, float, float, bool, bool, float) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _isEnable = skillInfo.Item1;
            _skillId = skillInfo.Item2;
            _skillRangeMin = skillInfo.Item3;
            _skillRangeMax = skillInfo.Item4;
            _skillCooldown = skillInfo.Item5;
            _isSkillReady = skillInfo.Item6;
            _canInterrupt = skillInfo.Item7;
            _damage = skillInfo.Item8;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }
        public BossBaseSkill((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _isEnable = skillInfo.Item1;
            _skillId = skillInfo.Item2;
            _skillRangeMin = skillInfo.Item3;
            _skillRangeMax = skillInfo.Item4;
            _skillCooldown = skillInfo.Item5;
            _isSkillReady = skillInfo.Item6;
            _canInterrupt = skillInfo.Item7;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }

        public BossBaseSkill((bool, int, float, float, float, bool, bool, GameObject, float) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _isEnable = skillInfo.Item1;
            _skillId = skillInfo.Item2;
            _skillRangeMin = skillInfo.Item3;
            _skillRangeMax = skillInfo.Item4;
            _skillCooldown = skillInfo.Item5;
            _isSkillReady = skillInfo.Item6;
            _canInterrupt = skillInfo.Item7;
            _prefab = skillInfo.Item8;
            _damage = skillInfo.Item9;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }

        public BossBaseSkill((bool, int, float, float, float, bool, bool, GameObject, int) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _isEnable = skillInfo.Item1;
            _skillId = skillInfo.Item2;
            _skillRangeMin = skillInfo.Item3;
            _skillRangeMax = skillInfo.Item4;
            _skillCooldown = skillInfo.Item5;
            _isSkillReady = skillInfo.Item6;
            _canInterrupt = skillInfo.Item7;
            _prefab = skillInfo.Item8;
            _count = skillInfo.Item9;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }

        public BossBaseSkill((bool, int, float, float, float, bool, bool, GameObject) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine)
        {
            _isEnable = skillInfo.Item1;
            _skillId = skillInfo.Item2;
            _skillRangeMin = skillInfo.Item3;
            _skillRangeMax = skillInfo.Item4;
            _skillCooldown = skillInfo.Item5;
            _isSkillReady = skillInfo.Item6;
            _canInterrupt = skillInfo.Item7;
            _prefab = skillInfo.Item8;
            _stateMachine = stateMachine;
            _bossModel = stateMachine._model;
            _skillDictionary = skillDictionary;
        }

        #endregion

        #region Properties

        public bool IsEnable => _isEnable;
        public int SkillId => _skillId;

        public float SkillRangeMin => _skillRangeMin;

        public float SkillRangeMax => _skillRangeMax;

        public float SkillCooldown => _skillCooldown;

        public bool IsNeedRage { get; set; }

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
        public GameObject SkillPrefab => _prefab;
        public bool IsSkillUsing { get; set; }
        public float SkillDamage => _damage;
        public int SkillCount => _count;


        #endregion

        public virtual void UseSkill(int id)
        {
            IsSkillUsing = true;
        }

        public virtual void StopSkill()
        {
            IsSkillUsing = false;
        }

        public virtual void StartCooldown(int skillId, float coolDownTime)
        {
            if (!_skillDictionary[skillId].IsCooldownStart && !_skillDictionary[skillId].IsSkillReady)
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

        protected void TurnOnHitBoxTrigger(WeaponHitBoxBehavior hitBox, float currentAttackTime, float delayTime, float Damage)
        {
            hitBox.TempDamage = Damage;
            TimeRemaining enableHitBox = new TimeRemaining(() => hitBox.IsInteractable = true, currentAttackTime * delayTime);
            enableHitBox.AddTimeRemaining(currentAttackTime * delayTime);
        }

        protected void TurnOnHitBoxCollider(Collider hitBox, float currentAttackTime, float delayTime, bool isOn = true)
        {
            TimeRemaining enableHitBox = new TimeRemaining(() => hitBox.enabled = isOn, currentAttackTime * delayTime);
            enableHitBox.AddTimeRemaining(currentAttackTime * delayTime);
        }

        internal virtual void SwitchAllowed(bool v)
        {
            
        }

        protected virtual void ReloadSkill(int id)
        {
            _skillDictionary[id].IsSkillReady = false;
            StartCooldown(id, _skillDictionary[id].SkillCooldown);
            _stateMachine.CurrentState.IsAnimationPlay = true;
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