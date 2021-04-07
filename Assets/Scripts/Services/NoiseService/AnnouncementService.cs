using UniRx;


namespace BeastHunter
{
    public sealed class AnnouncementService : IService
    {
        #region Methods

        public void MakeNoise(Noise noise)
        {
            MessageBroker.Default.Publish(noise);
        }

        public void MakeDrag(Drag drag)
        {
            MessageBroker.Default.Publish(drag);
        }

        #endregion
    }
}

