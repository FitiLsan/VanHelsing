using System;
using UnityEngine;


namespace BeastHunter
{

    [Serializable]
    public struct DeadStateSkillsSettings
    {
        #region Fields

        [Header("[Bush Trigger Attack]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _resurrectionSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _resurrectionSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _resurrectionSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _resurrectionSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _resurrectionSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _resurrectionSkillReady;

        #endregion


        #region Properties

        public bool ResurrectionSkillEnable => _resurrectionSkillEnable;
        public int ResurrectionSkillId => _resurrectionSkillId;
        public float ResurrectionSkillRangeMin => _resurrectionSkillRangeMin;
        public float ResurrectionSkillRangeMax => _resurrectionSkillRangeMax;
        public float ResurrectionSkillCooldown => _resurrectionSkillCooldown;
        public bool ResurrectionSkillReady => _resurrectionSkillReady;    

        #endregion


        #region Methods

        public (bool, int, float, float, float, bool, bool) GetResurrectionSkillInfo()
        {
            var tuple = (ResurrectionSkillEnable, ResurrectionSkillId, ResurrectionSkillRangeMin, ResurrectionSkillRangeMax, ResurrectionSkillCooldown, ResurrectionSkillReady, false);
            return tuple;
        }
        
        #endregion
    }
}