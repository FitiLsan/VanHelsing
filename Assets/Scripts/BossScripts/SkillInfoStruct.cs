using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class SkillInfoStruct
    {
        #region Fields

        private int _attackId;
        private float _attackRangeMin;
        private float _attackRangeMax;
        private float _attackCooldown;
        private bool _isAttackReady;
        private bool _isCooldownStart;

        #endregion

        #region ClassLifeCycle

        public SkillInfoStruct(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady)
        {
            _attackId = Id;
            _attackRangeMin = RangeMin;
            _attackRangeMax = RangeMax;
            _attackCooldown = Cooldown;
            _isAttackReady = IsReady;
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
    }
}