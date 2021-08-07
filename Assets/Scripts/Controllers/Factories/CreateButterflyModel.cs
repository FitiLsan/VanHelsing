using System;
using UnityEngine;

namespace BeastHunter
{
    public sealed class CreateButterflyModel : CreateEnemyModel
    {
        #region Fields

        private readonly Func<GameObject> _playerAccessor;
        private const DataType CREATED_DATA_TYPE = DataType.Butterfly;

        #endregion

        
        #region ClassLifeCycle
        
        public CreateButterflyModel(Func<GameObject> playerAccessor)
        {
            _playerAccessor = playerAccessor;
        }

        #endregion
        

        #region Methods

        public override bool CanCreateModel(DataType data) => CREATED_DATA_TYPE == data;

        public override EnemyModel CreateModel(GameObject instance, EnemyData data)
        {
            var butterflyData = (ButterflyData)data;
            butterflyData.PlayerAccessor ??= _playerAccessor;
            return new ButterflyModel(butterflyData, instance);
        }

        #endregion
    }
}
