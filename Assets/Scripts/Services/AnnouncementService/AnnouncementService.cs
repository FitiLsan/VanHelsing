using System;
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

        public void MakeSmell(Smell smell)
        {
            MessageBroker.Default.Publish(smell);
        }

        public IDisposable TakeNoise(Action<Noise> onDone)
        {
            return MessageBroker.Default.Receive<Noise>().Subscribe(onDone);
        }

        public IDisposable TakeSmell(Action<Smell> onDone)
        {
          return  MessageBroker.Default.Receive<Smell>().Subscribe(onDone);
        }

        #endregion
    }
}

