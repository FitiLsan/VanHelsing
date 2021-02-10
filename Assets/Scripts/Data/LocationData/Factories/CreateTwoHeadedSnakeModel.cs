using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateTwoHeadedSnakeModel : CreateEnemyModel
    {
        #region Constants

        private const DataType CREATED_DATA_TYPE = DataType.TwoHeadedSnake;

        #endregion


        #region Methods

        public override bool CanCreateModel(DataType data) => CREATED_DATA_TYPE == data;

        public override EnemyModel CreateModel(GameObject instance, EnemyData data) =>
            new TwoHeadedSnakeModel(instance, (TwoHeadedSnakeData)data);

        #endregion
    }
}

