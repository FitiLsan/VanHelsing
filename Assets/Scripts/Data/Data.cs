using System.IO;
using UnityEngine;


namespace BeastHunter {
    [CreateAssetMenu (fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject {
        #region Fields

        [SerializeField] private string _sphereDataPath;
        [SerializeField] private string _characterDataPath;
        [SerializeField] private string _startDialogueDataPath;
        [SerializeField] private string _dialogueSystemDataPath;
        [SerializeField] private string _giantMudCrabDataPath;
        [SerializeField] private string _questIndicatorDataPath;
        [SerializeField] private string _questJournalDataPath;

        private static Data _instance;

        private static SphereData _sphereData;
        private static CharacterData _characterData;
        private static StartDialogueData _startDialogueData;
        private static DialogueSystemData _dialogueSystemData;
        private static GiantMudCrabData _giantMudCrabData;
        private static QuestIndicatorData _questIndicatorData;
        private static WeaponItem _feast;
        private static ClothItem _jacket;
        private static QuestJournalData _questJournalData;

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
                    _sphereData = Load<SphereData> ("Data/" + Instance._sphereDataPath);
                }
                return _sphereData;
            }
        }

        public static StartDialogueData StartDialogueData {
            get {
                if (_startDialogueData == null) {
                    _startDialogueData = Load<StartDialogueData> ("Data/" + Instance._startDialogueDataPath);
                }
                return _startDialogueData;
            }
        }

        public static DialogueSystemData DialogueSystemData {
            get {
                if (_dialogueSystemData == null) {
                    _dialogueSystemData = Load<DialogueSystemData> ("Data/" + Instance._dialogueSystemDataPath);
                }
                return _dialogueSystemData;
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

        public static GiantMudCrabData GiantMudCrabData {
            get {
                if (_giantMudCrabData == null) {
                    _giantMudCrabData = Load<GiantMudCrabData> ("Data/" + Instance._giantMudCrabDataPath);
                }
                return _giantMudCrabData;
            }
        }

        public static QuestIndicatorData QuestIndicatorData
        {
            get
            {
                if (_questIndicatorData == null)
                {
                    _questIndicatorData = Load<QuestIndicatorData>("Data/" + Instance._questIndicatorDataPath);
                }
                return _questIndicatorData;
            }
        }

        public static WeaponItem Feast {
            get {
                if (_feast == null) {
                    _feast = Resources.Load<WeaponItem> ("Data/Weapons/FeastWeapon"); //TODO Сделать по примеру выше через Instance.****DataPath) и не забыть указать путь в инспекторе!
                }
                return _feast;
            }
        }

        public static ClothItem Jacket {
            get {
                if (_jacket == null) {
                    _jacket = Resources.Load<ClothItem> ("Data/Clothes/Jacket"); //TODO Сделать по примеру выше через Instance.****DataPath)
                }
                return _jacket;
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

        #endregion


        #region Methods

        private static T Load<T> (string resourcesPath) where T : Object =>
            Resources.Load<T> (Path.ChangeExtension (resourcesPath, null));

        #endregion
    }
}