using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu (fileName = "Data", menuName = "MainData/Data")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _locationDataPath;
        [SerializeField] private string _characterDataPath;
        [SerializeField] private string _startDialogueDataPath;
        [SerializeField] private string _dialogueSystemDataPath;
        [SerializeField] private string _rabbitDataPath;
        [SerializeField] private string _feastPath;
        [SerializeField] private string _bossFeastsPath;
        [SerializeField] private string _jacketPath;
        [SerializeField] private string _helmPath;
        [SerializeField] private string _cameraDataPath;
        [SerializeField] private string _questIndicatorDataPath;
        [SerializeField] private string _questJournalDataPath;
        [SerializeField] private string _uiBestiaryDataPath;
        [SerializeField] private string _healthBuffDataPath;
        [SerializeField] private string _staminaBuffDataPath;
        [SerializeField] private string _bossDataPath;
        [SerializeField] private string _trapDataPath;
        [SerializeField] private string _trapDataPath2;
        [SerializeField] private string _bossFirstWeakPointPath;
        [SerializeField] private string _bossSecondWeakPointPath;
        [SerializeField] private string _bossThirdWeakPointPath;
        [SerializeField] private string _shouldersPath;
        [SerializeField] private string _shoesPath;
        [SerializeField] private string _ironGreavesPath;
        [SerializeField] private string _uiElementsDataPath;
        [SerializeField] private string _materialsDataPath;
        [SerializeField] private string _torchObjectPath;
        [SerializeField] private string _hellHoundDataPath;
        [SerializeField] private string _twoHeadedSnakeDataPath;
        [SerializeField] private string _bouldersObjectPath;
        [SerializeField] private string _hideBushDataPath;
        [SerializeField] private string _audioDataPath;
        [SerializeField] private string _playerHealthBarDataPath;
        [SerializeField] private string _fallingTreeDataPath;
        [SerializeField] private string _enemyHealthBarDataPath;
        [SerializeField] private string _spawnpointsDataPath;
        [SerializeField] private string _hubMapUIDataPath;
        [SerializeField] private string _lureMeatDataPath;
        [SerializeField] private string _lureCharcoalDataPath;

        private static Data _instance;
        private static LocationData _locationData;
        private static CharacterData _characterData;
        private static StartDialogueData _startDialogueData;
        private static DialogueSystemData _dialogueSystemData;
        private static RabbitData _rabbitData;
        private static QuestIndicatorData _questIndicatorData;
        private static WeaponItem _feast;
        private static WeaponData _bossFeast;
        private static ClothItem _jacket;
        private static ClothItem _helm;
        private static ClothItem _shoulders;
        private static ClothItem _shoes;
        private static ClothItem _ironGreaves;
        private static CameraData _cameraData;
        private static QuestJournalData _questJournalData;
        private static UIBestiaryData _uiBestiaryData;
        private static TemporaryBuff _healthBuffData;
        private static TemporaryBuff _staminaBuffData;
        private static BossData _bossData;
        private static TrapData _trapData;
        private static TrapData _trapData2;
        private static WeakPointData _bossFirstWeakPoint;
        private static WeakPointData _bossSecondWeakPoint;
        private static WeakPointData _bossThirdWeakPoint;
        private static UIElementsData _uiElementsData;
        private static MaterialsData _materialsData;
        private static TorchData _torchObjectData;
        private static HellHoundData _hellHoundData;
        private static TwoHeadedSnakeData _twoHeadedSnakeData;
        private static BouldersData _bouldersObjectData;
        private static HideBushData _hideBushData;
        private static AudioData _audioData;
        private static PlayerHealthBarData _playerHealthBarData;
        private static FallingTreeData _fallingTreeData;
        private static EnemyHealthBarData _enemyHealthBarData;
        private static SpawnpointsData _spawnpointsData;
        private static HubMapUIData _hubMapUIData;
        private static OneHandedThrowableWeapon _LureMeatData;
        private static OneHandedThrowableWeapon _LureCharcoalData;

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

        public static WeaponData BossFeasts
        {
            get
            {
                if (_bossFeast == null)
                {
                    _bossFeast = Resources.Load<WeaponData>("Data/" + Instance._bossFeastsPath);
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

        public static UIBestiaryData UIBestiaryData
        {
            get
            {
                if(_uiBestiaryData == null)
                {
                    _uiBestiaryData = Load<UIBestiaryData>("Data/" + Instance._uiBestiaryDataPath);
                }
                return _uiBestiaryData;
            }
        }

        public static TemporaryBuff HealthBuffData
        {
            get
            {
                if (_healthBuffData == null)
                {
                    _healthBuffData = Load<TemporaryBuff>("Data/" + Instance._healthBuffDataPath);
                }
                return _healthBuffData;
            }
        }

        public static TemporaryBuff StaminaBuffData
        {
            get
            {
                if (_staminaBuffData == null)
                {
                    _staminaBuffData = Load<TemporaryBuff>("Data/" + Instance._staminaBuffDataPath);
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

        public static UIElementsData UIElementsData
        {
            get
            {
                if (_uiElementsData == null)
                {
                    _uiElementsData = Load<UIElementsData>("Data/" + Instance._uiElementsDataPath);
                }
                return _uiElementsData;
            }
        }

        public static MaterialsData MaterialsData
        {
            get
            {
                if (_materialsData == null)
                {
                    _materialsData = Load<MaterialsData>("Data/" + Instance._materialsDataPath);
                }
                return _materialsData;
            }
        }

        public static TorchData TorchObjectData
        {
            get
            {
                if (_torchObjectData == null)
                {
                    _torchObjectData = Load<TorchData>("Data/" + Instance._torchObjectPath);
                }
                return _torchObjectData;
            }
        }

        public static HellHoundData HellHoundData
        {
            get
            {
                if (_hellHoundData == null)
                {
                    _hellHoundData = Resources.Load<HellHoundData>("Data/" + Instance._hellHoundDataPath);
                }
                return _hellHoundData;
            }
        }

        public static TwoHeadedSnakeData TwoHeadedSnakeData
        {
            get
            {
                if (_twoHeadedSnakeData == null)
                {
                    _twoHeadedSnakeData =
                        Resources.Load<TwoHeadedSnakeData>("Data/" + Instance._twoHeadedSnakeDataPath);
                }

                return _twoHeadedSnakeData;
            }
        }

        public static BouldersData BoulderObjectData
        {
            get
            {
                if (_bouldersObjectData == null)
                {
                    _bouldersObjectData = Load<BouldersData>("Data/" + Instance._bouldersObjectPath);
                }
                return _bouldersObjectData;
            }
        }

        public static HideBushData HideBushData
        {
            get
            {
                if (_hideBushData == null)
                {
                    _hideBushData = Resources.Load<HideBushData>("Data/" + Instance._hideBushDataPath);
                }
                return _hideBushData;
            }
        }

        public static AudioData AudioData
        {
            get
            {
                if (_audioData == null)
                {
                    _audioData = Resources.Load<AudioData>("Data/" + Instance._audioDataPath);
                }
                return _audioData;
            }
        }

        public static PlayerHealthBarData PlayerHealthBarData
        {
            get
            {
                if (_playerHealthBarData == null)
                {
                    _playerHealthBarData = Resources.Load<PlayerHealthBarData>("Data/" + Instance._playerHealthBarDataPath);
                }
                return _playerHealthBarData;
            }
        }

        public static FallingTreeData FallingTreeData
        {
            get
            {
                if (_fallingTreeData == null)
                {
                    _fallingTreeData = Resources.Load<FallingTreeData>("Data/" + Instance._fallingTreeDataPath);
                }
                return _fallingTreeData;
            }
        }

        public static EnemyHealthBarData EnemyHealthBarData
        {
            get
            {
                if (_enemyHealthBarData == null)
                {
                    _enemyHealthBarData = Resources.Load<EnemyHealthBarData>("Data/" + Instance._enemyHealthBarDataPath);
                }
                return _enemyHealthBarData;
            }
        }

        public static SpawnpointsData SpawnpointsData
        {
            get
            {
                if (_spawnpointsData == null)
                {
                    _spawnpointsData = Resources.Load<SpawnpointsData>("Data/" + Instance._spawnpointsDataPath);
                }
                return _spawnpointsData;
            }
        }

        public static HubMapUIData HubMapData
        {
            get
            {
                if (_hubMapUIData == null)
                {
                    _hubMapUIData = Resources.Load<HubMapUIData>("Data/" + Instance._hubMapUIDataPath);
                }
                return _hubMapUIData;
            }
        }

        public static OneHandedThrowableWeapon LureMeatData
        {
            get
            {
                if (_LureMeatData == null)
                {
                    _LureMeatData = Resources.Load<OneHandedThrowableWeapon>("Data/" + Instance._lureMeatDataPath);
                }
                return _LureMeatData;
            }
        }

        public static OneHandedThrowableWeapon LureCharcoalData
        {
            get
            {
                if (_LureCharcoalData == null)
                {
                    _LureCharcoalData = Resources.Load<OneHandedThrowableWeapon>("Data/" + Instance._lureCharcoalDataPath);
                }
                return _LureCharcoalData;
            }
        }

        #endregion


        #region Methods

        private static T Load<T> (string resourcesPath) where T : UnityEngine.Object =>
            Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));

        #endregion
    }
}
