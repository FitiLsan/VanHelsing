using UnityEngine;


namespace BeastHunter
{
    public abstract class EnemyData : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject _prefab;
        [SerializeField] private Stats _startStats;
        [SerializeField] private ItemReactions _itemReactions;

        #endregion


        #region Properties

        public GameObject Prefab => _prefab;
        public Stats StartStats => _startStats;
        public ItemReactions ItemReactions => _itemReactions;

        #endregion


        #region Methods

        public abstract void Act(EnemyModel enemyModel);

        public virtual void TakeDamage(EnemyModel instance, Damage damage)
        {
            instance.CurrentStats.BaseStats.CurrentHealthPoints -= damage.GetTotalDamage();

            if (instance.CurrentStats.BaseStats.CurrentHealthPoints <= 0)
            {
                instance.CurrentStats.BaseStats.IsDead = true;
            }
        }

        #endregion
    }
}
