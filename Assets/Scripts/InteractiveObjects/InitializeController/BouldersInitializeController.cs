using UnityEngine;


namespace BeastHunter
{
    public class BouldersInitializeController : InteractiveObjectInitializeController
    {
        #region ClassLifeCycle

        public BouldersInitializeController(GameContext context) : base(context)
        {
        }

        #endregion


        #region InteractiveObjectInitializeController

        protected override void Initialize()
        {
            GameObject gameobject = Object.Instantiate(Data.BoulderObjectData.Prefab, Data.BoulderObjectData.PrefabPosition, Quaternion.Euler(Data.BoulderObjectData.PrefabEulers));
            _context.InteractableObjectModels.Add(gameobject.GetInstanceID(), new BouldersModel(gameobject, Data.BoulderObjectData));
        }

        #endregion
    }
}

