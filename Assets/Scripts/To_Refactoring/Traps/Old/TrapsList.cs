using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class TrapsList : IAwake, IUpdate
    {
        #region Field

        private GameContext _context;
        //public List<TrapController> TrapControllers;
        public InitializeTrapController InitializeTrapController;

        #endregion


        #region ClassLifeCycle

        public TrapsList(GameContext context)
        {
            _context = context;
            _context.TrapsList = this;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            if(InitializeTrapController != null)
            {
                //InitializeTrapController.OnAwake();
            }
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            //foreach (var TrapController in TrapControllers)
            //{
            //    TrapController.Updating();
            //}
        }

        #endregion
    }
}