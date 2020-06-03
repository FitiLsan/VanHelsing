using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public abstract class NpcModel : IDoSmth //IDoDamage, ITakeDamage
    {
        #region Fields

        public float CurrentHealth;
        public bool IsDead;

        #endregion


        #region Metods

        public abstract void Execute();

        #endregion


        #region IDoSmth

        public abstract void DoSmth(string how);

        #endregion
    }
}