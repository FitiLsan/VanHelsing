using UnityEngine;

namespace BeastHunter
{
    public class TwoHeadedSnakeInitializeController : EnemyInitializeController
    {
        #region ClassLifeCycles

        public TwoHeadedSnakeInitializeController(GameContext context) : base(context)
        { }

        #endregion


        #region IAwake

        public override void OnAwake()
        {
            var TwoHeadedSnakeData = Data.TwoHeadedSnakeData;
            GameObject instance = GameObject.Instantiate(TwoHeadedSnakeData.BaseStats.Prefab);
            TwoHeadedSnakeModel twoHeadedSnake = new TwoHeadedSnakeModel(instance, TwoHeadedSnakeData);
            _context.NpcModels.Add(instance.GetInstanceID(), twoHeadedSnake);

        }

        #endregion
    }
}
