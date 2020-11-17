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
        public float JumpHeight;
        public float JumpSpeed;

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
