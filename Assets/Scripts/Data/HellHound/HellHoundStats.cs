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

        [Header("Damage")]
        public float PhysicalDamage;
        public float StunProbability;

        [Header("Attacks")]
        public float AttackTorsoMaxDistance;
        public float AttackJumpMaxDistance;
        public float AttackJumpMinDistance;

        [Header("BackJump")]
        public float BackJumpDistance;
        public float BackJumpLength;
        public float BackJumpSpeed;
        public float BackJumpAnimationSpeedRate;
        [Range(0,1)]
        public float BackJumpAnimationIntensity;

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
