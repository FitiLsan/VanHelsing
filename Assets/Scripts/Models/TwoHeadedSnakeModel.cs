using System;
using UnityEngine;


namespace BeastHunter
{
    public class TwoHeadedSnakeModel : EnemyModel
    {

        #region Fields



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
            throw new NotImplementedException();
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override EnemyStats GetStats()
        {
            throw new NotImplementedException();
        }

        public override void OnTearDown()
        {
            throw new NotImplementedException();
        }

        public override void DoSmth(string how)
        {
            throw new NotImplementedException();
        }

        public override void TakeDamage(Damage damage)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
