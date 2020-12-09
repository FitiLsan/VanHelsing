using UnityEngine;


namespace BeastHunter
{
    public sealed class HideBushInitializeController : InteractiveObjectInitializeController
    {
        #region ClassLifeCycle

        public HideBushInitializeController(GameContext context) : base(context)
        {
        }

        #endregion


        #region IAwake

        protected override void Initialize()
        {
            GameObject instance = GameObject.Instantiate(Data.HideBushData.Prefab,
                 Services.SharedInstance.PhysicsService.GetGroundedPosition(Data.HideBushData.PrefabPosition), 
                    Quaternion.Euler(Data.HideBushData.PrefabEulers));

            _context.InteractableObjectModels.Add(instance.GetInstanceID(),
                new HideBushModel(instance, Data.HideBushData));
        }

        #endregion
    }
}

