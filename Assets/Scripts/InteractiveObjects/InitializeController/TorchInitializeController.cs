using UnityEngine;


namespace BeastHunter
{
    public class TorchInitializeController : InteractiveObjectInitializeController
    {
        #region ClassLifeCycle

        public TorchInitializeController(GameContext context) : base(context)
        {
        }

        #endregion


        #region IAwake

        protected override void Initialize()
        {
            GameObject instance = GameObject.Instantiate(Data.TorchObjectData.Prefab, 
                Data.TorchObjectData.PrefabPosition, Quaternion.Euler(Data.TorchObjectData.PrefabEulers));

            _context.InteractableObjectModels.Add(instance.GetInstanceID(), 
                new TorchModel(instance, Data.TorchObjectData));
        }

        #endregion
    }
}

