using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        private static SphereData _sphereData;
        private static WolfData _wolfData;
        private static CharacterData _characterData;
        private static RabbitData _rabbitData;


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
                if (_wolfData == null)
                {
                    _wolfData = Resources.Load<WolfData>("Data/WolfData");
                }
                return _wolfData;
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

        public static RabbitData RabbitData
        {
            get
            {
                if (_rabbitData == null)
                {
                    _rabbitData = Resources.Load<RabbitData>("Data/RabbitData");
                }
                return _rabbitData;
            }
        }

        #endregion
    }
}