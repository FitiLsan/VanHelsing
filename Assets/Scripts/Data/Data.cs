using System.IO;
using UnityEngine;


namespace BeastHunter 
{
    [CreateAssetMenu (fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject 
    {
        #region Fields

        [SerializeField] private string _sphereDataPath;
        [SerializeField] private string _characterDataPath;
        [SerializeField] private string _startDialogueDataPath;
        [SerializeField] private string _dialogueSystemDataPath;
        [SerializeField] private string _giantMudCrabDataPath;
        [SerializeField] private string _rabbitDataPath;
        [SerializeField] private string _feastPath;
        [SerializeField] private string _bossFeastPath;
        [SerializeField] private string _jacketPath;
        [SerializeField] private string _helmPath;
        [SerializeField] private string _cameraDataPath;
        [SerializeField] private string _questIndicatorDataPath;
        [SerializeField] private string _questJournalDataPath;
        [SerializeField] private string _healthBuffDataPath;
        [SerializeField] private string _staminaBuffDataPath;
        [SerializeField] private string _bossDataPath;
        [SerializeField] private string _trapDataPath;
        [SerializeField] private string _trapDataPath2;
        [SerializeField] private string _bossFirstWeakPointPath;
        [SerializeField] private string _bossSecondWeakPointPath;
        [SerializeField] private string _bossThirdWeakPointPath;
        [SerializeField] private string _timeSkipDataPath;
        [SerializeField] private string _weaponWheelUIPath;
        [SerializeField] private string _shouldersPath;
        [SerializeField] private string _shoesPath;
        [SerializeField] private string _ironGreavesPath;

        private static Data _instance;
        private static SphereData _sphereData;
        private static CharacterData _characterData;
        private static StartDialogueData _startDialogueData;
        private static DialogueSystemData _dialogueSystemData;
        private static GiantMudCrabData _giantMudCrabData;
        private static RabbitData _rabbitData;
        private static QuestIndicatorData _questIndicatorData;
        private static WeaponItem _feast;
        private static WeaponItem _bossFeast;
        private static ClothItem _jacket;
        private static ClothItem _helm;
        private static ClothItem _shoulders;
        private static ClothItem _shoes;
        private static ClothItem _ironGreaves;
        private static CameraData _cameraData;
        private static QuestJournalData _questJournalData;
        private static TemporaryBuffClass _healthBuffData;
        private static TemporaryBuffClass _staminaBuffData;
        private static BossData _bossData;
        private static TrapData _trapData;
        private static TrapData _trapData2;
        private static WeakPointData _bossFirstWeakPoint;
        private static WeakPointData _bossSecondWeakPoint;
        private static WeakPointData _bossThirdWeakPoint;
        private static TimeSkipData _timeSkipData;
        private static GameObject _weaponWheelUI;

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

        public static SphereData SphereData {
            get {
                if (_sphereData == null) {
                    _sphereData = Resources.Load<SphereData> ("Data/" + Instance._sphereDataPath);
                }
                return _sphereData;
            }
        }

        public static StartDialogueData StartDialogueData {
            get {
                if (_startDialogueData == null) {
                    _startDialogueData = Resources.Load<StartDialogueData> ("Data/" + Instance._startDialogueDataPath);
                }
                return _startDialogueData;
            }
        }

        public static DialogueSystemData DialogueSystemData {
            get {
                if (_dialogueSystemData == null) {
                    _dialogueSystemData = Resources.Load<DialogueSystemData> ("Data/" + Instance._dialogueSystemDataPath);
                }
                return _dialogueSystemData;
            }
        }

        public static CharacterData CharacterData {
            get {
                if (_characterData == null) {
                    _characterData = Resources.Load<CharacterData> ("Data/" + Instance._characterDataPath);
                }
                return _characterData;
            }
        }

        public static GiantMudCrabData GiantMudCrabData {
            get {
                if (_giantMudCrabData == null) {
                    _giantMudCrabData = Resources.Load<GiantMudCrabData> ("Data/" + Instance._giantMudCrabDataPath);
                }
                return _giantMudCrabData;
            }
        }

        public static RabbitData RabbitData
        {
            get
            {
                if (_rabbitData == null)
                {
                    _rabbitData = Resources.Load<RabbitData>("Data/" + Instance._rabbitDataPath);
                }
                return _rabbitData;
			}
        }

        public static QuestIndicatorData QuestIndicatorData
        {
            get
            {
                if (_questIndicatorData == null)
                {
                    _questIndicatorData = Resources.Load<QuestIndicatorData>("Data/" + Instance._questIndicatorDataPath);
                }
                return _questIndicatorData;
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

        public static WeaponItem BossFeast
        {
            get
            {
                if (_bossFeast == null)
                {
                    _bossFeast = Resources.Load<WeaponItem>("Data/" + Instance._bossFeastPath);
                }
                return _bossFeast;
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

        public static ClothItem Shoulders
        {
            get
            {
                if (_shoulders == null)
                {
                    _shoulders = Resources.Load<ClothItem>("Data/" + Instance._shouldersPath);
                }
                return _shoulders;
            }
        }

        public static ClothItem Helm
        {
            get
            {
                if (_helm == null)
                {
                    _helm = Resources.Load<ClothItem>("Data/" + Instance._helmPath);
                }
                return _helm;
            }
        }

        public static ClothItem Shoes
        {
            get
            {
                if (_shoes == null)
                {
                    _shoes = Resources.Load<ClothItem>("Data/" + Instance._shoesPath);
                }
                return _shoes;
            }
        }

        public static ClothItem IronGreaves
        {
            get
            {
                if (_ironGreaves == null)
                {
                    _ironGreaves = Resources.Load<ClothItem>("Data/" + Instance._ironGreavesPath);
                }
                return _ironGreaves;
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

        public static QuestJournalData QuestJournalData
        {
            get
            {
                if (_questJournalData == null)
                {
                    _questJournalData = Load<QuestJournalData>("Data/" + Instance._questJournalDataPath);
                }
                return _questJournalData;
            }
        }

        public static TemporaryBuffClass HealthBuffData
        {
            get
            {
                if (_healthBuffData == null)
                {
                    _healthBuffData = Load<TemporaryBuffClass>("Data/" + Instance._healthBuffDataPath);
                }
                return _healthBuffData;
            }
        }

        public static TemporaryBuffClass StaminaBuffData
        {
            get
            {
                if (_staminaBuffData == null)
                {
                    _staminaBuffData = Load<TemporaryBuffClass>("Data/" + Instance._staminaBuffDataPath);
                }
                return _staminaBuffData;
            }
        }

        public static BossData BossData
        {
            get
            {
                if (_bossData == null)
                {
                    _bossData = Load<BossData>("Data/" + Instance._bossDataPath);
                }
                return _bossData;
            }
        }

        public static TrapData TrapData
        {
            get
            {
                if (_trapData == null)
                {
                    _trapData = Load<TrapData>("Data/" + Instance._trapDataPath);
                }
                return _trapData;
            }
        }

        public static TrapData TrapData2
        {
            get
            {
                if (_trapData2 == null)
                {
                    _trapData2 = Load<TrapData>("Data/" + Instance._trapDataPath2);
                }
                return _trapData2;
            }
        }

        public static WeakPointData BossFirstWeakPoint
        {
            get
            {
                if (_bossFirstWeakPoint == null)
                {
                    _bossFirstWeakPoint = Load<WeakPointData>("Data/" + Instance._bossFirstWeakPointPath);
                }
                return _bossFirstWeakPoint;
            }
        }

        public static WeakPointData BossSecondWeakPoint
        {
            get
            {
                if (_bossSecondWeakPoint == null)
                {
                    _bossSecondWeakPoint = Load<WeakPointData>("Data/" + Instance._bossSecondWeakPointPath);
                }
                return _bossSecondWeakPoint;
            }
        }

        public static WeakPointData BossThirdWeakPoint
        {
            get
            {
                if (_bossThirdWeakPoint == null)
                {
                    _bossThirdWeakPoint = Load<WeakPointData>("Data/" + Instance._bossThirdWeakPointPath);
                }
                return _bossThirdWeakPoint;
            }
        }

        public static TimeSkipData TimeSkipData
        {
            get
            {
                if (_timeSkipData == null)
                {
                    _timeSkipData = Load<TimeSkipData>("Data/" + Instance._timeSkipDataPath);
                }
                return _timeSkipData;
            }
        }

        public static GameObject WeaponWheelUI
        {
            get
            {
                if(_weaponWheelUI == null)
                {
                    _weaponWheelUI = Load<GameObject>("Data/" + Instance._weaponWheelUIPath);
                }
                return _weaponWheelUI;
            }
        }

        #endregion


        #region Methods

        private static T Load<T> (string resourcesPath) where T : Object =>
            Resources.Load<T> (Path.ChangeExtension (resourcesPath, null));

        #endregion
    }
}