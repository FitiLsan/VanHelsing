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
        public float RoamingSpeed;
        public float RunSpeed;
        public float AngularSpeed;
        public float Acceleration;
        public float StoppingDistance;

        #endregion
    }
}
