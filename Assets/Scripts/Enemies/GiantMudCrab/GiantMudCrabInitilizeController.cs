using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class GiantMudCrabInitilizeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public GiantMudCrabInitilizeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var GiantMudCrabData = Data.GiantMudCrabData;
            Vector3 spawnPoint = Vector3.zero;
            Services.SharedInstance.PhysicsService.FindGround(GiantMudCrabData.GiantMudCrabStruct.SpawnPoint, out spawnPoint);
            GameObject instance = GameObject.Instantiate(GiantMudCrabData.GiantMudCrabStruct.Prefab, spawnPoint, Quaternion.identity);
            GiantMudCrabModel GiantMudCrab = new GiantMudCrabModel(instance, GiantMudCrabData);
            _context.GiantMudCrabModel = GiantMudCrab;
        }

        #endregion
    }
}

