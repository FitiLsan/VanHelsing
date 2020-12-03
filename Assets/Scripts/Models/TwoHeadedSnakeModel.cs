using System;
using UnityEngine;


namespace BeastHunter
{
    public class TwoHeadedSnakeModel : EnemyModel
    {

        #region Fields

        private TwoHeadedSnakeData _twoHeadedSnakeData;

        #endregion


        #region Properties



        #endregion


        #region ClassLifeCycle

        public TwoHeadedSnakeModel(GameObject prefab, TwoHeadedSnakeData twoHeadedSnakeData)
        {
            
        }

        #endregion


        #region NpcModel

        public override void OnAwake()
        {
            
        }

        public override void Execute()
        {
            
        }

        public override EnemyStats GetStats()
        {
            return _twoHeadedSnakeData.BaseStats;
        }

        public override void OnTearDown()
        {
            
        }

        public override void DoSmth(string how)
        {
            
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                _twoHeadedSnakeData.TakeDamage(this, damage);
            }
        }

        #endregion

    }
}
