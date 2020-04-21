using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class RabbitController : IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public RabbitController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            foreach(var rabbit in _context.RabbitModel)
            {
                rabbit.Execute();
            }
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {

        }

        #endregion        
    }
}
