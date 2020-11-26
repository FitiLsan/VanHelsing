using UnityEngine;


namespace BeastHunter
{
    public sealed class BossInitializeController : EnemyInitializeController
    {
        #region ClassLifeCycle

        public BossInitializeController(GameContext context) : base(context)
        { }

        #endregion


        #region IAwake

        public override void OnAwake()
        {
            var locationData = Resources.Load<LocationData>("Data/LocationData");
            var spawnPoint = Services.SharedInstance.PhysicsService.
                GetGroundedPosition(locationData.BossSpawnData.SpawnPoint); 
            var bossData = Data.BossData;

            GameObject instance = GameObject.Instantiate(bossData._bossSettings.Prefab);
            BossModel boss = new BossModel(instance, bossData, spawnPoint, _context);

            _context.NpcModels.Add(instance.GetInstanceID(), boss);
        }

        #endregion
    }
}

