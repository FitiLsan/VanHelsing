using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public interface ISpawnerable
    {
        #region Fields

        //TO DO on C# 8.0 ----> turn into fields, delete class
        //SpawnerableClass Spawner;
        SpawnerableClass SpawnerData { get; }

        #endregion


        #region Methods

        //void Spawn();

        #endregion
    }
}
