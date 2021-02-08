using UnityEngine;

namespace BeastHunter
{
    public sealed class CreateHellHoundModel : CreateEnemyModel
    {
        #region Constants

        private const DataType CREATED_DATA_TYPE = DataType.HellHound;

        #endregion


        #region Methods

        public override bool CanCreateModel(DataType data) => CREATED_DATA_TYPE == data;

        public override EnemyModel CreateModel(GameObject instance, EnemyData data) => 
            new HellHoundModel(instance, (HellHoundData)data);

        #endregion
    }
}

