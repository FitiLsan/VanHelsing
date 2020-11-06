using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewHellHound", menuName = "CreateData/HellHound", order = 2)]
    public sealed class HellHoundData : EnemyData
    {
        #region PrivateData


        #endregion


        #region Constants


        #endregion


        #region Fields

        public HellHoundStats Stats;

        #endregion


        #region ClassLifeCycles


        #endregion


        #region Methods

        public void Act(HellHoundModel model)
        {

        }

        #endregion


        #region EnemyData

        public override void TakeDamage(EnemyModel model, Damage damage)
        {
            base.TakeDamage(model, damage);

            if (model.IsDead)
            {
                Debug.Log("Hell hound is dead");
            }
        }

        #endregion
    }
}
