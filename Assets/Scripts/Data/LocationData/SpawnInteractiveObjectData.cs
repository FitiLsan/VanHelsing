using UnityEngine;


namespace BeastHunter
{
    [System.Serializable]
    public sealed class SpawnInteractiveObjectData
    {
        #region Fields

        [SerializeField] private LocationPosition _objectPosition;
        [SerializeField] private InteractiveObjectType _type;

        #endregion


        #region Properties

        public LocationPosition ObjectPosition => _objectPosition;
        public InteractiveObjectType Type => _type;

        #endregion


        #region ClassLifeCycle

        public SpawnInteractiveObjectData(LocationPosition position, InteractiveObjectType type)
        {
            _objectPosition = position;
            _type = type;
        }

        #endregion
    }
}

