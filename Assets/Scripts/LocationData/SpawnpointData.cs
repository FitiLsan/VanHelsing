using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public sealed class SpawnPointData
    {
        #region Fields

        [SerializeField] private SpawnEntityData _spawnData;
        [SerializeField] private LocationPosition _spawnPosition;

        #endregion


        #region Properties

        public SpawnEntityData SpawnData { get => _spawnData; }
        public LocationPosition SpawnPosition { get => _spawnPosition; }

        #endregion


        #region Methods

        public SpawnPointData()
        {
            _spawnData = new SpawnEntityData();
            _spawnPosition = new LocationPosition(Vector3.zero, Vector3.zero, Vector3.zero);
        }

        public SpawnPointData(SpawnEntityData spawnEntityData, LocationPosition position)
        {
            _spawnData = spawnEntityData; //COPY
            _spawnPosition = position;
        }

        #endregion
    }
}