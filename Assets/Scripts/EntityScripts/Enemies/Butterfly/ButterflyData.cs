using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/Butterfly", order = 2)]
    public sealed class ButterflyData : EnemyData
    {

        #region Metods

        public void Act(ButterflyModel butterfly)
        {
            Debug.Log("Butterfly flies");
        }

        #endregion

    }
}