using UnityEngine;
using Extensions;
using System.Collections.Generic;

namespace BeastHunter
{
    public abstract class EnemyModel : BaseModel, ITakeDamage
    {
        #region Fields

        public List<GameObject> FoodListInSight = new List<GameObject>();

        #endregion


        #region Properties

        //public GameObject ObjectOnScene { get; }
        //public EnemyData ThisEnemyData { get; }
        //public GameObject BuffEffectPrefab { get; protected set; }
        //public int InstanceID { get; }

        #endregion


        #region ClassLifeCycle

        public EnemyModel(GameObject objectOnScene, EnemyData data)
        {
            ObjectOnScene = objectOnScene;
            ThisEnemyData = data;
            CurrentStats = ThisEnemyData.StartStats.DeepCopy();
            InstanceID = CurrentStats.InstanceID = objectOnScene.GetInstanceID();
            CurrentStats.BuffHolder = new BuffHolder();
            CurrentStats.ItemReactions = ThisEnemyData.ItemReactions;
        }

        //public abstract void HealthBarController();

        #endregion


        #region ITakeDamge

        public abstract void TakeDamage(Damage damage);

        #endregion
    }
}