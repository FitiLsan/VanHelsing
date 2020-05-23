namespace BeastHunter
{
    public sealed class AttackService : Service
    {
        #region Fields

        private Damage _damage;

        #endregion


        #region ClassLifeCycles

        public AttackService(Contexts contexts) : base(contexts)
        {
            _damage = new Damage();
        }

        #endregion


        #region Methods

        public Damage CountDamage(WeaponItem weapon, BaseStatsClass dealerStats, BaseStatsClass recieverStats)
        {
            _damage.PhysicalDamage = weapon.CurrentAttack.AttackDamage.PhysicalDamage * weapon.Weight * 
                dealerStats.PhysicalPower * (1 - recieverStats.PhysicalResistance);
            _damage.StunProbability = weapon.CurrentAttack.AttackDamage.StunProbability - recieverStats.StunResistance > 0 ?
                weapon.CurrentAttack.AttackDamage.StunProbability - recieverStats.StunResistance : 0;

            return _damage;
        }

        public Damage CountDamage(Damage weaponDamage, BaseStatsClass dealerStats, BaseStatsClass recieverStats)
        {
            _damage.PhysicalDamage = weaponDamage.PhysicalDamage * dealerStats.PhysicalPower * 
                (1 - recieverStats.PhysicalResistance);
            _damage.StunProbability = weaponDamage.StunProbability - recieverStats.StunResistance > 0 ?
                weaponDamage.StunProbability - recieverStats.StunResistance : 0;

            return _damage;
        }

        #endregion
    }
}

