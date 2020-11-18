using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/LocationData")]
    public sealed class LocationData : ScriptableObject
    {
        #region Fields

        //[SerializeField] private static List<SpawnClass> _enemySpawnData;
        //[SerializeField] private static SpawnClass _playerSpawnData;
        //[SerializeField] private static SpawnClass _bossSpawnData;

        #endregion

        #region Properties

        //public List<SpawnClass> EnemySpawnData { get => _enemySpawnData; }
        public SpawnClass PlayerSpawnData;
        public SpawnClass BossSpawnData;
        //public SpawnClass OneEnemySpawnData;

        #endregion
    }

}
