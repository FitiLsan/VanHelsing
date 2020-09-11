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

        public Damage CountDamage(WeaponData weapon, BaseStatsClass dealerStats, BaseStatsClass recieverStats)
        {
            float weaponWeight = weapon is OneHandedWeaponData ? (weapon as OneHandedWeaponData).ActualWeapon.Weight :
                (weapon as TwoHandedWeaponData).CurrentAttack.WeaponItem.Weight;

            _damage.PhysicalDamage = weapon.CurrentAttack.AttackDamage.PhysicalDamage * weaponWeight *
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

        public Damage CountDamage(Damage weaponDamage, BaseStatsClass recieverStats)
        {
            _damage.PhysicalDamage = weaponDamage.PhysicalDamage * (1 - recieverStats.PhysicalResistance);
            _damage.StunProbability = weaponDamage.StunProbability * (1 - recieverStats.StunResistance);

            return _damage;
        }

        #endregion
    }
}

