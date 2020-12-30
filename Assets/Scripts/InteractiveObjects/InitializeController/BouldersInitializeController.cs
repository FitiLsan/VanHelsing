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
            GameObject gameobject = Object.Instantiate(Data.BoulderObjectData.Prefab, new Vector3(514.99f, 14.172f, 764.55f), Quaternion.identity);
            _context.InteractableObjectModels.Add(gameobject.GetInstanceID(), new BouldersModel(gameobject, Data.BoulderObjectData));
        }

        #endregion
    }
}

