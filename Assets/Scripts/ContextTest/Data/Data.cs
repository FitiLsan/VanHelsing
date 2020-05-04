using System.IO;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _sphereDataPath;
        [SerializeField] private string _characterDataPath;
        [SerializeField] private string _startDialogueDataPath;
        [SerializeField] private string _dialogueSystemDataPath;
        private static Data _instance;
        public static SphereData _sphereData;
        public static CharacterData _characterData;
        public static StartDialogueData _startDialogueData;
        public static DialogueSystemData _dialogueSystemData;

        #endregion


        #region Properties

        public static Data Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<Data>("Data/" + typeof(Data).Name);
                }

                return _instance;
            }
        }

        public static SphereData SphereData
        {
            get
            {
                if (_sphereData == null)
                {
                    _sphereData = Load<SphereData>("Data/" + Instance._sphereDataPath);
                }
                return _sphereData;
            }
        }

        public static StartDialogueData StartDialogueData
        {
            get
            {
                if (_startDialogueData == null)
                {
                    _startDialogueData = Load<StartDialogueData>("Data/" + Instance._startDialogueDataPath);
                }
                return _startDialogueData;
            }
        }

        public static DialogueSystemData DialogueSystemData
        {
            get
            {
                if (_dialogueSystemData == null)
                {
                    _dialogueSystemData = Load<DialogueSystemData>("Data/" + Instance._dialogueSystemDataPath);
                }
                return _dialogueSystemData;
            }
        }

        public static CharacterData CharacterData
        {
            get
            {
                if (_characterData == null)
                {
                    _characterData = Load<CharacterData>("Data/" + Instance._characterDataPath);
                }
                return _characterData;
            }
        }

        #endregion


        #region Methods

        private static T Load<T>(string resourcesPath) where T : Object =>
           Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));

        #endregion
    }
}