using UnityEngine;


namespace BeastHunter
{
    public sealed class SurvivorInitializeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public SurvivorInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion
        
        
        #region IAwake

        public void OnAwake()
        {
            var survivorData = Data.SurvivorData;
            var instance = GameObject.Instantiate(survivorData.TestCharacterStruct.Prefab);
            var survivorModel = new SurvivorModel(instance, survivorData);

            _context.SurvivorModel = survivorModel;
        }

        #endregion
    }
}