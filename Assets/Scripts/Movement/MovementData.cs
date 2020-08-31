using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "MovementData")]
    public class MovementData : ScriptableObject
    {
        #region Fields

        private List<MovementPath.Point> _points;
        [SerializeField] private MovementPath _path;

        #endregion


        #region Properties

        public List<MovementPath.Point> Points => _points ?? (_points = _path.GetPoints());

        public bool Loop => _path.Close;

        #endregion
    }
}