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
            GameObject instance = GameObject.Instantiate(GiantMudCrabData.GiantMudCrabStruct.Prefab, GiantMudCrabData.GiantMudCrabStruct.SpawnPoint, Quaternion.identity);
            GiantMudCrabModel GiantMudCrab = new GiantMudCrabModel(instance, GiantMudCrabData);
            _context.GiantMudCrabModel = GiantMudCrab;
        }

        #endregion
    }
}

