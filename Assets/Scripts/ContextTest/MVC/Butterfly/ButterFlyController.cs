using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BeastHunter
{
    public class ButterFlyController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ButterFlyController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.ButterFlyModel.Initilize();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {

        }

        #endregion
    }
}
