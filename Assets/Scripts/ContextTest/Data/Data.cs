using System.IO;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        public static SphereData _sphereData;
        public static ButterflyData _butterflyData;

        #endregion


        #region Properties

        public static SphereData SphereData
        {
            get
            {
                if (_sphereData == null)
                {
                    _sphereData = Resources.Load<SphereData>("Data/SphereData");
                }
                return _sphereData;
            }
        }

        public static ButterflyData ButterflyData
        {
            get
            {
                if (_butterflyData == null)
                {
                    _butterflyData = Resources.Load<ButterflyData>("Data/ButterflyData");
                }
                return _butterflyData;
            }
        }

        #endregion
    }
}