using System;
using UnityEngine;


namespace BeastHunter
{

    [Serializable]
    public struct RetreatingStateSkillsSettings
    {
        #region Fields

        [Header("[Fake Tree Attack]")]

        [Tooltip("Enable")]
        [SerializeField] private bool _fakeTreeSkillEnable;
        [Tooltip("Skill ID")]
        [SerializeField] private int _fakeTreeSkillId;
        [Tooltip("Skill Range Min")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _fakeTreeSkillRangeMin;
        [Tooltip("Skill Range Max")]
        [Range(-1.0f, 30.0f)]
        [SerializeField] private float _fakeTreeSkillRangeMax;
        [Tooltip("Skill Cooldown")]
        [Range(0.0f, 200.0f)]
        [SerializeField] private float _fakeTreeSkillCooldown;
        [Tooltip("Skill Ready")]
        [SerializeField] private bool _fakeTreeSkillReady;
        [Tooltip("Fake Tree Prefab")]
        [SerializeField] private GameObject _fakeTreePrefab;

        #endregion


        #region Properties

        public bool FakeTreeSkillEnable => _fakeTreeSkillEnable;
        public int FakeTreeSkillId => _fakeTreeSkillId;
        public float FakeTreeSkillRangeMin => _fakeTreeSkillRangeMin;
        public float FakeTreeSkillRangeMax => _fakeTreeSkillRangeMax;
        public float FakeTreeSkillCooldown => _fakeTreeSkillCooldown;
        public bool FakeTreeSkillReady => _fakeTreeSkillReady;
        public GameObject FakeTreePrefab => _fakeTreePrefab;

        #endregion


        #region Methods

        public (bool, int, float, float, float, bool, bool) GetFakeTreeSkillInfo()
        {
            var tuple = (FakeTreeSkillEnable, FakeTreeSkillId, FakeTreeSkillRangeMin, FakeTreeSkillRangeMax, FakeTreeSkillCooldown, FakeTreeSkillReady, false);
            return tuple;
        }
        
        #endregion
    }
}