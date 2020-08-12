using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class CubeInitilizeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public CubeInitilizeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var CubeData = Data.CubeData;
            GameObject instance = GameObject.Instantiate(CubeData.CubeStruct.Prefab);
            CubeModel Cube = new CubeModel(instance, CubeData);
            _context.CubeModel = Cube;
        }

        #endregion
    }
}
