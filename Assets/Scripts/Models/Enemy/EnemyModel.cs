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

        #endregion


        #region ClassLifeCycle

        public EnemyModel(GameObject objectOnScene, EnemyData data)
        {
            ObjectOnScene = objectOnScene;
            ThisEnemyData = data;
            CurrentStats = ThisEnemyData.StartStats.DeepCopy();
        }

        #endregion


        #region ITakeDamge

        public abstract void TakeDamage(Damage damage);

        #endregion
    }
}