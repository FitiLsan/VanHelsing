using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct DefenceStateSkillsSettings
    {
        #region Fields

        [Header("[Default Defence]")]


        [Tooltip("Enable")]
        [SerializeField] private bool _defaultDefenceSkillEnable;
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

        [Tooltip("Enable")]
        [SerializeField] private bool _selfHealSkillEnable;
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
        [Tooltip("Skill can Interrupt")]
        [SerializeField] private bool _selfHealSkillCanInterrupt;

        [Header("[Hard Bark]")]


        [Tooltip("Enable")]
        [SerializeField] private bool _hardBarkSkillEnable;
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

        [Tooltip("Enable")]
        [SerializeField] private bool _canibalHealingSkillEnable;
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

        [Tooltip("Enable")]
        [SerializeField] private bool _callOfForestSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _callOfForestSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _callOfForestSkillRangeMin;
        [Range(-1.0f, 100.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _callOfForestSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _callOfForestSkillCooldown;
        [Tooltip("Skill Prefab")]
        [SerializeField] private GameObject _callOfForestSkillPrefab;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _callOfForestSkillReady;

        
        #endregion


        #region Properties

        public bool DefaultDefenceSkillEnable => _defaultDefenceSkillEnable;
        public int DefaultDefenceSkillId => _defaultDefenceSkillId;
        public float DefaultDefenceSkillRangeMin => _defaultDefenceSkillRangeMin;
        public float DefaultDefenceSkillRangeMax => _defaultDefenceSkillRangeMax;
        public float DefaultDefenceSkillCooldown => _defaultDefenceSkillCooldown;
        public bool DefaultDefenceSkillReady => _defaultDefenceSkillReady;

        public bool SelfHealSkillEnable => _selfHealSkillEnable;
        public int SelfHealSkillId => _selfHealSkillId;
        public float SelfHealSkillRangeMin => _selfHealSkillRangeMin;
        public float SelfHealSkillRangeMax => _selfHealSkillRangeMax;
        public float SelfHealSkillCooldown => _selfHealSkillCooldown;
        public bool SelfHealSkillReady => _selfHealSkillReady;
        public bool SelfHealSkillCanInterrupt => _selfHealSkillCanInterrupt;

        public bool HardBarkSkillEnable => _hardBarkSkillEnable;
        public int HardBarkSkillId => _hardBarkSkillId;
        public float HardBarkSkillRangeMin => _hardBarkSkillRangeMin;
        public float HardBarkSkillRangeMax => _hardBarkSkillRangeMax;
        public float HardBarkSkillCooldown => _hardBarkSkillCooldown;
        public bool HardBarkSkillReady => _hardBarkSkillReady;

        public bool CanibalHealingSkillEnable => _canibalHealingSkillEnable;
        public int CanibalHealingSkillId => _canibalHealingSkillId;
        public float CanibalHealingSkillRangeMin => _canibalHealingSkillRangeMin;
        public float CanibalHealingSkillRangeMax => _canibalHealingSkillRangeMax;
        public float CanibalHealingSkillCooldown => _canibalHealingSkillCooldown;
        public bool CanibalHealingSkillReady => _canibalHealingSkillReady;

        public bool CallOfForestSkillEnable => _callOfForestSkillEnable;
        public int CallOfForestSkillId => _callOfForestSkillId;
        public float CallOfForestSkillRangeMin => _callOfForestSkillRangeMin;
        public float CallOfForestSkillRangeMax => _callOfForestSkillRangeMax;
        public float CallOfForestSkillCooldown => _callOfForestSkillCooldown;
        public bool CallOfForestSkillReady => _callOfForestSkillReady;
        public GameObject CallOfForestSkillPrefab => _callOfForestSkillPrefab;


        #endregion


        #region Methods

        public (bool, int, float, float, float, bool, bool) GetDefaultDefencelSkillInfo()
        {
            var tuple = (DefaultDefenceSkillEnable, DefaultDefenceSkillId, DefaultDefenceSkillRangeMin, DefaultDefenceSkillRangeMax, DefaultDefenceSkillCooldown, DefaultDefenceSkillReady , false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetSelfHealSkillInfo()
        {
            var tuple = (SelfHealSkillEnable, SelfHealSkillId, SelfHealSkillRangeMin, SelfHealSkillRangeMax, SelfHealSkillCooldown, SelfHealSkillReady, SelfHealSkillCanInterrupt);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetHardBarkSkillInfo()
        {
            var tuple = (HardBarkSkillEnable, HardBarkSkillId,  HardBarkSkillRangeMin,  HardBarkSkillRangeMax,  HardBarkSkillCooldown,  HardBarkSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetCanibalHealingSkillInfo()
        {
            var tuple = (CanibalHealingSkillEnable, CanibalHealingSkillId, CanibalHealingSkillRangeMin, CanibalHealingSkillRangeMax, CanibalHealingSkillCooldown, CanibalHealingSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool, GameObject) GetCallOfForestSkillInfo()
        {
            var tuple = (CallOfForestSkillEnable, CallOfForestSkillId, CallOfForestSkillRangeMin, CallOfForestSkillRangeMax, CallOfForestSkillCooldown, CallOfForestSkillReady, false, CallOfForestSkillPrefab);
            return tuple;
        }

        #endregion
    }
}