using UnityEngine;

namespace BeastHunter
{
    internal class CreateButterflyModel : CreateEnemyModel
    {
        #region Constants

        private const DataType CREATED_DATA_TYPE = DataType.Butterfly;

        #endregion


        #region Methods

        public override bool CanCreateModel(DataType data) => CREATED_DATA_TYPE == data;

        public override EnemyModel CreateModel(GameObject instance, EnemyData data)
        {
            return new ButterflyModel(instance, (ButterflyData)data);
        }

        #endregion
    }
}