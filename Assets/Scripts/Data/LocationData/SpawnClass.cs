using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public class SpawnClass
    {
        #region Fields

        //[SerializeField] private EnemyData _spawningEnemyData;
        [SerializeField] private Vector3 _spawnPoint;
        //[SerializeField] private int _spawnRadius;
        //[SerializeField] private int _numberToSpawn;

        #endregion


        #region Properties

        //public EnemyData SpawningEnemyData { get => _spawningEnemyData; }
        public Vector3 SpawnPoint { get => _spawnPoint; }
        //public int SpawnRadius { get => _spawnRadius; }
        //public int NumberToSpawn { get => _numberToSpawn; }

        #endregion


        #region ClassLifeCycle

        //public SpawnClass(EnemyData data, Vector3 spawnpoint, int number)
        //{
        //    _spawningEnemyData = data;
        //    _spawnPoint = spawnpoint;
        //    _numberToSpawn = number;
        //}

        #endregion
    }

}
