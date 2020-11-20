using UnityEngine;


namespace BeastHunter
{
    public sealed class TorchModel : SimpleInteractiveObjectModel
    {
        #region Properties

        public Light TorchLight { get; private set; }
        public ParticleSystem TorchFlameParticles { get; private set; }

        #endregion


        #region ClassLifeCycle

        public TorchModel(GameObject prefab, TorchData data) : base(prefab, data)
        {
            TorchLight = prefab.GetComponentInChildren<Light>();
            TorchFlameParticles = prefab.GetComponentInChildren<ParticleSystem>();

            TorchLight.intensity = (InteractiveObjectData as TorchData).LightIntensivity;
            TorchLight.range = (InteractiveObjectData as TorchData).LightRange;
        }

        #endregion
    }
}

