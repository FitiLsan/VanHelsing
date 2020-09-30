using UnityEngine;

namespace BeastHunter
{
    public class InitializeController<T> : EnemyInitializeController where T: EnemyModel
    {
        #region ClassLifeCycles

        public InitializeController(GameContext context) : base(context) 
        { }

        #endregion


        #region IAwake

        public override void OnAwake()
        {
            //var data = Data.T;//Enemy model: add DataName
            //var SpawnPoint = GameObject.Find(data.BaseStats.SpawnPointName);
            //GameObject instance = GameObject.Instantiate(data.BaseStats.Prefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
            //T enemy = new T(instance, data);
            //_context.EnemyModels.Add(instance.GetInstanceID(), enemy);
        }

        #endregion
    }
}
