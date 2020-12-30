using System;
using UnityEngine;


namespace BeastHunter
{

    [Serializable]
    public struct NonStateSkillsSettings
    {
        #region Fields

        [Header("[Test Attack]")]

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
      
        #endregion


        #region Properties

        public int TestSkillId => _testSkillId;
        public float TestSkillRangeMin => _testSkillRangeMin;
        public float TestSkillRangeMax => _testSkillRangeMax;
        public float TestSkillCooldown => _testSkillCooldown;
        public bool TestSkillReady => _testSkillReady;    

        #endregion


        #region Methods

        public (int, float, float, float, bool, bool) GetTestSkillInfo()
        {
            var tuple = (TestSkillId, TestSkillRangeMin, TestSkillRangeMax, TestSkillCooldown, TestSkillReady, false);
            return tuple;
        }
        
        #endregion
    }
}