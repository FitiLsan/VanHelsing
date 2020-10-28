using System;
using BeastHunter;
using UnityEngine;

namespace Assets.Scripts.EntityScripts.Example.Butterfly
{
    public sealed class ButterflyController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion

        #region ClassLifeCycle

        public ButterflyController(GameContext context)
        {
            _context = context;
        }

        #endregion

        #region IUpdate

        public void Updating()
        {
            _context.ButterflyModel.Initilize();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {

        }

        #endregion
    }
}
