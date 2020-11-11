using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public abstract class InteractionObjectModel
    {
        #region Fields

        public bool isInteraction;

        #endregion


        #region Metods

        public abstract void OnAwake();

        public abstract void Execute();

        public abstract void OnTearDown();

        #endregion
    }
}
