using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public sealed class SpawnPointData
    {
        #region Fields

        [SerializeField] private List<SpawnEntityData> _spawnDataList;
        [SerializeField] private Vector3 _spawnPoint;
        [SerializeField] private int _spawnRadius;
        [SerializeField] private int _numberToSpawn;

        #endregion


        #region Properties

        public List<SpawnEntityData> SpawnDataList { get => _spawnDataList; }
        public Vector3 SpawnPoint { get => _spawnPoint; }
        public int SpawnRadius { get => _spawnRadius; }
        public int NumberToSpawn { get => _numberToSpawn; }

        #endregion
    }
}
