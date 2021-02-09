using System;
using UnityEngine;


namespace BeastHunter
{

    [Serializable]
    public struct AttackStateSkillsSettings
    {
        #region Fields

        [Header("[Default Attack]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _defaultSkillEnable;
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

        [Tooltip("Enable")]
        [SerializeField] private bool _horizontalSkillEnable;
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

        [Tooltip("Enable")]
        [SerializeField] private bool _stompSplashSkillEnable;
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

        [Header("[Poison Spores Line]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _poisonSporesSkillEnable;
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

        [Header("[Poison Spores Circle]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _poisonSporesCircleSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _poisonSporesCircleSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _poisonSporesCircleSkillRangeMin;
        [Range(-1.0f, 30.0f)]
        [Tooltip("Skill Range Max")]
        [SerializeField] private float _poisonSporesCircleSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _poisonSporesCircleSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _poisonSporesCircleSkillReady;

        [Header("[Catch Attack]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _catchSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _catchSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _catchSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _catchSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _catchSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _catchSkillReady;

        [Header("[Finger Attack]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _fingerSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _fingerSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _fingerSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _fingerSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _fingerSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _fingerSkillReady;

        #endregion


        #region Properties

        public bool DefaultSkillEnable => _defaultSkillEnable;
        public int DefaultSkillId => _defaultSkillId;
        public float DefaultSkillRangeMin => _defaultSkillRangeMin;
        public float DefaultSkillRangeMax => _defaultSkillRangeMax;
        public float DefaultSkillCooldown => _defaultSkillCooldown;
        public bool DefaultSkillReady => _defaultSkillReady;

        public bool HorizontalSkillEnable => _horizontalSkillEnable;
        public int HorizontalSkillId => _horizontalSkillId;
        public float HorizontalSkillRangeMin => _horizontalSkillRangeMin;
        public float HorizontalSkillRangeMax => _horizontalSkillRangeMax;
        public float HorizontalSkillCooldown => _horizontalSkillCooldown;
        public bool HorizontalSkillReady => _horizontalSkillReady;

        public bool StompSplashSkillEnable => _stompSplashSkillEnable;
        public int StompSplashSkillId => _stompSplashSkillId;
        public float StompSplashSkillRangeMin => _stompSplashSkillRangeMin;
        public float StompSplashSkillRangeMax => _stompSplashSkillRangeMax;
        public float StompSplashSkillCooldown => _stompSplashSkillCooldown;
        public bool StompSplashSkillReady => _stompSplashSkillReady;

        public bool PoisonSporesSkillEnable => _poisonSporesSkillEnable;
        public int PoisonSporesSkillId => _poisonSporesSkillId;
        public float PoisonSporesSkillRangeMin => _poisonSporesSkillRangeMin;
        public float PoisonSporesSkillRangeMax => _poisonSporesSkillRangeMax;
        public float PoisonSporesSkillCooldown => _poisonSporesSkillCooldown;
        public bool PoisonSporesSkillReady => _poisonSporesSkillReady;

        public bool PoisonSporesCircleSkillEnable => _poisonSporesCircleSkillEnable;
        public int PoisonSporesCircleSkillId => _poisonSporesCircleSkillId;
        public float PoisonSporesCircleSkillRangeMin => _poisonSporesCircleSkillRangeMin;
        public float PoisonSporesCircleSkillRangeMax => _poisonSporesCircleSkillRangeMax;
        public float PoisonSporesCircleSkillCooldown => _poisonSporesCircleSkillCooldown;
        public bool PoisonSporesCircleSkillReady => _poisonSporesCircleSkillReady;


        public bool CatchSkillSkillEnable => _catchSkillEnable;
        public int CatchSkillId => _catchSkillId;
        public float CatchSkillRangeMin => _catchSkillRangeMin;
        public float CatchSkillRangeMax => _catchSkillRangeMax;
        public float CatchSkillCooldown => _catchSkillCooldown;
        public bool CatchSkillReady => _catchSkillReady;

        public bool FignerSkillEnable => _fingerSkillEnable;
        public int FingerSkillId => _fingerSkillId;
        public float FingerSkillRangeMin => _fingerSkillRangeMin;
        public float FingerSkillRangeMax => _fingerSkillRangeMax;
        public float FingerSkillCooldown => _fingerSkillCooldown;
        public bool FingerSkillReady => _fingerSkillReady;

        #endregion


        #region Methods

        public (bool, int, float, float, float, bool, bool) GetDefaultSkillInfo()
        {
            var tuple = (DefaultSkillEnable, DefaultSkillId, DefaultSkillRangeMin, DefaultSkillRangeMax, DefaultSkillCooldown, DefaultSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetHorizontalSkillInfo()
        {
            var tuple = (HorizontalSkillEnable, HorizontalSkillId, HorizontalSkillRangeMin, HorizontalSkillRangeMax, HorizontalSkillCooldown, HorizontalSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetStompSplashSkillInfo()
        {
            var tuple = (StompSplashSkillEnable, StompSplashSkillId, StompSplashSkillRangeMin, StompSplashSkillRangeMax, StompSplashSkillCooldown, StompSplashSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetPoisonSporesSkillInfo()
        {
            var tuple = (PoisonSporesSkillEnable, PoisonSporesSkillId, PoisonSporesSkillRangeMin, PoisonSporesSkillRangeMax, PoisonSporesSkillCooldown, PoisonSporesSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetPoisonSporesCircleSkillInfo()
        {
            var tuple = (PoisonSporesCircleSkillEnable, PoisonSporesCircleSkillId, PoisonSporesCircleSkillRangeMin, PoisonSporesCircleSkillRangeMax, PoisonSporesCircleSkillCooldown, PoisonSporesCircleSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetCatchSkillInfo()
        {
            var tuple = (CatchSkillSkillEnable, CatchSkillId, CatchSkillRangeMin, CatchSkillRangeMax, CatchSkillCooldown, CatchSkillReady, false);
            return tuple;
        }

        public (bool, int, float, float, float, bool, bool) GetFingerSkillInfo()
        {
            var tuple = (FignerSkillEnable, FingerSkillId, FingerSkillRangeMin, FingerSkillRangeMax, FingerSkillCooldown, FingerSkillReady, false);
            return tuple;
        }
        #endregion
    }
}