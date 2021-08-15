using System.IO;
using UnityEngine;


namespace BeastHunter {
    [CreateAssetMenu (fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject {
        #region Fields

        [SerializeField] private string _locationDataPath;
        [SerializeField] private string _sphereDataPath;
        [SerializeField] private string _characterDataPath;
        [SerializeField] private string _rabbitDataPath;
        [SerializeField] private string _feastPath;
        [SerializeField] private string _jacketPath;
        [SerializeField] private string _cameraDataPath;
        [SerializeField] private string _butterflyDataPath;

        private static Data _instance;
        private static LocationData _locationData;
        private static SphereData _sphereData;
        private static CharacterData _characterData;
        private static RabbitData _rabbitData;
        private static WeaponItem _feast;
        private static ClothItem _jacket;
        private static CameraData _cameraData;
        private static ButterflyData _butterflyData;

        #endregion


        #region Properties

        public static Data Instance {
            get {
                if (_instance == null) {
                    _instance = Resources.Load<Data> ("Data/" + typeof (Data).Name);
                }
                return _instance;
            }
        }

        public static LocationData LocationData
        {
            get
            {
                if (_locationData == null)
                {
                    _locationData = Resources.Load<LocationData>("Data/" + Instance._locationDataPath);
                }
                return _locationData;
            }
        }

        public static SphereData SphereData {
            get {
                if (_sphereData == null) {
                    _sphereData = Load<SphereData> ("Data/" + Instance._sphereDataPath);
                }
                return _sphereData;
            }
        }

        public static CharacterData CharacterData {
            get {
                if (_characterData == null) {
                    _characterData = Load<CharacterData> ("Data/" + Instance._characterDataPath);
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
                    _rabbitData = Load<RabbitData>("Data/" + Instance._rabbitDataPath);
                }
                return _rabbitData;
			}
        }

        public static WeaponItem Feast {
            get {
                if (_feast == null) {
                    _feast = Resources.Load<WeaponItem> ("Data/" + Instance._feastPath); 
                }
                return _feast;
            }
        }

        public static ClothItem Jacket {
            get {
                if (_jacket == null) {
                    _jacket = Resources.Load<ClothItem> ("Data/" + Instance._jacketPath);
                }
                return _jacket;
            }
        }

        public static CameraData CameraData
        {
            get
            {
                if (_cameraData == null)
                {
                    _cameraData = Resources.Load<CameraData>("Data/" + Instance._cameraDataPath);
                }
                return _cameraData;
            }
        }

        public static ButterflyData ButterflyData
        {
            get
            {
                if(_butterflyData == null)
                {
                    _butterflyData = Load<ButterflyData>("Data/" + Instance._butterflyDataPath);
                }
                return _butterflyData;
            }
        }

        #endregion


        #region Methods

        private static T Load<T> (string resourcesPath) where T : Object =>
            Resources.Load<T> (Path.ChangeExtension (resourcesPath, null));

        #endregion
    }
}