namespace BeastHunter
{
    public sealed class AttackService : IService
    {
        #region Fields

        private readonly GameContext _context;
        private Damage _damage;

        #endregion


        #region ClassLifeCycles

        public AttackService(GameContext context)
        {
            _context = context;
            _damage = new Damage();
        }

        #endregion


        #region Methods

        public Damage CountDamage(Damage baseDamage, int receiverID, WeaponData usedWeapon = null)
        {
            return CountDamage(baseDamage, receiverID, new Stats(), usedWeapon);
        }

        public Damage CountDamage(Damage baseDamage, int receiverID, Stats dealerStats, WeaponData usedWeapon = null)
        {
            Stats receiverStats;

            
            if(_context.CharacterModel.InstanceID == receiverID)
            {
                receiverStats = _context.CharacterModel.CurrentStats;
            }
            else if (_context.NpcModels.ContainsKey(receiverID))
            {
                receiverStats = _context.NpcModels[receiverID].CurrentStats;
            }
            else
            {
                CustomDebug.Log("no active reciever for damage found");
                return new Damage();
            }

            if(usedWeapon == null)
            {
                _damage.PhysicalDamage = (baseDamage.PhysicalDamage + dealerStats.AttackStats.PhysicalPower) *
                        (1 - receiverStats.DefenceStats.PhysicalDamageResistance);
                _damage.StunProbability = baseDamage.StunProbability *
                    (1 - receiverStats.DefenceStats.StunProbabilityResistance);
                _damage.FireDamage = baseDamage.FireDamage *
                    (1 - receiverStats.DefenceStats.FireDamageResistance);
            }
            else
            {
                switch (usedWeapon.Type)
                {
                    case WeaponType.Melee:
                        _damage.PhysicalDamage = (baseDamage.PhysicalDamage + dealerStats.AttackStats.PhysicalPower) *
                           (1 - receiverStats.DefenceStats.PhysicalDamageResistance) *
                               usedWeapon.CurrentAttack.WeaponItem.Weight;
                        _damage.StunProbability = baseDamage.StunProbability *
                            (1 - receiverStats.DefenceStats.StunProbabilityResistance) *
                                usedWeapon.CurrentAttack.WeaponItem.Weight;
                        _damage.FireDamage = baseDamage.FireDamage *
                            (1 - receiverStats.DefenceStats.FireDamageResistance) *
                                usedWeapon.CurrentAttack.WeaponItem.Weight;
                        break;
                    case WeaponType.Shooting:
                        _damage.PhysicalDamage = baseDamage.PhysicalDamage * 
                            (1 - receiverStats.DefenceStats.PhysicalDamageResistance);
                        _damage.StunProbability = baseDamage.StunProbability * 
                            (1 - receiverStats.DefenceStats.StunProbabilityResistance);
                        _damage.FireDamage = baseDamage.FireDamage * 
                            (1 - receiverStats.DefenceStats.FireDamageResistance);
                        break;
                    case WeaponType.Throwing:
                        break;
                    default:
                        break;
                }
            }

            return _damage;
        }

        public void DealDamage(Damage countedDamage, int receiverID)
        {
            if(receiverID == _context.CharacterModel.InstanceID)
            {
                _context.CharacterModel.PlayerBehavior.TakeDamageEvent(countedDamage);
            }
            else if(_context.NpcModels.ContainsKey(receiverID))
            {
                _context.NpcModels[receiverID].TakeDamage(countedDamage);
            }
        }

        public void CountAndDealDamage(Damage baseDamage, int receiverID, Stats dealerStats, WeaponData usedWeapon = null)
        {
            DealDamage(CountDamage(baseDamage, receiverID, dealerStats, usedWeapon), receiverID);
        }

        public void CountAndDealDamage(Damage baseDamage, int receiverID, WeaponData usedWeapon = null)
        {
            DealDamage(CountDamage(baseDamage, receiverID, usedWeapon), receiverID);
        }

        #endregion
    }
}

