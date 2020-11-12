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
            GameObject instance = GameObject.Instantiate(hellHoundData.BaseStats.Prefab);
            HellHoundModel hellHound = new HellHoundModel(instance, hellHoundData);
            instance.GetComponent<HellHoundBehaviour>().Initialize(hellHound);
            _context.NpcModels.Add(instance.GetInstanceID(), hellHound);
        }

        #endregion
    }
}
