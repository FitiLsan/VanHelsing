using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "TorchData", menuName = "CreateData/SimpleInteractiveObjects/TorchData", order = 0)]
    public sealed class TorchData : SimpleInteractiveObjectData
    {
        #region Fields

        [SerializeField] private Vector3 _prefabPosition;
        [SerializeField] private Vector3 _prefabEulers;

        [SerializeField] private float _lightRange;
        [SerializeField] private float _lightIntensivty;

        #endregion


        #region Properties

        public Vector3 PrefabPosition => _prefabPosition;
        public Vector3 PrefabEulers => _prefabEulers;

        public float LightRange => _lightRange;
        public float LightIntensivity => _lightIntensivty;

        #endregion


        #region Methods

        public override void MakeInteractive(BaseInteractiveObjectModel interactiveObjectModel, 
            ITrigger interactiveTrigger, Collider enteredCollider)
        {
            (interactiveObjectModel as TorchModel).CanvasObject.gameObject.SetActive(true);
            interactiveObjectModel.IsInteractive = true;
        }

        public override void MakeNotInteractive(BaseInteractiveObjectModel interactiveObjectModel, 
            ITrigger interactiveTrigger, Collider exitedCollider)
        {
            interactiveObjectModel.IsInteractive = false;
            (interactiveObjectModel as TorchModel).CanvasObject.gameObject.SetActive(false);
        }

        protected override void Activate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            (interactiveObjectModel as TorchModel).TorchFlameParticles.Play();
            (interactiveObjectModel as TorchModel).TorchLight.enabled = true;
        }

        protected override void Deactivate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            (interactiveObjectModel as TorchModel).TorchFlameParticles.Stop();
            (interactiveObjectModel as TorchModel).TorchLight.enabled = false;
        }

        #endregion
    }
}

