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
            var bossData = Data.BossData;

            Vector3 instantiatePosition = bossData._bossSettings.InstantiatePosition;
            Vector3 groundedInstancePosition = GetGroundedPosition(instantiatePosition);

            GameObject instance = GameObject.Instantiate(bossData._bossSettings.Prefab);
            BossModel boss = new BossModel(instance, bossData, groundedInstancePosition, _context);

            _context.NpcModels.Add(instance.GetInstanceID(), boss);
        }

        #endregion


        #region Methods

        private Vector3 GetGroundedPosition(Vector3 startPosition)
        {
            Vector3 groundedPosition = new Vector3();

            bool isGroundBelow = Services.SharedInstance.PhysicsService.FindGround(startPosition, out groundedPosition);

            if (!isGroundBelow)
            {
                throw new System.Exception("Ground is above object's position!");
            }

            return groundedPosition;
        }

        #endregion
    }
}

