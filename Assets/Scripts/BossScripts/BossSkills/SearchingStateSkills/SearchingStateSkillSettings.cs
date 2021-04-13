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
        [Range(-1.0f, 100.0f)]
        [SerializeField] private float _bushTriggerSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 100.0f)]
        [SerializeField] private float _bushTriggerSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private int _bushTriggerSkillCount;
        [Tooltip("Skill Bush MaxCount")]
        [Range(1, 50)]
        [SerializeField] private float _bushTriggerSkillCooldown;
        [Tooltip("Skill Prefab")]
        [SerializeField] private GameObject _bushTriggerSkillPrefab;
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
        public GameObject BushTriggerSkillPrefab => _bushTriggerSkillPrefab;
        public int BushTriggerSkillCount => _bushTriggerSkillCount;

        #endregion


        #region Methods

        public (bool, int, float, float, float, bool, bool, GameObject, int) GetBushTriggerSkillInfo()
        {
            var tuple = (BushTriggerSkillEnable, BushTriggerSkillId, BushTriggerSkillRangeMin, BushTriggerSkillRangeMax, BushTriggerSkillCooldown, BushTriggerSkillReady, false, BushTriggerSkillPrefab, BushTriggerSkillCount);
            return tuple;
        }
        
        #endregion
    }
}