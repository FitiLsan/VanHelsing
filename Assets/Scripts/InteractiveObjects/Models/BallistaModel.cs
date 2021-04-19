using UniRx;
using UnityEngine;


namespace BeastHunter
{
    public sealed class BallistaModel : SimpleInteractiveObjectModel
    {
        #region Properties

        public BallistaAnimationController BallistaAnimationController { get; private set; }


        #endregion


        #region ClassLifeCycle

        public BallistaModel(GameObject prefab, BallistaData data) : base(prefab, data)
        {
            BallistaAnimationController = prefab.GetComponent<BallistaAnimationController>();
            IsNeedControl = true;
            MessageBroker.Default.Receive<PlayerInteractionCatch>().Subscribe(data.CallTriggerInteraction);
        }

        #endregion
    }
}

