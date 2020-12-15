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
            var locationData = Data.LocationData;
            var bossData = Data.BossData;

            var spawnPoint = locationData.BossSpawnPosition; 
            Vector3 groundedInstancePosition = Services.SharedInstance.PhysicsService.GetGroundedPosition(spawnPoint);

            GameObject instance = GameObject.Instantiate(bossData._bossSettings.Prefab);
            BossModel boss = new BossModel(instance, bossData, groundedInstancePosition, _context);
            //BossModel boss = new BossModel(instance, bossData, spawnPoint, _context);

            _context.NpcModels.Add(instance.GetInstanceID(), boss);
        }

        #endregion
    }
}

