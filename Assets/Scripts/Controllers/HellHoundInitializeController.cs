using UnityEngine;

namespace BeastHunter
{
    public class HellHoundInitializeController : EnemyInitializeController
    {
        #region ClassLifeCycles

        public HellHoundInitializeController(GameContext context) : base(context)
        { }

        #endregion


        #region IAwake

        public override void OnAwake()
        {
            var hellHoundData = Data.HellHoundData;

            if (hellHoundData.BaseStats.Prefab == null)
            {
                hellHoundData.BaseStats.Prefab = Resources.Load<GameObject>("HellHound");
            }

            GameObject instance = Object.Instantiate(hellHoundData.BaseStats.Prefab);
            HellHoundModel hellHound = new HellHoundModel(instance, hellHoundData);
            _context.NpcModels.Add(instance.GetInstanceID(), hellHound);

            //GameObject instance2 = GameObject.Instantiate(hellHoundData.BaseStats.Prefab, new Vector3(495,1,495), Quaternion.identity);
            //HellHoundModel hellHound2 = new HellHoundModel(instance2, hellHoundData);
            //_context.NpcModels.Add(instance2.GetInstanceID(), hellHound2);
        }

        #endregion
    }
}
