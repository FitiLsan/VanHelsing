using UnityEngine;
using UnityEngine.Audio;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "CreateData/AudioData", order = 0)]
    public sealed class AudioData : ScriptableObject
    {
        #region Fields

        [Header("Main mixer")]
        [SerializeField] private AudioMixer _mainMixer;
        [SerializeField] private string _characterMovementVolumeName;

        [Header("Ambience")]
        [SerializeField] private GameObject _ambienceObjectPrefab;
        [SerializeField] private string _firstAmbientMusicSourceObjectName;
        [SerializeField] private string _secondAmbientMusicSourceObjectName;
        [SerializeField] private string _firstAmbientSFXSourceObjectName;
        [SerializeField] private string _secondAmbientSFXSourceObjectName;

        [SerializeField] private Sound[] _ambientMusicArray;
        [SerializeField] private Sound[] _ambientSFXArray;

        #endregion

        #region Properties

        public AudioMixer MainMixer => _mainMixer;
        public GameObject AmbienceObjectPrefab => _ambienceObjectPrefab;
        public string CharacterMovementVolumeName => _characterMovementVolumeName;
        public string FirstAmbientMusicSourceObjectName => _firstAmbientMusicSourceObjectName;
        public string SecondAmbientMusicSourceObjectName => _secondAmbientMusicSourceObjectName;
        public string FirstAmbientSFXSourceObjectName => _firstAmbientSFXSourceObjectName;
        public string SecondAmbientSFXSourceObjectName => _secondAmbientSFXSourceObjectName;

        public Sound[] AmbientMusicArray => _ambientMusicArray;
        public Sound[] AmbientSFXArray => _ambientSFXArray;

        #endregion
    }
}

