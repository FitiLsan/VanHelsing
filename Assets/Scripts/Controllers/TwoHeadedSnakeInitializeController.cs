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
            Debug.Log(Data.TwoHeadedSnakeData == null);
            //if (TwoHeadedSnakeData.BaseStats.Prefab == null)
            //{
            //    TwoHeadedSnakeData.BaseStats.Prefab = Resources.Load<GameObject>("Amphisbaena_04");
            //}

            GameObject instance = GameObject.Instantiate(TwoHeadedSnakeData.BaseStats.Prefab);
            TwoHeadedSnakeModel twoHeadedSnake = new TwoHeadedSnakeModel(instance, TwoHeadedSnakeData);
            _context.NpcModels.Add(instance.GetInstanceID(), twoHeadedSnake);

        }

        #endregion
    }
}
