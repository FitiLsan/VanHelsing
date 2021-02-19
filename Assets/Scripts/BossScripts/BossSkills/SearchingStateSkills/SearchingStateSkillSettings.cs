using System;
using UnityEngine;


namespace BeastHunter
{

    [Serializable]
    public struct SearchingStateSkillsSettings
    {
        #region Fields

        [Header("[Bush Trigger Attack]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _bushTriggerSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _bushTriggerSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _bushTriggerSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _bushTriggerSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _bushTriggerSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _bushTriggerSkillReady;

        #endregion


        #region Properties

        public bool BushTriggerSkillEnable => _bushTriggerSkillEnable;
        public int BushTriggerSkillId => _bushTriggerSkillId;
        public float BushTriggerSkillRangeMin => _bushTriggerSkillRangeMin;
        public float BushTriggerSkillRangeMax => _bushTriggerSkillRangeMax;
        public float BushTriggerSkillCooldown => _bushTriggerSkillCooldown;
        public bool BushTriggerSkillReady => _bushTriggerSkillReady;    

        #endregion


        #region Methods

        public (bool, int, float, float, float, bool, bool) GetBushTriggerSkillInfo()
        {
            var tuple = (BushTriggerSkillEnable, BushTriggerSkillId, BushTriggerSkillRangeMin, BushTriggerSkillRangeMax, BushTriggerSkillCooldown, BushTriggerSkillReady, false);
            return tuple;
        }
        
        #endregion
    }
}