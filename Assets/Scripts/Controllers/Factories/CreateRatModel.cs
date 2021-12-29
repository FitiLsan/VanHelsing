using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateRatModel : CreateEnemyModel
    {
        #region Constants

        private const DataType CREATED_DATA_TYPE = DataType.Rat;

        #endregion


        #region Methods

        public override bool CanCreateModel(DataType data) => CREATED_DATA_TYPE == data;

        public override EnemyModel CreateModel(GameObject instance, EnemyData data) =>
            new RatModel(instance, (RatData)data);

        #endregion
    }
}