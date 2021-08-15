using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class CreateButterflyModel : CreateEnemyModel
    {
        #region Constants

        private const DataType CREATED_DATA_TYPE = DataType.Butterfly;

        #endregion

        public override bool CanCreateModel(DataType data)
        {
            return CREATED_DATA_TYPE == data;
        }

        public override EnemyModel CreateModel(GameObject instance, EnemyData data)
        {
            return new ButterflyModel(instance, (ButterflyData)data);
        }
    }
}
