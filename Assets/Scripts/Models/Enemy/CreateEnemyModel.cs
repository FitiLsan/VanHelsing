using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public abstract class CreateEnemyModel
    {
        public abstract EnemyModel CreateModel(GameObject instance, EnemyData data);
    }

}
