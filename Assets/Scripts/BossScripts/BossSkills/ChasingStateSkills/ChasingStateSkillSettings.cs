using System;
using UnityEngine;


namespace BeastHunter
{

    [Serializable]
    public struct ChasingStateSkillsSettings
    {
        #region Fields

        [Header("[Vine Fishing Attack]")]

        [Tooltip("Skill ID")]
        [SerializeField] private int _vineFishingSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _vineFishingSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _vineFishingSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _vineFishingSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _vineFishingSkillReady;
      
        #endregion


        #region Properties

        public int VineFishingSkillId => _vineFishingSkillId;
        public float VineFishingSkillRangeMin => _vineFishingSkillRangeMin;
        public float VineFishingSkillRangeMax => _vineFishingSkillRangeMax;
        public float VineFishingSkillCooldown => _vineFishingSkillCooldown;
        public bool VineFishingSkillReady => _vineFishingSkillReady;    

        #endregion


        #region Methods

        public (int, float, float, float, bool, bool) GetVineFishingSkillInfo()
        {
            var tuple = (VineFishingSkillId, VineFishingSkillRangeMin, VineFishingSkillRangeMax, VineFishingSkillCooldown, VineFishingSkillReady, false);
            return tuple;
        }
        
        #endregion
    }
}