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
        [SerializeField] private LocationPosition _spawnPosition;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private int _numberToSpawn;

        #endregion


        #region Properties

        public List<SpawnEntityData> SpawnDataList { get => _spawnDataList; }
        public LocationPosition SpawnPosition { get => _spawnPosition; }
        public float SpawnRadius { get => _spawnRadius; }
        public int NumberToSpawn { get => _numberToSpawn; }

        #endregion


        #region Methods

        public SpawnPointData()
        {
            _spawnDataList = new List<SpawnEntityData>();
            _spawnPosition = new LocationPosition(Vector3.zero, Vector3.zero, Vector3.zero);
            _spawnRadius = 0.0f;
            _numberToSpawn = 1;
        }

        public SpawnPointData(List<SpawnEntityData> spawnEntityDatas, LocationPosition position, float radius, int number)
        {
            _spawnDataList = spawnEntityDatas; //COPY
            _spawnPosition = position;
            _spawnRadius = radius;
            _numberToSpawn = number;
        }

        #endregion
    }
}
