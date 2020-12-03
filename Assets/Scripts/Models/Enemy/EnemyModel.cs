namespace BeastHunter
{
    public abstract class EnemyModel : ITakeDamage
    {
        #region Fields

        public float CurrentHealth;

        public bool IsDead;

        #endregion


        #region Metods

        public abstract void OnAwake();

        public abstract void Execute();

        public abstract EnemyStats GetStats();

        public abstract void OnTearDown();

        #endregion


        #region ITakeDamge

        public abstract void TakeDamage(Damage damage);

        #endregion
    }
}