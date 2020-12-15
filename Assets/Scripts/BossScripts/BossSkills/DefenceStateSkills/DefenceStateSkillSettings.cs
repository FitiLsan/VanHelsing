using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct DefenceStateSkillsSettings
    {
        #region Fields

        [Header("[Default Defence]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _defaultDefenceSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _defaultDefenceSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _defaultDefenceSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _defaultDefenceSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _defaultDefenceSkillReady;

        [Header("[Self Heal]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _selfHealSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _selfHealSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _selfHealSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _selfHealSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _selfHealSkillReady;

        [Header("[Hard Bark]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _hardBarkSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _hardBarkSkillRangeMin;
        [Range(-1.0f, 30.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _hardBarkSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _hardBarkSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _hardBarkSkillReady;

        [Header("[Canibal Healing]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _canibalHealingSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _canibalHealingSkillRangeMin;
        [Range(-1.0f, 30.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _canibalHealingSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _canibalHealingSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _canibalHealingSkillReady;

        [Header("[Call Of Forest]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _callOfForestSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _callOfForestSkillRangeMin;
        [Range(-1.0f, 30.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _callOfForestSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _callOfForestSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _callOfForestSkillReady;


        #endregion


        #region Properties

        public int DefaultDefenceSkillId => _defaultDefenceSkillId;
        public float DefaultDefenceSkillRangeMin => _defaultDefenceSkillRangeMin;
        public float DefaultDefenceSkillRangeMax => _defaultDefenceSkillRangeMax;
        public float DefaultDefenceSkillCooldown => _defaultDefenceSkillCooldown;
        public bool DefaultDefenceSkillReady => _defaultDefenceSkillReady;

        public int SelfHealSkillId => _selfHealSkillId;
        public float SelfHealSkillRangeMin => _selfHealSkillRangeMin;
        public float SelfHealSkillRangeMax => _selfHealSkillRangeMax;
        public float SelfHealSkillCooldown => _selfHealSkillCooldown;
        public bool SelfHealSkillReady => _selfHealSkillReady;

        public int HardBarkSkillId => _hardBarkSkillId;
        public float HardBarkSkillRangeMin => _hardBarkSkillRangeMin;
        public float HardBarkSkillRangeMax => _hardBarkSkillRangeMax;
        public float HardBarkSkillCooldown => _hardBarkSkillCooldown;
        public bool HardBarkSkillReady => _hardBarkSkillReady;

        public int CanibalHealingSkillId => _canibalHealingSkillId;
        public float CanibalHealingSkillRangeMin => _canibalHealingSkillRangeMin;
        public float CanibalHealingSkillRangeMax => _canibalHealingSkillRangeMax;
        public float CanibalHealingSkillCooldown => _canibalHealingSkillCooldown;
        public bool CanibalHealingSkillReady => _canibalHealingSkillReady;

        public int CallOfForestSkillId => _callOfForestSkillId;
        public float CallOfForestSkillRangeMin => _callOfForestSkillRangeMin;
        public float CallOfForestSkillRangeMax => _callOfForestSkillRangeMax;
        public float CallOfForestSkillCooldown => _callOfForestSkillCooldown;
        public bool CallOfForestSkillReady => _callOfForestSkillReady;

        #endregion


        #region Methods

        public (int, float, float, float, bool) GetDefaultDefencelSkillInfo()
        {
            var tuple = (DefaultDefenceSkillId, DefaultDefenceSkillRangeMin, DefaultDefenceSkillRangeMax, DefaultDefenceSkillCooldown, DefaultDefenceSkillReady);
            return tuple;
        }

        public (int, float, float, float, bool) GetSelfHealSkillInfo()
        {
            var tuple = (SelfHealSkillId, SelfHealSkillRangeMin, SelfHealSkillRangeMax, SelfHealSkillCooldown, SelfHealSkillReady);
            return tuple;
        }

        public (int, float, float, float, bool) GetHardBarkSkillInfo()
        {
            var tuple = ( HardBarkSkillId,  HardBarkSkillRangeMin,  HardBarkSkillRangeMax,  HardBarkSkillCooldown,  HardBarkSkillReady);
            return tuple;
        }

        public (int, float, float, float, bool) GetCanibalHealingSkillInfo()
        {
            var tuple = (CanibalHealingSkillId, CanibalHealingSkillRangeMin, CanibalHealingSkillRangeMax, CanibalHealingSkillCooldown, CanibalHealingSkillReady);
            return tuple;
        }

        public (int, float, float, float, bool) GetCallOfForestSkillInfo()
        {
            var tuple = (CallOfForestSkillId, CallOfForestSkillRangeMin, CallOfForestSkillRangeMax, CallOfForestSkillCooldown, CallOfForestSkillReady);
            return tuple;
        }

        #endregion
    }
}