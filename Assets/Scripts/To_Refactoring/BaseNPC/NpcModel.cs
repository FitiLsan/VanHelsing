using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public abstract class NpcModel : IDoSmth, ITakeDamage
    {
        #region Fields

        public float CurrentHealth;
        public bool IsDead;

        #endregion


        #region Metods

        public abstract void Execute();

        public abstract NpcStats GetStats(); 

        #endregion


        #region IDoSmth

        public abstract void DoSmth(string how);

        #endregion

        #region ITakeDamge

        public abstract void TakeDamage(Damage damage);

        #endregion
    }
}