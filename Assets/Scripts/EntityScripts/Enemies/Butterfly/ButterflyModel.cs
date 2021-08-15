using UnityEngine;

namespace BeastHunter
{
    public sealed class ButterflyModel : EnemyModel
    {
        #region Properties

        public ButterflyData ButterflyData { get; }

        #endregion


        #region ClassLifeCycle

        public ButterflyModel(GameObject prefab, ButterflyData data)
        {
            ButterflyData = data;
        }

        #endregion


        #region NpcModel

        public override void DoSmth(string how)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute()
        {
            if (!IsDead)
            {
                ButterflyData.Act(this);
            }
        }

        public override EnemyStats GetStats()
        {
            return ButterflyData.BaseStats;
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                ButterflyData.TakeDamage(this, damage);
                Debug.Log("Butterfly damage: " + damage);
            }
        }

        #endregion
    }
}
