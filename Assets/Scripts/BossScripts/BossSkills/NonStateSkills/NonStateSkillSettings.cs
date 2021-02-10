using System;
using UnityEngine;


namespace BeastHunter
{

    [Serializable]
    public struct NonStateSkillsSettings
    {
        #region Fields

        [Header("[Test Attack]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _testSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _testSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _testSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _testSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _testSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _testSkillReady;

        [Header("[Throw Attack]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _throwSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _throwSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _throwSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _throwSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _throwSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _throwSkillReady;

        [Header("[Rage Of Forest]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _rageOfForestSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _rageOfForestSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _rageOfForestSkillRangeMin;
        [Range(-1.0f, 30.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _rageOfForestSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _rageOfForestSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _rageOfForestSkillReady;

        #endregion


        #region Properties

        public bool TestSkillEnable => _testSkillEnable;
        public int TestSkillId => _testSkillId;
        public float TestSkillRangeMin => _testSkillRangeMin;
        public float TestSkillRangeMax => _testSkillRangeMax;
        public float TestSkillCooldown => _testSkillCooldown;
        public bool TestSkillReady => _testSkillReady;

        public bool ThrowSkillEnable => _throwSkillEnable;
        public int ThrowSkillId => _throwSkillId;
        public float ThrowSkillRangeMin => _throwSkillRangeMin;
        public float ThrowSkillRangeMax => _throwSkillRangeMax;
        public float ThrowSkillCooldown => _throwSkillCooldown;
        public bool ThrowSkillReady => _throwSkillReady;

        public bool RageOfForestSkillEnable => _rageOfForestSkillEnable;
        public int RageOfForestSkillId => _rageOfForestSkillId;
        public float RageOfForestSkillRangeMin => _rageOfForestSkillRangeMin;
        public float RageOfForestSkillRangeMax => _rageOfForestSkillRangeMax;
        public float RageOfForestSkillCooldown => _rageOfForestSkillCooldown;
        public bool RageOfForestSkillReady => _rageOfForestSkillReady;

        #endregion


        #region Methods

        public (bool, int, float, float, float, bool, bool) GetTestSkillInfo()
        {
            var tuple = (TestSkillEnable, TestSkillId, TestSkillRangeMin, TestSkillRangeMax, TestSkillCooldown, TestSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetThrowSkillInfo()
        {
            var tuple = (ThrowSkillEnable, ThrowSkillId, ThrowSkillRangeMin, ThrowSkillRangeMax, ThrowSkillCooldown, ThrowSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetRageOfForestSkillInfo()
        {
            var tuple = (RageOfForestSkillEnable, RageOfForestSkillId, RageOfForestSkillRangeMin, RageOfForestSkillRangeMax, RageOfForestSkillCooldown, RageOfForestSkillReady, false);
            return tuple;
        }
        #endregion
    }
}