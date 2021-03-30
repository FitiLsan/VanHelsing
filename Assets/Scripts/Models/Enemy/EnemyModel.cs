using UnityEngine;
using Extensions;


namespace BeastHunter
{
    public abstract class EnemyModel : ITakeDamage
    {
        #region Fields

        public Stats CurrentStats;

        #endregion


        #region Properties

        public GameObject ObjectOnScene { get; }
        public EnemyData ThisEnemyData { get; }
        public GameObject BuffEffectPrefab { get; protected set; }
        public int InstanceID { get; }

        #endregion


        #region ClassLifeCycle

        public EnemyModel(GameObject objectOnScene, EnemyData data)
        {
            ObjectOnScene = objectOnScene;
            ThisEnemyData = data;
            CurrentStats = ThisEnemyData.StartStats.DeepCopy();
            InstanceID = CurrentStats.InstanceID = objectOnScene.GetInstanceID();
            CurrentStats.BuffHolder = new BuffHolder();
        }

        //public abstract void HealthBarController();

        #endregion


        #region ITakeDamge

        public abstract void TakeDamage(Damage damage);

        #endregion
    }
}