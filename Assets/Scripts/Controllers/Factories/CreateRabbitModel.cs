using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateRabbitModel : CreateEnemyModel
    {
        #region Constants

        private const DataType CREATED_DATA_TYPE = DataType.Rabbit;

        #endregion


        #region Methods

        public override bool CanCreateModel(DataType data) => CREATED_DATA_TYPE == data;

        public override EnemyModel CreateModel(GameObject instance, EnemyData data) =>
            new RabbitModel(instance, (RabbitData)data);

        #endregion
    }
}