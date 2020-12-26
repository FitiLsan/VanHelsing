using UnityEngine;
using BeastHunter;


namespace Extensions
{
    public static partial class AudioSourceExtensions
    {
        public static void PlayOneShot(this AudioSource source, Sound soundToPlay)
        {
            source.volume = soundToPlay.Volume;
            source.pitch = soundToPlay.Pitch;
            source.PlayOneShot(soundToPlay.SoundClip);
        }

        public static void Play(this AudioSource source, Sound soundToPlay)
        {
            source.volume = soundToPlay.Volume;
            source.pitch = soundToPlay.Pitch;
            source.loop = soundToPlay.DoLoop;
            source.clip = soundToPlay.SoundClip;
            source.Play();
        }
    }

}
