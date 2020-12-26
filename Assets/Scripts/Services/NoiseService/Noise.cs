using UnityEngine;


namespace BeastHunter
{
    public sealed class Noise
    {
        #region Properties

        public Vector3 NoisePointPosition { get; }
        public NoiseType Type { get; }
        public float HearingDistance { get; }

        #endregion


        #region ClassLifeCycle

        public Noise(Vector3 noisePointPosition, NoiseType type, float hearingDistance)
        {
            NoisePointPosition = noisePointPosition;
            Type = type;
            HearingDistance = hearingDistance;
        }

        #endregion
    }
}

