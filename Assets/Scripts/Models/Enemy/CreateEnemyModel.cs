using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public abstract class CreateEnemyModel
    {
        #region Methods

        public abstract bool CanCreateModel(DataType data);

        public abstract EnemyModel CreateModel(GameObject instance, EnemyData data);

        #endregion
    }
}
