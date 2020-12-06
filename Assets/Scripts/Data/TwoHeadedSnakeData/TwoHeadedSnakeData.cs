using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/TwoHeadedSnakeData", order = 9)]
    public sealed class TwoHeadedSnakeData : EnemyData
    {

        #region Fields

        public TwoHeadedSnakeStats twoHeadedSnakeStats;

        #endregion

        #region Methods

        public void Act(TwoHeadedSnakeModel twoHeadedSnake)
        {
           twoHeadedSnakeStats.InstantiatePosition = new Vector3(0,0,0);
        }

        #endregion
    }
}
