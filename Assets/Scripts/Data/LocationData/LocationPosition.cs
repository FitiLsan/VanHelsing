using UnityEngine;


namespace BeastHunter
{
    [System.Serializable]
    public sealed class LocationPosition
    {
        #region Fields

        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _eulers;
        [SerializeField] private Vector3 _scale;

        #endregion


        #region Properties

        public Vector3 Position => _position;
        public Vector3 Eulers => _eulers;
        public Vector3 Scale => _scale;

        #endregion


        #region ClassLifeCycle

        public LocationPosition()
        {
            _position = Vector3.zero;
            _eulers = Vector3.zero;
            _scale = Vector3.one;
        }

        public LocationPosition(Transform transform)
        {
            _position = transform.position;
            _eulers = transform.eulerAngles;
            _scale = transform.localScale;
        }

        public LocationPosition(Vector3 position, Vector3 eulers, Vector3 scale)
        {
            _position = position;
            _eulers = eulers;
            _scale = scale;
        }

        #endregion
    }
}

