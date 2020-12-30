using UnityEngine;


namespace BeastHunter
{
    [System.Serializable]
    public sealed class Sound
    {
        #region Fields

        [SerializeField] private AudioClip _soundClip;
        [Range(0f, 1f)]
        [SerializeField] private float _volume;
        [Range(-3f, 3f)]
        [SerializeField] private float _pitch;
        [SerializeField] private bool _doLoop;

        #endregion


        #region Properties

        public AudioClip SoundClip => _soundClip;
        public float Volume => _volume;
        public float Pitch => _pitch;
        public bool DoLoop => _doLoop;

        #endregion
    }
}

