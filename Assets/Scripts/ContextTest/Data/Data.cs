using System.IO;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        public static SphereData _sphereData;
        public static WolfData _wolfData;

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

        public static WolfData WolfData
        {
            get
            {
                if(_wolfData==null)
                {
                    _wolfData = Resources.Load<WolfData>("Data/WolfData");
                }
                return _wolfData;
            }
        }

        #endregion
    }
}