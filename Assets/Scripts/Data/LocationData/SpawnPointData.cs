using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public sealed class SpawnPointData
    {
        #region Fields

        [SerializeField] private List<SpawnEntityData> _spawnDataList;
        [SerializeField] private Vector3 _spawnPoint;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private int _numberToSpawn;

        #endregion


        #region Properties

        public List<SpawnEntityData> SpawnDataList { get => _spawnDataList; }
        public Vector3 SpawnPoint { get => _spawnPoint; }
        public float SpawnRadius { get => _spawnRadius; }
        public int NumberToSpawn { get => _numberToSpawn; }

        #endregion


        public SpawnPointData()
        {
            _spawnDataList = new List<SpawnEntityData>();
            _spawnPoint = new Vector3();
            _spawnRadius = 0.0f;
            _numberToSpawn = 1;
        }
        public SpawnPointData(List<SpawnEntityData> spawnEntityDatas, Vector3 point, float radius, int number)
        {
            _spawnDataList = spawnEntityDatas; //COPY
            _spawnPoint = point;
            _spawnRadius = radius;
            _numberToSpawn = number;
        }
    }
}
