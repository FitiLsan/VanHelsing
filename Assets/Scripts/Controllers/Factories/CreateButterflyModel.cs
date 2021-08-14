using UnityEngine;

namespace BeastHunter
{
    public sealed class CreateButterflyModel : CreateEnemyModel
    {
        #region Fields

        private readonly GameObject _player;
        private const DataType CREATED_DATA_TYPE = DataType.Butterfly;

        #endregion

        
        #region ClassLifeCycle
        
        public CreateButterflyModel(GameObject player)
        {
            _player = player;
        }

        #endregion
        

        #region Methods

        public override bool CanCreateModel(DataType data) => CREATED_DATA_TYPE == data;

        public override EnemyModel CreateModel(GameObject instance, EnemyData data)
        {
            var butterflyData = (ButterflyData)data;
            return new ButterflyModel(butterflyData, instance, _player);
        }

        #endregion
    }
}
