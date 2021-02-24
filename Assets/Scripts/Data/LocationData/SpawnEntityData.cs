using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public sealed class SpawnEntityData
    {
        #region Fields

        [SerializeField] private DataType _spawningDataType;
        [SerializeField] private EnemyData _spawningEnemyData;
        [SerializeField] public float SpawningChance;

        #endregion


        #region Properties

        public DataType SpawningDataType { get => _spawningDataType; }
        public EnemyData SpawningEnemyData { get => _spawningEnemyData; }

        #endregion
    }
}
