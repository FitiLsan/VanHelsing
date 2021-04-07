using UnityEngine;


namespace BeastHunter
{
    public sealed class Light
    {
        #region Properties

        public Vector3 LightPointPosition { get; }
        public LightType Type { get; }
        public float SeeingDistance { get; }

        #endregion


        #region ClassLifeCycle

        public Light(Vector3 lightPointPosition, LightType type, float seeingDistance)
        {
            LightPointPosition = lightPointPosition;
            Type = type;
            SeeingDistance = seeingDistance;
        }

        #endregion
    }
}
