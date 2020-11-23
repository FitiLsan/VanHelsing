using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct HellHoundStats
    {
        #region Properties

        public float WanderingRadius;
        public float DetectionRadius;
        public float JumpingSpeedRate;
        public float RoamingChance;
        public float RestingChance;
        public float IdlingMinTime;
        public float IdlingMaxTime;
        public float RestingMinTime;
        public float RestingMaxTime;

        [Header("Damage")]
        public float PhysicalDamage;
        public float StunProbability;

        [Header("Escape")]
        [Tooltip("Must be greater than a detection radius")]
        public float EscapeDistance;
        public float PercentEscapeHealth;

        [Header("Attacks")]
        public float AttacksTurnSpeed;
        public float AttacksMaxDistance;
        public float AttackJumpMaxDistance;
        public float AttackJumpMinDistance;

        [Header("BackJump")]
        public float BackJumpDistance;
        public float BackJumpLength;
        public float BackJumpSpeed;
        public float BackJumpAnimationSpeedRate;
        [Range(0,1)]
        public float BackJumpAnimationIntensity;

        [Header("BattleCircling")]
        public float BattleCirclingRadius;
        public float BattleCirclingSpeed;
        public float BattleCirclingMinTime;
        public float BattleCirclingMaxTime;

        [Header("NavMeshAgent")]
        public float MaxRoamingSpeed;
        public float MaxChasingSpeed;
        public float AngularSpeed;
        public float Acceleration;
        public float StoppingDistance;
        public float BaseOffsetByY;

        #endregion
    }
}
