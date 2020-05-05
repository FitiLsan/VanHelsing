using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        public static SphereData _sphereData;
        public static CharacterData _characterData;
        public static GiantMudCrabData _giantMudCrabData;

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

        public static CharacterData CharacterData
        {
            get
            {
                if (_characterData == null)
                {
                    _characterData = Resources.Load<CharacterData>("Data/CharacterData");
                }
                return _characterData;
            }
        }

        public static GiantMudCrabData GiantMudCrabData
        {
            get
            {
                if (_giantMudCrabData == null)
                {
                    _giantMudCrabData = Resources.Load<GiantMudCrabData>("Data/GiantMudCrabData");
                }
                return _giantMudCrabData;
            }
        }

        #endregion
    }
}