using System;
using UnityEngine;


namespace BeastHunter
{

    [Serializable]
    public struct AttackStateSkillsSettings
    {
        #region Fields

        [Header("[Default Attack]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _defaultSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _defaultSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _defaultSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _defaultSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _defaultSkillReady;

        [Header("[Horizontal Attack]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _horizontalSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _horizontalSkillRangeMin;
        [Range(-1.0f, 30.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _horizontalSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _horizontalSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _horizontalSkillReady;

        [Header("[Stomp Splash]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _stompSplashSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _stompSplashSkillRangeMin;
        [Range(-1.0f, 30.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _stompSplashSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _stompSplashSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _stompSplashSkillReady;

        [Header("[Rage Of Forest]")]

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

        [Header("[Poison Spores]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _poisonSporesSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _poisonSporesSkillRangeMin;
        [Range(-1.0f, 30.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _poisonSporesSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _poisonSporesSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _poisonSporesSkillReady;

        #endregion


        #region Properties

        public int DefaultSkillId => _defaultSkillId;
        public float DefaultSkillRangeMin => _defaultSkillRangeMin;
        public float DefaultSkillRangeMax => _defaultSkillRangeMax;
        public float DefaultSkillCooldown => _defaultSkillCooldown;
        public bool DefaultSkillReady => _defaultSkillReady;
        (int, float, float, float, bool) DefaultSkillTuple;

        public int HorizontalSkillId => _horizontalSkillId;
        public float HorizontalSkillRangeMin => _horizontalSkillRangeMin;
        public float HorizontalSkillRangeMax => _horizontalSkillRangeMax;
        public float HorizontalSkillCooldown => _horizontalSkillCooldown;
        public bool HorizontalSkillReady => _horizontalSkillReady;

        public int StompSplashSkillId => _stompSplashSkillId;
        public float StompSplashSkillRangeMin => _stompSplashSkillRangeMin;
        public float StompSplashSkillRangeMax => _stompSplashSkillRangeMax;
        public float StompSplashSkillCooldown => _stompSplashSkillCooldown;
        public bool StompSplashSkillReady => _stompSplashSkillReady;

        public int RageOfForestSkillId => _rageOfForestSkillId;
        public float RageOfForestSkillRangeMin => _rageOfForestSkillRangeMin;
        public float RageOfForestSkillRangeMax => _rageOfForestSkillRangeMax;
        public float RageOfForestSkillCooldown => _rageOfForestSkillCooldown;
        public bool RageOfForestSkillReady => _rageOfForestSkillReady;

        public int PoisonSporesSkillId => _poisonSporesSkillId;
        public float PoisonSporesSkillRangeMin => _poisonSporesSkillRangeMin;
        public float PoisonSporesSkillRangeMax => _poisonSporesSkillRangeMax;
        public float PoisonSporesSkillCooldown => _poisonSporesSkillCooldown;
        public bool PoisonSporesSkillReady => _poisonSporesSkillReady;

        #endregion


        #region Methods

        public (int, float, float, float, bool) GetDefaultSkillInfo()
        {
            var tuple = (DefaultSkillId, DefaultSkillRangeMin, DefaultSkillRangeMax, DefaultSkillCooldown, DefaultSkillReady);
            return tuple;
        }

        public (int, float, float, float, bool) GetHorizontalSkillInfo()
        {
            var tuple = ( HorizontalSkillId,  HorizontalSkillRangeMin,  HorizontalSkillRangeMax,  HorizontalSkillCooldown,  HorizontalSkillReady);
            return tuple;
        }

        public (int, float, float, float, bool) GetStompSplashSkillInfo()
        {
            var tuple = (StompSplashSkillId, StompSplashSkillRangeMin, StompSplashSkillRangeMax, StompSplashSkillCooldown, StompSplashSkillReady);
            return tuple;
        }

        public (int, float, float, float, bool) GetRageOfForestSkillInfo()
        {
            var tuple = (RageOfForestSkillId, RageOfForestSkillRangeMin, RageOfForestSkillRangeMax, RageOfForestSkillCooldown, RageOfForestSkillReady);
            return tuple;
        }

        public (int, float, float, float, bool) GetPoisonSporesSkillInfo()
        {
            var tuple = (PoisonSporesSkillId, PoisonSporesSkillRangeMin, PoisonSporesSkillRangeMax, PoisonSporesSkillCooldown, PoisonSporesSkillReady);
            return tuple;
        }

        #endregion
    }
}