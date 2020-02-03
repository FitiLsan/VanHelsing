using UnityEngine;


namespace Models
{
    [CreateAssetMenu(menuName = "NewNPCPoint", fileName = "NewNPCPoint")]
    public class NPCPoint : ScriptableObject
    {
        #region PrivateData

        [Tooltip("ID")]
        [SerializeField] private int _npcPointID;

        [Tooltip("Point Name")]
        [SerializeField] private string _pointName;

        [Tooltip("Position of the point")]
        [SerializeField] private Vector3 _pointPosition;

        [Tooltip("Time on the point")]
        [SerializeField] private float _timeOnPoint;

        [Tooltip("Distance to activate point")]
        [SerializeField] private float _distanceToActivate;

        [Tooltip("Animation on the point")]
        [SerializeField] private Animation _animationOnPoint;

        #endregion


        #region Properties

        public int ID => _npcPointID;
        public string PointName => _pointName;
        public Vector3 PointPosition => _pointPosition;
        public float TimeOnPoint => _timeOnPoint;
        public float DistanceToActivate => _distanceToActivate;
        public Animation AnimationOnPoint => _animationOnPoint;

        #endregion
    }
}
