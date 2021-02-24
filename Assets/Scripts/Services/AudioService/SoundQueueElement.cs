namespace BeastHunter
{
    public sealed class SoundQueueElement
    {
        #region Properties

        public Sound Sound { get; }
        public float BlendTIme { get; }
        public bool DoOverlap { get; }

        #endregion


        #region ClassLifeCycle

        public SoundQueueElement(Sound sound, float blendTime = 0f, bool doOverlap = false)
        {
            Sound = sound;
            BlendTIme = blendTime;
            DoOverlap = doOverlap;
        }

        #endregion
    }
}

