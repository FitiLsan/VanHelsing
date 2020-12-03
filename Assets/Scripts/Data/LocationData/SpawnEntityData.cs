using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public sealed class SpawnEntityData //where T:ScriptableObject
    {
        #region Fields

        [SerializeField] private DataType _spawningDataType;
        [SerializeField] private EnemyData _spawningEnemyData;
        [SerializeField] private int _spawningChance;

        #endregion


        #region Properties

        public DataType SpawningDataType { get => _spawningDataType; }
        public EnemyData SpawningEnemyData { get => _spawningEnemyData; }
        public int SpawningChance { get => _spawningChance; }

        #endregion
    }
}
