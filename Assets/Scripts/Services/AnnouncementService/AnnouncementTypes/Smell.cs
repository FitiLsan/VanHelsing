using UnityEngine;


namespace BeastHunter
{
    public sealed class Smell
    {
        #region Properties

        public Vector3 SmellPointPosition { get; }
        public SmellType Type { get; }
        public float HearingDistance { get; }

        #endregion


        #region ClassLifeCycle

        public Smell(Vector3 smellPointPosition, SmellType type, float hearingDistance)
        {
            SmellPointPosition = smellPointPosition;
            Type = type;
            HearingDistance = hearingDistance;
        }

        #endregion
    }
}

