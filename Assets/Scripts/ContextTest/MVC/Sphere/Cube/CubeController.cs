using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class CubeController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public CubeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.CubeModel.Initilize();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {

        }

        #endregion
    }
}

