namespace BeastHunter
{
    public sealed class AttackService : IService
    {
        #region Fields

        private readonly GameContext _context;
        private Damage _damage;
        private Stats _receiverStats;
        private Stats _dealerStats;

        #endregion


        #region ClassLifeCycles

        public AttackService(GameContext context)
        {
            _context = context;
            _damage = new Damage();
        }

        #endregion


        #region Methods

        public Damage CountDamage(Damage baseDamage, int receiverID, Stats dealerStats = null, WeaponData usedWeapon = null)
        {
            var powerModifier = true;
            if (dealerStats == null)
            {
                dealerStats = new Stats();
                powerModifier = false;
            }
            _dealerStats = dealerStats;
            _receiverStats = _context.CharacterModel.InstanceID == receiverID ?
            _context.CharacterModel.CurrentStats : _context.NpcModels[receiverID].CurrentStats;

            if (usedWeapon == null)
            {
                _damage.PhysicalDamageValue =
                    (baseDamage.PhysicalDamageValue + Services.SharedInstance.EffectsManager.GetPhysicalPower(baseDamage.PhysicalDamageType, _dealerStats, powerModifier)) *
                    (1 - Services.SharedInstance.EffectsManager.GetPhysicalResistance(baseDamage.PhysicalDamageType, _receiverStats));
                _damage.ElementDamageValue =
                    (baseDamage.ElementDamageValue + Services.SharedInstance.EffectsManager.GetElementPower(baseDamage.ElementDamageType, _dealerStats, powerModifier)) *
                    (1 - Services.SharedInstance.EffectsManager.GetElementResistance(baseDamage.ElementDamageType, _receiverStats));
            }
            else
            {
                switch (usedWeapon.Type)
                {
                    case WeaponType.Melee:
                        {
                            _damage.PhysicalDamageValue =
                                (baseDamage.PhysicalDamageValue + Services.SharedInstance.EffectsManager.GetPhysicalPower(baseDamage.PhysicalDamageType, _dealerStats, powerModifier)) *
                                (1 - Services.SharedInstance.EffectsManager.GetPhysicalResistance(baseDamage.PhysicalDamageType, _receiverStats)) * usedWeapon.CurrentAttack.WeaponItem.Weight;
                            _damage.ElementDamageValue =
                                (baseDamage.ElementDamageValue + Services.SharedInstance.EffectsManager.GetElementPower(baseDamage.ElementDamageType, _dealerStats, powerModifier)) *
                                (1 - Services.SharedInstance.EffectsManager.GetElementResistance(baseDamage.ElementDamageType, _receiverStats)) * usedWeapon.CurrentAttack.WeaponItem.Weight;
                        }
                        break;
                    case WeaponType.Shooting:
                        // What unique weapon value ? Mb distance ?
                        {
                            _damage.PhysicalDamageValue =
                                (baseDamage.PhysicalDamageValue + Services.SharedInstance.EffectsManager.GetPhysicalPower(baseDamage.PhysicalDamageType, _dealerStats, powerModifier)) *
                                (1 - Services.SharedInstance.EffectsManager.GetPhysicalResistance(baseDamage.PhysicalDamageType, _receiverStats)); //* usedWeapon.CurrentAttack.WeaponItem.Weight; need?
                            _damage.ElementDamageValue =
                                (baseDamage.ElementDamageValue + Services.SharedInstance.EffectsManager.GetElementPower(baseDamage.ElementDamageType, _dealerStats, powerModifier)) *
                                (1 - Services.SharedInstance.EffectsManager.GetElementResistance(baseDamage.ElementDamageType, _receiverStats)); //* usedWeapon.CurrentAttack.WeaponItem.Weight;
                        }
                        break;
                    case WeaponType.Throwing:
                        break;
                    default:
                        break;
                }
            }
            _damage.PhysicalDamageType = baseDamage.PhysicalDamageType;
            _damage.ElementDamageType = baseDamage.ElementDamageType;
            _damage.IsEffectDamage = baseDamage.IsEffectDamage;
            return _damage;
        }

        public void DealDamage(Damage countedDamage, int receiverID)
        {
            if (receiverID == _context.CharacterModel.InstanceID)
            {
                _context.CharacterModel.PlayerBehavior.TakeDamageEvent(countedDamage);
            }
            else if (_context.NpcModels.ContainsKey(receiverID))
            {
                _context.NpcModels[receiverID].TakeDamage(countedDamage);
            }
        }

        public void CountAndDealDamage(Damage baseDamage, int receiverID, Stats dealerStats = null, WeaponData usedWeapon = null)
        {
            var damage = CountDamage(baseDamage, receiverID, dealerStats, usedWeapon);
            DealDamage(damage, receiverID);
        }

        #endregion
    }
}

