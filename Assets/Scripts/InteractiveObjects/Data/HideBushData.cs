using UnityEngine;
using UniRx;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HideBushData", menuName = "CreateData/SimpleInteractiveObjects/HideBushData", order = 0)]
    public sealed class HideBushData : SimpleInteractiveObjectData
    {
        #region Fields

        [SerializeField] private Vector3 _prefabPosition;
        [SerializeField] private Vector3 _prefabEulers;

        #endregion


        #region Properties

        public Vector3 PrefabPosition => _prefabPosition;
        public Vector3 PrefabEulers => _prefabEulers;

        #endregion


        #region Methods

        public override void MakeInteractive(BaseInteractiveObjectModel interactiveObjectModel,
            ITrigger interactiveTrigger, Collider enteredCollider)
        {
            MessageBroker.Default.Publish(new OnPlayerReachHidePlaceEventClass(true));
        }

        public override void MakeNotInteractive(BaseInteractiveObjectModel interactiveObjectModel,
            ITrigger interactiveTrigger, Collider exitedCollider)
        {
            MessageBroker.Default.Publish(new OnPlayerReachHidePlaceEventClass(false));
            MessageBroker.Default.Publish(new OnPlayerHideEventClass(false));
        }

        protected override void Activate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            // TODO
        }

        protected override void Deactivate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            // TODO
        }

        #endregion
    }
}

