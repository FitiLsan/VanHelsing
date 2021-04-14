using UnityEngine;


namespace BeastHunter
{
    public sealed class Smell
    {
        #region Properties

        public Vector3 SmellPointPosition { get; }
        public LureSmellTypeEnum Type { get; }
        public float SmellingDistance { get; }
        public GameObject SmellObject { get; }

        #endregion


        #region ClassLifeCycle

        public Smell(Transform smellPointTransform, LureSmellTypeEnum type, float smellingDistance)
        {
            SmellObject = smellPointTransform.gameObject;
            SmellPointPosition = smellPointTransform.position;
            Type = type;
            SmellingDistance = smellingDistance;
            
        }

        #endregion
    }
}

