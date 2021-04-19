using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "BallistaData", menuName = "CreateData/SimpleInteractiveObjects/BallistaData", order = 0)]
    public sealed class BallistaData : SimpleInteractiveObjectData
    {
        #region Fields

        [SerializeField] private Vector3 _prefabPosition;
        [SerializeField] private Vector3 _prefabEulers;

        [SerializeField] private float _shotForce;
        [SerializeField] private GameObject _ballistaBolt;
        private GameObject currentTarget;
        private PlayerInteractionCatch _playerInteractionCatch;

        #endregion


        #region Properties

        public Vector3 PrefabPosition => _prefabPosition;
        public Vector3 PrefabEulers => _prefabEulers;

        public float ShotForce => _shotForce;
        public GameObject BallistaBolt => _ballistaBolt;

        #endregion


        #region Methods

        public override void MakeInteractive(BaseInteractiveObjectModel interactiveObjectModel, 
            ITrigger interactiveTrigger, Collider enteredCollider)
        {
            if(interactiveTrigger.GameObject != currentTarget)
            {
                return;
            }

            (interactiveObjectModel as BallistaModel).CanvasObject.gameObject.SetActive(true);
            interactiveObjectModel.IsInteractive = true;
        }

        public override void MakeNotInteractive(BaseInteractiveObjectModel interactiveObjectModel, 
            ITrigger interactiveTrigger, Collider exitedCollider)
        {
            interactiveObjectModel.IsInteractive = false;
            (interactiveObjectModel as BallistaModel).CanvasObject.gameObject.SetActive(false);
            currentTarget = null;
        }

        protected override void Activate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            (interactiveObjectModel as BallistaModel).BallistaAnimationController.IsActive = true;
            (interactiveObjectModel as BallistaModel).CanvasObject.gameObject.SetActive(false);
            _playerInteractionCatch.StartInteract();
        }

        protected override void Deactivate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            (interactiveObjectModel as BallistaModel).BallistaAnimationController.IsActive = false;
            (interactiveObjectModel as BallistaModel).CanvasObject.gameObject.SetActive(true);
            _playerInteractionCatch.StopInteract();
        }

        public void CallTriggerInteraction(PlayerInteractionCatch playerInteractionCatch)
        {
            _playerInteractionCatch = playerInteractionCatch;
            currentTarget = playerInteractionCatch.target.gameObject;
        }

        #endregion
    }
}

