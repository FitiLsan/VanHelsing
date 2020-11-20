using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class CreateRabbitModel : CreateEnemyModel
    {
        public override EnemyModel CreateModel(GameObject instance, EnemyData data)
        {
            return new RabbitModel(instance, (RabbitData)data);
        }
    }
}

