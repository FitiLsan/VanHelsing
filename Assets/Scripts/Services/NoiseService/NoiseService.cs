using UniRx;


namespace BeastHunter
{
    public sealed class NoiseService : IService
    {
        #region Methods

        public void MakeNoise(Noise noise)
        {
            MessageBroker.Default.Publish(noise);
        }

        #endregion
    }
}

