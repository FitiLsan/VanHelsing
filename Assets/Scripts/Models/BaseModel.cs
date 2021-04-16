using UnityEngine;

namespace BeastHunter
{
    public abstract class BaseModel
    {
        #region Fields

        public Stats CurrentStats;
        public Vector3 CurrentPosition => ObjectOnScene.transform.position;

        #endregion


        #region Properties

        public EnemyData ThisEnemyData { get; protected set; }
        public GameObject ObjectOnScene { get; protected set; }
        public GameObject BuffEffectPrefab { get; protected set; }
        public int InstanceID { get; protected set; }

        #endregion
    }
}