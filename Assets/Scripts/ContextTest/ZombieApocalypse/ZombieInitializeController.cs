using UnityEngine;


namespace BeastHunter
{
    public sealed class ZombieInitializeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ZombieInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion
        
        
        #region IAwake

        public void OnAwake()
        {
            var zombieData = Data.ZombieData;
            var instance = GameObject.Instantiate(zombieData.TestCharacterStruct.Prefab);
            var zombieModel = new ZombieModel(instance, zombieData);

            _context.ZombieModel = zombieModel;
        }

        #endregion
    }
}