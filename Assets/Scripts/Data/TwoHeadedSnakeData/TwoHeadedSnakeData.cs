using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/TwoHeadedSnakeData", order = 9)]
    public sealed class TwoHeadedSnakeData : EnemyData
    {

        #region Fields

        public TwoHeadedSnakeSettings twoHeadedSnakeSettings;

        #endregion

        #region Methods

        public void Act(TwoHeadedSnakeModel twoHeadedSnake)
        {
           
        }

        #endregion
    }
}
