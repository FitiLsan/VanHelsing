using UniRx;


namespace BeastHunter
{
    public sealed class NoiseService : Service
    {
        #region ClassLifeCycle

        public NoiseService(Contexts contexts) : base(contexts)
        {
        }

        #endregion


        #region Methods

        public void MakeNoise(Noise noise)
        {
            MessageBroker.Default.Publish(noise);
        }

        #endregion
    }
}

