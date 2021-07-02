using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "AnimaliaBoss", menuName = "CreateData/HubUIData/Boss/AnimaliaBoss", order = 0)]
    public class AnimaliaBossSO : BossDataSO
    {
        [Header("AnimaliaBossData")]
        [SerializeField] private AnimaliaBossType _bossSubtype;


        public override BossType BossType => BossType.Animalia;
        public override Enum BossSubtype => _bossSubtype;
    }
}
