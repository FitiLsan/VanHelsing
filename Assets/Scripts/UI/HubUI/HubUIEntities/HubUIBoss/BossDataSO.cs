using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    public abstract class BossDataSO : ScriptableObject
    {
        [Header("BaseBossData")]
        [SerializeField] private string _name;
        [SerializeField] private BossSizeType _bossSize;
        [SerializeField] private AttractionOrAvoidanceType _attraction;
        [SerializeField] private AttractionOrAvoidanceType _avoidance;
        [SerializeField] private VulnerabilityType _vulnerability;


        public abstract BossType BossType { get; }
        public abstract Enum BossSubtype { get; }
        public string Name => _name;
        public BossSizeType BossSize => _bossSize;
        public AttractionOrAvoidanceType Attraction => _attraction;
        public AttractionOrAvoidanceType Avoidance => _avoidance;
        public VulnerabilityType Vulnerability => _vulnerability;
    }
}
