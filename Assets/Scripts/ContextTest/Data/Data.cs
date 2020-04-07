using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        public static SphereData _sphereData;
        public static CharacterData _characterData;

        public static StartDialogueData _startDialogueData;

        public static DialogueSystemData _dialogueSystemData;


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

        public static StartDialogueData StartDialogueData
        {
            get
            {
                if (_startDialogueData == null)
                {
                    _startDialogueData = Resources.Load<StartDialogueData>("Data/StartDialogueData");
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
                    _dialogueSystemData = Resources.Load<DialogueSystemData>("Data/DialogueSystemData");
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
                    _characterData = Resources.Load<CharacterData>("Data/CharacterData");
                }
                return _characterData;
            }
        }

        #endregion
    }
}