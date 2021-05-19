using UnityEngine;


namespace BeastHunter
{
    public abstract class EnemyData : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject _prefab;
        [SerializeField] private DataType _dataType;
        [SerializeField] private Stats _startStats;
        
        #endregion


        #region Properties

        public GameObject Prefab => _prefab;
        public DataType DataType => _dataType;
        public Stats StartStats => _startStats;

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
